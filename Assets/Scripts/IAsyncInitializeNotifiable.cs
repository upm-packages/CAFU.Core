using System.Threading;
using Cysharp.Threading.Tasks;

namespace CAFU.Core
{
    public interface IAsyncInitializeNotifiable
    {
        UniTask OnInitializeAsync(CancellationToken cancellationToken = default);
    }
}