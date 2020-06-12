using System.Threading;
using UniRx.Async;

namespace CAFU.Core
{
    public abstract class RepositoryBase : Base, IAsyncInitializeNotifiable, IAsyncFinalizeNotifiable, IInitializeAwaitable, ICancellationTokenLinkable
    {
        private bool isInitialized = false;

        public virtual async UniTask WaitUntilInitialized(CancellationToken cancellationToken = default)
        {
            await UniTask.WaitWhile(() => !isInitialized, cancellationToken: cancellationToken);
        }

        public virtual async UniTask OnInitializeAsync(CancellationToken cancellationToken = default)
        {
            // ReSharper disable once SuspiciousTypeConversion.Global
            if (this is IAsyncAutomaticLoadableRepository asyncLoadableRepository)
            {
                await asyncLoadableRepository.LoadAsync(GetCancellationToken());
            }

            isInitialized = true;
        }

        public virtual async UniTask OnFinalizeAsync(CancellationToken cancellationToken = default)
        {
            // ReSharper disable once SuspiciousTypeConversion.Global
            if (this is IAsyncAutomaticSavableRepository asyncSavableRepository)
            {
                await asyncSavableRepository.SaveAsync(GetCancellationToken());
            }
        }
    }
}