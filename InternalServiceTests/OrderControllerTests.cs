using AutoMapper;
using InternalService.AutoMapperProfiles;
using InternalService.Controller;
using InternalService.Dto.Input.Order;
using InternalService.Dto.Output.Dish;
using InternalService.Dto.Output.Order;
using InternalService.Models;
using InternalService.Repository.Argument.Order;
using InternalService.Service.OrderService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SemanticComparison;

namespace InternalServiceTests;

[TestClass]
public class OrderControllerTests
{
    private IMapper _mapper;
    private Mock<IOrderService> _orderService;
    
    public OrderControllerTests()
    {
        var orderProfile = new OrderMapperConfiguration();
        var dishProfile = new DishMapperConfiguration();
        var configuration = new MapperConfiguration(cfg => 
                                                    cfg.AddProfiles(new Profile[]{orderProfile, dishProfile}));
        _mapper = new Mapper(configuration);
        _orderService = new Mock<IOrderService>();
    }
    
    
    [TestMethod]
    public void Create_InputsArgument_CallsServiceWithMappedArgument()
    {
        //Assert

        var createDto = GetTestCreateDto();
        var controller = new OrderController(_orderService.Object, _mapper);
            
        //Act
        controller.Create(createDto);

        //Assert
        _orderService.Verify(s => s.Create(It.IsAny<CreateOrderArgument>()));
    }

    [TestMethod]
    public void Create_InputsArgument_ReturnsValidOrderDto()
    {
        //Assert
        var createDto = GetTestCreateDto();

        _orderService.Setup(s => s
                .Create(It.IsAny<CreateOrderArgument>()))
                .Returns(new Order());
        var controller = new OrderController(_orderService.Object, _mapper);
        
        
        //Act
        var result = controller.Create(createDto).Result as OkObjectResult;
        
        //Assert
        Assert.IsNotNull(result);
        Assert.IsNotNull(result.Value);
        _orderService.Verify(s => s.Create(It.IsAny<CreateOrderArgument>()));

    }

    [TestMethod]
    public void GetAll_ReturnsAllOrders()
    {
        //Assert
        var testOrders = GetTestOrders();
        _orderService.Setup(s => s
                .GetAll())
                .Returns(testOrders);
        var controller = new OrderController(_orderService.Object, _mapper);
        
        //Act
        var result = controller.GetAll().Result as OkObjectResult;
        var resultList = result.Value as List<OrderDto>;
        
        //Assert
        Assert.IsNotNull(resultList);
        Assert.IsTrue(resultList.Count == 2);

    }
    
    private CreateOrderDto GetTestCreateDto()
    {
        var createDto = new CreateOrderDto
        {
            Customer = "ted7007@yandex.ru",
            Price = 100,
            EmployeeId = Guid.Parse("d0e5d24e-ea32-4262-8b32-f273341c6ef5"),
            Type = OrderType.Offline,
            Dishes = new List<InputDishDto>()
            {
                new() { Id = Guid.Parse("d0e5d24e-ea32-4262-8b32-f273341c6ef4") }
            }
        };
        return createDto;
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
                    new() { Id = Guid.Parse("d0e5d24e-ea32-4262-8b32-f273341c6ef4") }
                }
            }
        };
        return orders;
    }
}