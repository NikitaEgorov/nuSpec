using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using NHibernate.Linq;

using nuSpec.Abstraction;

namespace nuSpec.NHibernate
{
    public class FutureEnumerable<T> : IFutureEnumerable<T>
    {
        private readonly global::NHibernate.IFutureEnumerable<T> futureEnumerable;

        public FutureEnumerable(IQueryable<T> querable) =>
            this.futureEnumerable = querable.ToFuture() ?? throw new ArgumentNullException(nameof(this.futureEnumerable));

        public Task<IEnumerable<T>> GetEnumerableAsync(CancellationToken cancellationToken = default) =>
            this.futureEnumerable.GetEnumerableAsync(cancellationToken);

        public IEnumerable<T> GetEnumerable() => this.futureEnumerable.GetEnumerable();
    }
}
