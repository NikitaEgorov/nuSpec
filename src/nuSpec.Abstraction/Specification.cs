using System;
using System.Collections.Generic;
using System.Linq;

namespace nuSpec.Abstraction
{
    public abstract class Specification<TDomainObject> : Specification<TDomainObject, TDomainObject>
    {
    }

    public abstract class Specification<TDomainObject, TProjection> : IFetchableSpecification<TDomainObject>
    {
        private readonly List<Func<IFetchableQueryable<TDomainObject>, IQueryable<TDomainObject>>> fetches = new();

        public Func<IQueryable<TDomainObject>, IQueryable<TProjection>>? Query
        {
            get;
            protected set;
        }

        IEnumerable<Func<IFetchableQueryable<TDomainObject>, IQueryable<TDomainObject>>> IFetchableSpecification<TDomainObject>.
            Fetches =>
            this.fetches;

        protected void AddFetch(
            Func<IFetchableQueryable<TDomainObject>, IQueryable<TDomainObject>> fetch) =>
            this.fetches.Add(fetch);
    }
}
