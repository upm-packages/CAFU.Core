using System.Threading;
using UniRx.Async;

namespace CAFU.Core
{
    public interface IAsyncAutomaticSavableRepository
    {
        UniTask SaveAutomaticallyAsync(CancellationToken cancellationToken = default);
    }
}