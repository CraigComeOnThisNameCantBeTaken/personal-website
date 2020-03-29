using System;
using System.Collections.Generic;
using System.Linq;
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

        public IEnumerable<T> AddData(IEnumerable<T> data)
        {
            var list = new List<T>();
            foreach (var ele in data)
            {
                list.Add(AddData(ele));
            }

            return list;
        }

        public T AddData(T data)
        {
            var dbSet = dataContext.Set<T>();
            dbSet.Add(data);
            Commit();

            return data;
        }

        public T UpdateData(T data)
        {
            var dbSet = dataContext.Set<T>();
            dbSet.Update(data);
            Commit();

            return data;
        }

        public IEnumerable<T> UpdateData(IEnumerable<T> data)
        {
            var dbSet = dataContext.Set<T>();
            dbSet.UpdateRange(data);
            Commit();

            return data;
        }

        private void Commit()
        {
            dataContext.SaveChanges();
        }

        public IEnumerable<T> Get()
        {
            return dataContext.Set<T>();
        }
    }
}
