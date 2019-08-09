using System;
using System.Linq.Expressions;
using Enfo.Domain.Entities;

namespace Enfo.Domain.Specifications
{
    public class ActiveOrInactiveItemsSpec<TEntity> : Specification<TEntity> where TEntity : BaseEntity
    {
        private readonly ActiveInactive set;

        public ActiveOrInactiveItemsSpec(ActiveInactive set) => this.set = set;

        public override Expression<Func<TEntity, bool>> Criteria
        {
            get
            {
                switch (set)
                {
                    case ActiveInactive.ActiveOnly:
                        return e => e.Active;
                    case ActiveInactive.InactiveOnly:
                        return e => !e.Active;
                    default:
                        return null;
                }
            }
        }
    }

    public enum ActiveInactive
    {
        ActiveOnly,
        InactiveOnly,
        All,
    }
}
