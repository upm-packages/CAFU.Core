using System.Threading;
using UniRx.Async;

namespace CAFU.Core
{
    public interface IAsyncInitializeNotifiable
    {
        UniTask NotifyAsync(CancellationToken cancellationToken = default);
    }
}