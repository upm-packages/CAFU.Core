using System.Threading;
using UniRx.Async;

namespace CAFU.Core
{
    public interface IAsyncAutomaticSavableRepository
    {
        UniTask SaveAsync(CancellationToken cancellationToken = default);
    }
}