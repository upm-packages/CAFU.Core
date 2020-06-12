using System.Threading;
using UniRx.Async;

namespace CAFU.Core
{
    public interface IAsyncAutomaticLoadableRepository
    {
        UniTask LoadAutomaticallyAsync(CancellationToken cancellationToken = default);
    }
}