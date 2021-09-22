using DAL.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.UnitOfWork
{
    public interface IUnitOfWork
    {
        //generic repository reference for an interface with template T
        IGenericRepository<T> GenericRepository<T>() where T : class;
        void Save();
    }
}
