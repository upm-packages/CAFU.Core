using System.Threading;
using Cysharp.Threading.Tasks;

namespace CAFU.Core
{
    public interface IAsyncAutomaticSavableRepository
    {
        UniTask SaveAutomaticallyAsync(CancellationToken cancellationToken = default);
    }
}