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

        IReadOnlyCollection<IQueryable<TProjection>> GetQuery<TDomainObject, TProjection>(
            IQueryable<TDomainObject> query,
            FutureSpecification<TDomainObject, TProjection> specification);

        IReadOnlyCollection<IQueryable<TDomainObject>> GetQuery<TDomainObject>(
            IQueryable<TDomainObject> query,
            FutureSpecification<TDomainObject> specification);
    }
}
