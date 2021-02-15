using System;

namespace TR.SystemOfLegalCases.Domain.AbstractBaseModel
{
    public abstract class Entity
    {
        protected Entity()
        {
            Id = Guid.NewGuid();
        }

        public virtual Guid Id { get; set; }        
    }
}
