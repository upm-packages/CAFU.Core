using System.Threading;
using UniRx.Async;

namespace CAFU.Core
{
    public interface IInitializeAwaitable
    {
        UniTask WaitUntilInitialized(CancellationToken cancellationToken = default);
    }
}