using System.Collections.Generic;
using System.Linq;

namespace nuSpec.Abstraction
{
    public interface ISpecificationEvalualor
    {
        IQueryable<TProjection> GetQuery<TDomainObject, TProjection>(
            IQueryable<TDomainObject> query,
            Specification<TDomainObject, TProjection> specification);

        IQueryable<TDomainObject> GetQuery<TDomainObject>(
            IQueryable<TDomainObject> query,
            Specification<TDomainObject> specification);

        IFutureValue<TProjection> GetFutureValue<TDomainObject, TProjection>(
            IQueryable<TDomainObject> query,
            Specification<TDomainObject, TProjection> specification);

        IFutureEnumerable<TProjection> GetFuture<TDomainObject, TProjection>(
            IQueryable<TDomainObject> query,
            Specification<TDomainObject, TProjection> specification);
    }
}
