using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Tests
{
    public interface IDatabase<T> where T : class
    {
        IEnumerable<T> AddData(IEnumerable<T> data);

        T AddData(T data);

        T UpdateData(T data);

        IEnumerable<T> UpdateData(IEnumerable<T> data);

        IEnumerable<T> Get();
    }
}
