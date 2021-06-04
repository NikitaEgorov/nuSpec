using System.Linq;

using nuSpec.Abstraction;

namespace nuSpec.NHibernate
{
    public class NhSpecificationEvaluator : ISpecificationEvaluator
    {
        public IQueryable<TProjection> GetQuery<TDomainObject, TProjection>(
            IQueryable<TDomainObject> query,
            Specification<TDomainObject, TProjection> specification)
        {
            var queryable = ApplyFetch(query, specification);

            return specification.Query(queryable);
        }

        public IFutureValue<TProjection> GetFutureValue<TDomainObject, TProjection>(
            IQueryable<TDomainObject> query,
            Specification<TDomainObject, TProjection> specification)
        {
            var queryable = ApplyFetch(query, specification);

            return new FutureValue<TProjection>(specification.Query(queryable));
        }

        public IFutureEnumerable<TProjection> GetFuture<TDomainObject, TProjection>(
            IQueryable<TDomainObject> query,
            Specification<TDomainObject, TProjection> specification)
        {
            var queryable = ApplyFetch(query, specification);

            return new FutureEnumerable<TProjection>(specification.Query(queryable));
        }

        public IQueryable<TDomainObject> GetQuery<TDomainObject>(
            IQueryable<TDomainObject> query,
            Specification<TDomainObject> specification) =>
            this.GetQuery(query, (Specification<TDomainObject, TDomainObject>)specification);

        private static IQueryable<TDomainObject> ApplyFetch<TDomainObject>(
            IQueryable<TDomainObject> query,
            IFetchableSpecification<TDomainObject> specification) =>
            specification.Fetches.Aggregate(
                                            query,
                                            (current, applyFetch) => applyFetch(new FetchableQueryable<TDomainObject>(current)));
    }
}
