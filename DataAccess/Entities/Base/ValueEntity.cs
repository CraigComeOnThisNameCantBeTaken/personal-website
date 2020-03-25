using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Entities
{
    public class ValueEntity
    {
        public Guid Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
