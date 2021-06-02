using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace nuSpec.Abstraction
{
    public interface IFetchableQueryable<TDomainObject> : IQueryable<TDomainObject>
    {
        IFetchRequest<TDomainObject, TRelated> Fetch<TRelated>(Expression<Func<TDomainObject, TRelated>> relatedObjectSelector);

        IFetchRequest<TDomainObject, TRelated> FetchMany<TRelated>(Expression<Func<TDomainObject, IEnumerable<TRelated>>> relatedObjectSelector);
    }
}
