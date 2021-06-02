using System;
using System.Collections.Generic;
using System.Linq;

namespace nuSpec.Abstraction
{
    public abstract class FutureSpecification<TDomainObject, TProjection> : FutureSpecification<TDomainObject>
    {
        public Func<IQueryable<TDomainObject>, IQueryable<TProjection>> Projection
        {
            get;
            protected init;
        }
    }

    public abstract class FutureSpecification<TDomainObject> : IFetchebleSpecification<TDomainObject>
    {
        private readonly List<Func<IQueryable<TDomainObject>, IQueryable<TDomainObject>>> queries = new();

        private readonly List<Func<IFetchableQueryable<TDomainObject>, IQueryable<TDomainObject>>> fetches = new();

        public IEnumerable<Func<IQueryable<TDomainObject>, IQueryable<TDomainObject>>> Queries => this.queries;

        IEnumerable<Func<IFetchableQueryable<TDomainObject>, IQueryable<TDomainObject>>> IFetchebleSpecification<TDomainObject>.Fetches => this.fetches;

        protected void AddQuery(
            Func<IQueryable<TDomainObject>, IQueryable<TDomainObject>> query) =>
            this.queries.Add(query);

        protected void AddFetch(
            Func<IFetchableQueryable<TDomainObject>, IQueryable<TDomainObject>> fetch) =>
            this.fetches.Add(fetch);
    }
}
