using System.Threading;
using System.Threading.Tasks;

namespace nuSpec.Abstraction
{
    public interface INuFutureValue<T>
    {
        T Value { get; }

        Task<T> GetValueAsync(CancellationToken cancellationToken = default);
    }
}
