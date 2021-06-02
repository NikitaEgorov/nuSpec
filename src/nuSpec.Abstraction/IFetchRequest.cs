using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace nuSpec.Abstraction
{
    public interface IFetchRequest<out TDomainObject, TFetch> : IOrderedQueryable<TDomainObject>
    {
        IFetchRequest<TDomainObject, TRelated> ThenFetch<TRelated>(Expression<Func<TFetch, TRelated>> selector);

        IFetchRequest<TDomainObject, TRelated> ThenFetchMany<TRelated>(Expression<Func<TFetch, IEnumerable<TRelated>>> selector);
    }
}
