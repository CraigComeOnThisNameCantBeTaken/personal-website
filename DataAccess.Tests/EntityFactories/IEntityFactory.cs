using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Tests.EntityFactories
{
    public interface IEntityFactory<T>
    {
        T Create();

        IEnumerable<T> CreateMany(int amount);
    }
}
