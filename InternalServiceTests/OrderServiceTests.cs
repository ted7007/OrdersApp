using AutoMapper;
using InternalService.AutoMapperProfiles;
using InternalService.Models;
using InternalService.Repository.Order;
using InternalService.Service.Argument.Order;
using InternalService.Service.DishService;
using InternalService.Service.OrderService;
using InternalService.Service.Param;
using Moq;

namespace InternalServiceTests;

public class OrderServiceTests
{
    private Mock<IOrderRepository> _repository;
    private Mock<IDishService> _dishService;
    private Mapper _mapper;
    private OrderService _sup;

    /// <summary>
    /// configuration of infrastructure for tests
    /// </summary>
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
        _sup = new OrderService(_repository.Object, _dishService.Object, _mapper);
    }

    /// <summary>
    /// test for getting value from repo
    /// </summary>
    [Test]
    public void GetList_ReturnsOrders()
    {
          //Arrange
          var ordersMock =new Mock<List<Order>>();
          var expected = ordersMock.Object;
          _repository.Setup(r => r
              .GetList(It.IsAny<Func<Order, bool>>()))
              .Returns(expected);
          var emptySearchParam = new OrderSearchParam();
          
          //Act
          var actual = _sup.GetList(emptySearchParam);
          
          //Assert
          
          _repository.Verify(r => r.
              GetList(It.
                  IsAny<Func<Order, bool>>()));
          Assert.That(actual,Is.EqualTo(expected));
    }

    /// <summary>
    /// test for getting value from repo
    /// </summary>
    [Test]
    public void Get_ReturnsOrder()
    {
        //Arrange
        var testId = new Guid();
        var expected = new Mock<Order>().Object;
        _repository.Setup(r => r.Get(It.IsAny<Guid>())).Returns(expected);
        var service = new OrderService(_repository.Object, _dishService.Object, _mapper);
        
        //Act
        var actual = service.Get(testId);
        
        //Assert
        _repository.Verify(r => r.Get(testId));
        Assert.That(actual, Is.EqualTo(expected));
                              
    }                                                   

    /// <summary>
    /// test for inputting/getting value in/from repo
    /// </summary>
    [Test]
    public void Create_InputArgument_CallsRepository()
    {
       //Arrange
       var testCreateArgument = new Mock<CreateOrderArgument>();
       var expectedOrder = new Mock<Order>().Object;
       _repository.Setup(r => r.Create(It.IsAny<Order>())).Returns(expectedOrder);
       var service = new OrderService(_repository.Object, _dishService.Object, _mapper);
       
       //Act
       var actual = service.Create(testCreateArgument.Object);
       
       //Assert
       _repository.Verify(r => r.Create(It.IsAny<Order>()));
       Assert.That(actual, Is.EqualTo(expectedOrder));

    }
    
    /// <summary>
    /// test for inputting/getting value in/from dishService
    /// </summary>
    [Test]
    public void Create_InputArgument_CallsDishService()
    {
        //Arrange
        var expectedOrder = new Mock<Order>();
        var expectedDish = new Dish() { Id = Guid.NewGuid()};
        var testCreateArgument = new CreateOrderArgument() { Dishes = new List<Dish> { expectedDish } };
        _repository
            .Setup(r => r.Create(It.IsAny<Order>()))
            .Returns(expectedOrder.Object);
        _dishService
            .Setup(s => s.Get(It.IsAny<Guid>()))
            .Returns(expectedDish);
        var service = new OrderService(_repository.Object, _dishService.Object, _mapper);
       
        //Act
        var actual = service.Create(testCreateArgument);
       
        //Assert
        _dishService.Verify(s => s.
            Get(It.
                Is<Guid>(i => i == expectedDish.Id)));
        Assert.That(actual, Is.EqualTo(expectedOrder.Object));
        

    }

    /// <summary>
    /// test for calculating price in order
    /// </summary>
    [Test]
    public void Create_InputsArgument_ReturnsRightPrice()
    {
        //Arrange
        var testArgument = GetTestArgument();
        decimal expectedPrice = 0;
        foreach (var d in testArgument.Dishes)
        {
            _dishService.Setup(s => s.Get(d.Id)).Returns(d);
            expectedPrice += d.Price;
        }
        
        //Act
        _sup.Create(testArgument); // i didnt imitate returns value by repository,
                                   // so i dont check actual value 
        
        //Assert
        _repository.Verify(r => r            // however i check order that inputs to repository 
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
}