#nullable enable

using System.Collections.Generic;
using System.Linq;

using nuSpec.Abstraction;

namespace nuSpec.NHibernate
{
    public class NHSpecificationEvalualor : ISpecificationEvalualor
    {
        public IReadOnlyCollection<IQueryable<TProjection>> GetQuery<TDomainObject, TProjection>(
            IQueryable<TDomainObject> query,
            FutureSpecification<TDomainObject, TProjection> specification) =>
            this.GetQuery(query, (FutureSpecification<TDomainObject>)specification)
                .Select(q => specification.Projection(q))
                .ToList();

        public IReadOnlyCollection<IQueryable<TDomainObject>> GetQuery<TDomainObject>(
            IQueryable<TDomainObject> query,
            FutureSpecification<TDomainObject> specification) =>
            specification
                .Queries
                .Select(func => ApplyFetch(func(query), specification))
                .ToList();

        public IQueryable<TProjection> GetQuery<TDomainObject, TProjection>(
            IQueryable<TDomainObject> query,
            Specification<TDomainObject, TProjection> specification)
        {
            var queryable = this.GetQuery(query, (Specification<TDomainObject>)specification);
            return specification.Projection(queryable);
        }

        public IQueryable<TDomainObject> GetQuery<TDomainObject>(
            IQueryable<TDomainObject> query,
            Specification<TDomainObject> specification)
        {
            var queryable = specification.Query(query);
            return ApplyFetch(queryable, specification);
        }

        private static IQueryable<TDomainObject> ApplyFetch<TDomainObject>(
            IQueryable<TDomainObject> query,
            IFetchebleSpecification<TDomainObject> specification) =>
            specification.Fetches.Aggregate(
                                            query,
                                            (current, applyFetch) => applyFetch(new FetchableQueryable<TDomainObject>(current)));
    }
}
