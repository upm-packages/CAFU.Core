using System.Threading;
using UniRx.Async;

namespace CAFU.Core
{
    public interface IAsyncSavableRepository
    {
        UniTask SaveAsync(CancellationToken cancellationToken = default);
    }
}