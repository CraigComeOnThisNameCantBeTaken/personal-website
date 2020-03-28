using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Tests
{
    public interface IDatabase<T> where T : class
    {
        void AddData(IEnumerable<T> data);

        void AddData(T data);

        void UpdateData(T data);

        void UpdateData(IEnumerable<T> data);

        IEnumerable<T> Get();

        void Commit();
    }
}
