using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using Cysharp.Threading.Tasks;
using Zenject;

namespace CAFU.Core
{
    [SuppressMessage("ReSharper", "SuspiciousTypeConversion.Global")]
    public abstract class Base : IInitializable, IDisposable
    {
        private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private bool isDisposed = false;

        public void LinkCancellationToken(CancellationToken cancellationToken)
        {
            cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(GetCancellationToken(), cancellationToken);
        }

        protected virtual CancellationToken GetCancellationToken()
        {
            return cancellationTokenSource.Token;
        }

        protected virtual void OnError(Exception exception)
        {
            UnityEngine.Debug.LogException(exception);
            cancellationTokenSource.Cancel();
            throw exception;
        }

        void IInitializable.Initialize()
        {
            if (this is IInitializeNotifiable initializeNotifiable)
            {
                initializeNotifiable.OnInitialize();
            }

            if (this is IAsyncInitializeNotifiable asyncInitializeNotifiable)
            {
                asyncInitializeNotifiable
                    .OnInitializeAsync(GetCancellationToken())
                    .Forget(OnError);
            }
        }

        void IDisposable.Dispose()
        {
            if (isDisposed)
            {
                return;
            }

            isDisposed = true;

            if (this is IFinalizeNotifiable finalizeNotifiable)
            {
                finalizeNotifiable.OnFinalize();
            }

            if (this is IAsyncFinalizeNotifiable asyncFinalizeNotifiable)
            {
                asyncFinalizeNotifiable
                    .OnFinalizeAsync(GetCancellationToken())
                    .ContinueWith(cancellationTokenSource.Dispose)
                    .Forget(OnError);
            }
        }
    }
}