using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using NHibernate.Linq;

using nuSpec.Abstraction;

namespace nuSpec.NHibernate
{
    public class FutureValue<T> : IFutureValue<T>
    {
        private readonly global::NHibernate.IFutureValue<T> futureValue;

        public FutureValue(IQueryable<T> query) => this.futureValue = query.ToFutureValue() ?? throw new ArgumentNullException(nameof(query));

        public T Value => this.futureValue.Value;

        public Task<T> GetValueAsync(CancellationToken cancellationToken = default) =>
            this.futureValue.GetValueAsync(cancellationToken);
    }
}
