using System;
using System.Collections.Generic;
using System.Text;
using DataAccess.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Tests.EntityFramework
{
    public class EntityFrameworkDatabase<T> : IDatabase<T> where T : class
    {
        public readonly DataContext dataContext;

        public EntityFrameworkDatabase()
        {
            var dbOptions = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            dataContext = new DataContext(dbOptions);
        }

        public void AddData(IEnumerable<T> data)
        {
            var dbSet = dataContext.Set<T>();
            dbSet.AddRange(data);
        }

        public void AddData(T data)
        {
            var dbSet = dataContext.Set<T>();
            dbSet.Add(data);
        }

        public void UpdateData(T data)
        {
            var dbSet = dataContext.Set<T>();
            dbSet.Update(data);
        }

        public void UpdateData(IEnumerable<T> data)
        {
            var dbSet = dataContext.Set<T>();
            dbSet.UpdateRange(data);
        }

        public void Commit()
        {
            dataContext.SaveChanges();
        }

        public IEnumerable<T> Get()
        {
            return dataContext.Set<T>();
        }
    }
}
