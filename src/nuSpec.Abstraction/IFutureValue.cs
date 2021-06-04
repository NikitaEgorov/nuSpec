using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace nuSpec.Abstraction
{
    public interface IFutureValue<T>
    {
        T Value { get; }

        Task<T> GetValueAsync(CancellationToken cancellationToken = default(CancellationToken));
    }

    public interface IFutureEnumerable<T>
    {
        Task<IEnumerable<T>> GetEnumerableAsync(CancellationToken cancellationToken = default(CancellationToken));

        IEnumerable<T> GetEnumerable();
    }
}
