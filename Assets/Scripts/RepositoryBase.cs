using System;
using System.Threading;
using JetBrains.Annotations;
using UniRx.Async;
using UnityEngine;
using Zenject;

namespace CAFU.Core
{
    [PublicAPI]
    public abstract class RepositoryBase : IInitializable, IDisposable, IInitializeAwaitable
    {
        [InjectOptional] protected CancellationTokenSource CancellationTokenSource { get; set; }

        public abstract UniTask LoadAsync(CancellationToken cancellationToken = default);
        public abstract UniTask SaveAsync(CancellationToken cancellationToken = default);

        private bool isInitialized = false;
        private bool isDisposed = false;

        public virtual async UniTask WaitUntilInitialized(CancellationToken cancellationToken = default)
        {
            await UniTask.WaitWhile(() => !isInitialized, cancellationToken: cancellationToken);
        }

        void IInitializable.Initialize()
        {
            // ReSharper disable once ConvertIfStatementToNullCoalescingAssignment
            if (CancellationTokenSource == default)
            {
                CancellationTokenSource = new CancellationTokenSource();
            }

            LoadAsync(CancellationTokenSource.Token)
                .ContinueWith(() => isInitialized = true)
                .Forget(OnError);
        }

        void IDisposable.Dispose()
        {
            if (isDisposed)
            {
                return;
            }

            isDisposed = true;
            SaveAsync(CancellationTokenSource.Token)
                .ContinueWith(() => CancellationTokenSource.Cancel())
                .Forget(OnError);
        }

        protected virtual void OnError(Exception exception)
        {
            Debug.LogException(exception);
            CancellationTokenSource.Cancel();
            throw exception;
        }
    }
}