﻿using SuperMarket_DataAccess.Repository.IRepository.GenericInterface;
using SuperMarket_Models.Models;


namespace SuperMarket_DataAccess.Repository.IRepository
{
    public interface ICustomer:IRepository<Customer>
    {
        void Update(Customer obj);
    }
}
