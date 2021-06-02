using System;
using System.Collections.Generic;
using System.Linq;

namespace nuSpec.Abstraction
{
    public abstract class Specification<TDomainObject> : IFetchebleSpecification<TDomainObject>
    {
        public Func<IQueryable<TDomainObject>, IQueryable<TDomainObject>> Query
        {
            get;
            protected init;
        }

        private readonly List<Func<IFetchableQueryable<TDomainObject>, IQueryable<TDomainObject>>> fetches = new();

        IEnumerable<Func<IFetchableQueryable<TDomainObject>, IQueryable<TDomainObject>>> IFetchebleSpecification<TDomainObject>.Fetches => this.fetches;

        protected void AddFetch(
            Func<IFetchableQueryable<TDomainObject>, IQueryable<TDomainObject>> fetch) =>
            this.fetches.Add(fetch);
    }

    public abstract class Specification<TDomainObject, TProjection> : Specification<TDomainObject>
    {
        public Func<IQueryable<TDomainObject>, IQueryable<TProjection>> Projection
        {
            get;
            protected init;
        }
    }
}
