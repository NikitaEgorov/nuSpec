using System.Linq;

namespace nuSpec.Abstraction
{
    public interface ISpecificationEvaluator
    {
        IQueryable<TProjection> GetQuery<TDomainObject, TProjection>(
            IQueryable<TDomainObject> query,
            Specification<TDomainObject, TProjection> specification);

        INuFutureValue<TProjection> GetFutureValue<TDomainObject, TProjection>(
            IQueryable<TDomainObject> query,
            Specification<TDomainObject, TProjection> specification);

        INuFutureEnumerable<TProjection> GetFuture<TDomainObject, TProjection>(
            IQueryable<TDomainObject> query,
            Specification<TDomainObject, TProjection> specification);
    }
}
