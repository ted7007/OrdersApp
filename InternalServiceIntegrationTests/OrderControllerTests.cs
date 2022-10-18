using System.Collections.Immutable;
using System.Net.Http.Json;
using System.Text.Unicode;
using System.Web;
using AutoFixture;
using AutoFixture.Dsl;
using InternalService.Dto.Input.Order;
using InternalService.Models;
using InternalService.Repository;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace InternalServiceIntegrationTests;

public class OrderControllerTests : IClassFixture<IntegrationTestFactory<Program, ApplicationContext>>, IAsyncLifetime
{
     private readonly IntegrationTestFactory<Program, ApplicationContext> _factory;
     private readonly ApplicationContext _context;
     private readonly IFixture _fixture;
     private readonly IPostprocessComposer<Order> _orderCustomizationComposer;
     private readonly HttpClient _client;


     public OrderControllerTests(IntegrationTestFactory<Program, ApplicationContext> factory)
     {
          _factory = factory;
          _context = _factory.Services.GetRequiredService<ApplicationContext>();
          _fixture = new Fixture();

          _orderCustomizationComposer = _fixture
              .Build<Order>()
              .With(o => o.DateOfCreation, DateTime.UtcNow);
          _client = factory.CreateClient();

     }
     
     

     [Fact]
     public async Task GetList_ShouldReturn4Orders()
     {
          //Arrange
          var expectedOrders = _orderCustomizationComposer.CreateMany(4);
          await _context.Orders.AddRangeAsync(expectedOrders);
          _context.SaveChanges();
          
          //Act
          var response = await _client.GetAsync("api/v1/Order");
          var actual = await GetContent<IEnumerable<Order>>(response);
          
          //Assert
          Assert.NotNull(actual);
          Assert.Equal(expectedOrders.Count(), actual.Count());
          response.EnsureSuccessStatusCode();
     }

     [Fact]
     public async Task GetList_ShouldReturnPrice400()
     {
          //Arrange
          var expectedPrice = 400;
          var orderWithNecessaryPrice = _orderCustomizationComposer
               .With(o => o.Price, expectedPrice)
               .Create();
          var otherOrders = _orderCustomizationComposer
               .With(o => o.Price, expectedPrice-1)
               .CreateMany(4);
          await _context.Orders.AddAsync(orderWithNecessaryPrice);
          await _context.Orders.AddRangeAsync(otherOrders);
          await _context.SaveChangesAsync();
          
          //Act
          var response = await _client.GetAsync($"api/v1/Order?Price={expectedPrice}");
          var actual = await GetContent<IEnumerable<Order>>(response);
          
          //Assert
          Assert.NotNull(actual);
          Assert.True(actual.All(o => o.Price == 400));
          
     }

     [Fact]
     public async Task Create_ShouldProcessArgumentAndReturnRightOrder()
     {
         //Arrange
         var expectedDish = _fixture.Create<Dish>();
         var countOrdersBeforeAct = expectedDish.CountOrders;
         _context.Dishes.Add(expectedDish);
         await _context.SaveChangesAsync();
         
         var expectedOrder = _fixture
              .Build<CreateOrderDto>()
              .With(o => 
                   o.Dishes, new List<InputDishDto>{new (){ Id = expectedDish.Id}})
              .Create();

         //Act

         var response = _client.PostAsJsonAsync("api/v1/Order", expectedOrder).Result;
         var actualOrder = await GetContent<Order>(response);
         await _context.Entry(expectedDish).ReloadAsync();
         
         var actualDish = await _context.Dishes.FindAsync(expectedDish.Id);
         
         //Assert
         Assert.NotNull(actualDish);
         Assert.NotNull(actualOrder);
         Assert.True(actualDish.CountOrders-countOrdersBeforeAct == 1); // as added new order with this dish
         Assert.True(actualOrder.Id != Guid.Empty);
         Assert.True(actualOrder.Price == expectedDish.Price); // Order's price should be equal sum of dishes prices
     }

     [Fact]
     public async Task Get_ShouldReturnRightOrder()
     {
          //Arrange
          var expectedOrder = _orderCustomizationComposer
               .With(o => o.Dishes, new List<Dish>())
               .Create();
          _context.Orders.Add(expectedOrder);
          _context.SaveChanges();
          
          //Act
          var response = await _client.GetAsync($"api/v1/Order/{expectedOrder.Id}");
          var actual = await GetContent<Order>(response);
          
          //Assert
          Assert.NotNull(actual);
          Assert.Equal(actual.Id, expectedOrder.Id);
     }

     private async Task<T?> GetContent<T>(HttpResponseMessage response)
     {
          var content = await response.Content.ReadAsStringAsync();
          return JsonConvert.DeserializeObject<T>(content);
     }
     
     public async Task InitializeAsync()
     {
          await _context.Database.EnsureCreatedAsync();
     }

     public async Task DisposeAsync()
     {
          await _context.Database.EnsureDeletedAsync();
     }
}