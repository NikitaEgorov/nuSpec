using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using NHibernate.Linq;

using nuSpec.Abstraction;

namespace nuSpec.NHibernate
{
    public class FetchableQueryable<TQueried> : IFetchableQueryable<TQueried>
    {
        public FetchableQueryable(IQueryable<TQueried> query) => this.Query = query;

        private IQueryable<TQueried> Query
        {
            get;
        }

        public IEnumerator<TQueried> GetEnumerator() => this.Query.GetEnumerator();

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => this.Query.GetEnumerator();

        public Type ElementType => this.Query.ElementType;

        public Expression Expression => this.Query.Expression;

        public IQueryProvider Provider => this.Query.Provider;

        public IFetchRequest<TQueried, TRelated> Fetch<TRelated>(Expression<Func<TQueried, TRelated>> relatedObjectSelector) =>
            new FetchRequest<TQueried, TRelated>(this.Query.Fetch(relatedObjectSelector));

        public IFetchRequest<TQueried, TRelated> FetchMany<TRelated>(
            Expression<Func<TQueried, IEnumerable<TRelated>>> relatedObjectSelector) =>
            new FetchRequest<TQueried, TRelated>(this.Query.FetchMany(relatedObjectSelector));
    }
}
