﻿using InternalService.Models;
using InternalService.Repository.Argument.Order;

namespace InternalService.Repository;

public interface IOrderRepository
{
    public Order Create(Order order);
    
    public IEnumerable<Order> GetAll();

    public Order Get(Guid id);
}