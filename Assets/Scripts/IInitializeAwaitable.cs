using System.Threading;
using Cysharp.Threading.Tasks;

namespace CAFU.Core
{
    public interface IInitializeAwaitable
    {
        UniTask WaitUntilInitialized(CancellationToken cancellationToken = default);
    }
}