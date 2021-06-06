using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace nuSpec.Abstraction
{
    public interface INuFutureEnumerable<T>
    {
        Task<IEnumerable<T>> GetEnumerableAsync(CancellationToken cancellationToken = default);

        IEnumerable<T> GetEnumerable();
    }
}
