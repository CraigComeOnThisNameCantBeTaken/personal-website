using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Entities
{
    public abstract class ValueEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
