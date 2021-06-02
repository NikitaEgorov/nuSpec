using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using NHibernate.Linq;

using nuSpec.Abstraction;

namespace nuSpec.NHibernate
{
    public class FetchRequest<TQueried, TFetch> : IFetchRequest<TQueried, TFetch>
    {
        public FetchRequest(INhFetchRequest<TQueried, TFetch> nhFetchRequest) => this.NhFetchRequest = nhFetchRequest;

        private INhFetchRequest<TQueried, TFetch> NhFetchRequest
        {
            get;
        }

        public IEnumerator<TQueried> GetEnumerator() => this.NhFetchRequest.GetEnumerator();

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => this.NhFetchRequest.GetEnumerator();

        public Type ElementType => this.NhFetchRequest.ElementType;

        public Expression Expression => this.NhFetchRequest.Expression;

        public IQueryProvider Provider => this.NhFetchRequest.Provider;

        public IFetchRequest<TQueried, TRelated> Fetch<TRelated>(Expression<Func<TQueried, TRelated>> relatedObjectSelector)
        {
            var fetch = this.NhFetchRequest.Fetch(relatedObjectSelector);
            return new FetchRequest<TQueried, TRelated>(fetch);
        }

        public IFetchRequest<TQueried, TRelated> FetchMany<TRelated>(Expression<Func<TQueried, IEnumerable<TRelated>>> relatedObjectSelector)
        {
            var fecth = this.NhFetchRequest.FetchMany(relatedObjectSelector);
            return new FetchRequest<TQueried, TRelated>(fecth);
        }

        public IFetchRequest<TQueried, TRelated> ThenFetch<TRelated>(Expression<Func<TFetch, TRelated>> relatedObjectSelector)
        {
            var fetch = this.NhFetchRequest.ThenFetch(relatedObjectSelector);
            return new FetchRequest<TQueried, TRelated>(fetch);
        }

        public IFetchRequest<TQueried, TRelated> ThenFetchMany<TRelated>(Expression<Func<TFetch, IEnumerable<TRelated>>> relatedObjectSelector)
        {
            var fetch = this.NhFetchRequest.ThenFetchMany(relatedObjectSelector);
            return new FetchRequest<TQueried, TRelated>(fetch);
        }
    }
}
