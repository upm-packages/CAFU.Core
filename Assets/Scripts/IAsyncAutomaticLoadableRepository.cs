using System.Threading;
using Cysharp.Threading.Tasks;

namespace CAFU.Core
{
    public interface IAsyncAutomaticLoadableRepository
    {
        UniTask LoadAutomaticallyAsync(CancellationToken cancellationToken = default);
    }
}