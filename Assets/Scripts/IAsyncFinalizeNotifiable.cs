using System.Threading;
using UniRx.Async;

namespace CAFU.Core
{
    public interface IAsyncFinalizeNotifiable
    {
        UniTask NotifyAsync(CancellationToken cancellationToken = default);
    }
}