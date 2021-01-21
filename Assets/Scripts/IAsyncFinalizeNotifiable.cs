using System.Threading;
using Cysharp.Threading.Tasks;

namespace CAFU.Core
{
    public interface IAsyncFinalizeNotifiable
    {
        UniTask OnFinalizeAsync(CancellationToken cancellationToken = default);
    }
}