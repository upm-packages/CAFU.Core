using System.Threading;
using UniRx.Async;

namespace CAFU.Core
{
    public interface IAsyncFinalizeNotifiable
    {
        UniTask OnFinalizeAsync(CancellationToken cancellationToken = default);
    }
}