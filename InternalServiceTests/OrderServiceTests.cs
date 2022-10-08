using AutoMapper;
using InternalService.AutoMapperProfiles;
using InternalService.Models;
using InternalService.Service;
using InternalService.Service.Argument.Order;
using InternalService.Service.DishService;
using InternalService.Service.OrderService;
using Moq;

namespace InternalServiceTests;

public class OrderServiceTests
{
    private Mock<IOrderRepository> _repository;
    private Mock<IDishService> _dishService;
    private Mapper _mapper;

    [SetUp]
    public void Init()
    {
        _repository = new Mock<IOrderRepository>();
        _dishService = new Mock<IDishService>();
        var orderProfile = new OrderMapperConfiguration();
        var dishProfile = new DishMapperConfiguration();
        var configuration = new MapperConfiguration(cfg => 
            cfg.AddProfiles(new Profile[]{orderProfile, dishProfile}));
        _mapper = new Mapper(configuration);
    }

    [Test]
    public void GetAll_ReturnsAllOrders()
    {
          //Arrange
          var orders = GetTestOrders();
          _repository.Setup(r => r.GetAll()).Returns(orders);
          var service = new OrderService(_repository.Object, _dishService.Object, _mapper);
          
          //Act
          var result = service.GetAll();
          
          //Assert
          Assert.IsNotNull(result);
          Assert.IsTrue(result.Count() == 2);
    }

    [Test]
    public void Get_ReturnsOrder()
    {
        //Arrange
        var testId = Guid.NewGuid();
        var testOrder = new Order() { Id = testId };
        _repository.Setup(r => r.Get(testId)).Returns(testOrder);
        var service = new OrderService(_repository.Object, _dishService.Object, _mapper);
        
        //Act
        var result = service.Get(testId);
        
        //Assert
        _repository.Verify(r => r.Get(testId));
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Id == testId);

    }

    [Test]
    public void Create_InputsArgument_CallsServices()
    {
       //Arrange
       var testArgument = GetTestArgument();
       _dishService.Setup(s => s.Get(It.IsAny<Guid>())).Returns(new Dish());
       var service = new OrderService(_repository.Object, _dishService.Object, _mapper);
       
       //Act
       service.Create(testArgument);
       
       //Assert
       _repository.Verify(r => r.Create(It.IsAny<Order>()));
       _dishService.Verify(s => s.Get(It.IsAny<Guid>()));

    }

    [Test]
    public void Create_InputsArgument_ReturnsRightPrice()
    {
        //Assert
        var testArgument = GetTestArgument();
        decimal expectedPrice = 0;
        foreach (var d in testArgument.Dishes)
        {
            _dishService.Setup(s => s.Get(d.Id)).Returns(d);
            expectedPrice += d.Price;
        }
        var service = new OrderService(_repository.Object, _dishService.Object, _mapper);
        
        //Act
        service.Create(testArgument);
        
        //Assert
        _repository.Verify(r => r
            .Create(It.
                Is<Order>(order => order.Price == expectedPrice )));

    }



    private CreateOrderArgument GetTestArgument()
    {
        var argument = new CreateOrderArgument()
        {
            Customer = "ted7007@yandex.ru",
            EmployeeId = Guid.Parse("d0e5d24e-ea32-4262-8b32-f273341c6ef5"),
            Type = OrderType.Offline,
            Dishes = new List<Dish>()
            {
                new()
                {
                    Id = Guid.Parse("d0e5d24e-ea32-4262-8b32-f273341c6ef4"),
                    Price = 100
                },
                new()
                {
                    Id = Guid.Parse("d0e5d24e-ea32-4262-8b32-f273341c6ef4"),
                    Price = 100
                }
            }
        };
        return argument;
    }
    private List<Order> GetTestOrders()
    {
        var orders = new List<Order>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Customer = "ted7007@yandex.ru",
                Price = 100,
                EmployeeId = Guid.Parse("d0e5d24e-ea32-4262-8b32-f273341c6ef5"),
                Type = OrderType.Offline,
                Dishes = new List<Dish>()
                {
                    new() { Id = Guid.Parse("d0e5d24e-ea32-4262-8b32-f273341c6ef4") }
                }
            },
            new()
            {
                Id = Guid.NewGuid(),
                Customer = "abc@yandex.ru",
                Price = 150,
                EmployeeId = Guid.Parse("d0e5d24e-ea32-4262-8b32-f273341c6ef5"),
                Type = OrderType.Online,
                Dishes = new List<Dish>()
                {
                    new() { Id = Guid.Parse("d0e5d24e-ea32-4262-8b32-f273341c6ef6") }
                }
            }
        };
        return orders;
    }
    
    
}