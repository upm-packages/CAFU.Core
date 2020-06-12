using System.Threading;
using UniRx.Async;

namespace CAFU.Core
{
    public interface IAsyncAutomaticLoadableRepository
    {
        UniTask LoadAsync(CancellationToken cancellationToken = default);
    }
}