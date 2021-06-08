using System;
using System.Linq;
using System.Linq.Expressions;

using nuSpec.Abstraction;

namespace nuSpec.NHibernate
{
    public class NhSpecificationEvaluator : ISpecificationEvaluator
    {
        public IQueryable<TProjection> GetQuery<TDomainObject, TProjection>(
            IQueryable<TDomainObject> query,
            Specification<TDomainObject, TProjection> specification)
        {
            if (specification.Query == null)
            {
                throw new ArgumentNullException(nameof(specification.Query), "Specification Query should be initialized");
            }

            var queryable = ApplyFetch(query, specification);

            return specification.Query(queryable);
        }

        public INuFutureValue<TProjection> GetFutureValue<TDomainObject, TProjection>(
            IQueryable<TDomainObject> query,
            Specification<TDomainObject, TProjection> specification)
        {
            if (specification.Query == null)
            {
                throw new ArgumentNullException(nameof(specification.Query), "Specification Query should be initialized");
            }

            var queryable = ApplyFetch(query, specification);

            return new FutureValue<TProjection>(specification.Query(queryable));
        }

        public INuFutureValue<TResult> GetFutureValue<TDomainObject, TProjection, TResult>(
            IQueryable<TDomainObject> query,
            Specification<TDomainObject, TProjection> specification,
            Expression<Func<IQueryable<TProjection>, TResult>> selector)
        {
            if (specification.Query == null)
            {
                throw new ArgumentNullException(nameof(specification.Query), "Specification Query should be initialized");
            }

            var queryable = ApplyFetch(query, specification);

            return new FutureValue<TProjection, TResult>(specification.Query(queryable), selector);
        }

        public INuFutureEnumerable<TProjection> GetFuture<TDomainObject, TProjection>(
            IQueryable<TDomainObject> query,
            Specification<TDomainObject, TProjection> specification)
        {
            if (specification.Query == null)
            {
                throw new ArgumentNullException(nameof(specification.Query), "Specification Query should be initialized");
            }

            var queryable = ApplyFetch(query, specification);

            return new FutureEnumerable<TProjection>(specification.Query(queryable));
        }

        private static IQueryable<TDomainObject> ApplyFetch<TDomainObject>(
            IQueryable<TDomainObject> query,
            IFetchableSpecification<TDomainObject> specification) =>
            specification.Fetches.Aggregate(
                                            query,
                                            (current, applyFetch) => applyFetch(new FetchableQueryable<TDomainObject>(current)));
    }
}
