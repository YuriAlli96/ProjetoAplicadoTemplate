using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Common.Base
{
    public abstract class AggregateRoot : Entity
    {
        protected AggregateRoot() : base()
        {
        }
        protected AggregateRoot(long id) : base(id)
        {
        }
    }
}
