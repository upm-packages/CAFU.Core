using System.Threading;
using UniRx.Async;

namespace CAFU.Core
{
    public interface IAsyncLoadableRepository
    {
        UniTask LoadAsync(CancellationToken cancellationToken = default);
    }
}