using System;
using System.Collections.Generic;
using System.Linq;

namespace nuSpec.Abstraction
{
    public interface IFetchableSpecification<TDomainObject>
    {
        IEnumerable<Func<IFetchableQueryable<TDomainObject>, IQueryable<TDomainObject>>> Fetches
        {
            get;
        }
    }
}
