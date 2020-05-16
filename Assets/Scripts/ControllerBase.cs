using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using UniRx.Async;
using Zenject;

namespace CAFU.Core
{
    [SuppressMessage("ReSharper", "VirtualMemberNeverOverridden.Global")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "SuspiciousTypeConversion.Global")]
    public abstract class ControllerBase : IInitializable, IDisposable
    {
        [InjectOptional] protected CancellationTokenSource CancellationTokenSource { get; set; }

        [InjectOptional] private IEnumerable<IInitializeNotifiable> InitializeNotifiables { get; }
        [InjectOptional] private IEnumerable<IAsyncInitializeNotifiable> AsyncInitializeNotifiables { get; }
        [InjectOptional] private IEnumerable<IFinalizeNotifiable> FinalizeNotifiables { get; }
        [InjectOptional] private IEnumerable<IAsyncFinalizeNotifiable> AsyncFinalizeNotifiables { get; }

        private bool isDisposed = false;

        protected virtual void OnInitialize()
        {
            if (InitializeNotifiables.IsNullOrEmpty())
            {
                return;
            }
            // Compare instance to avoid infinity-loop
            InitializeNotifiables.Where(x => !Equals(x, this)).NotifyAll();
        }

        protected virtual async UniTask OnInitializeAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (AsyncInitializeNotifiables.IsNullOrEmpty())
            {
                return;
            }
            // Compare instance to avoid infinity-loop
            await AsyncInitializeNotifiables.Where(x => !Equals(x, this)).NotifyAsyncAll(cancellationToken);
        }

        protected virtual void OnFinalize()
        {
            if (FinalizeNotifiables.IsNullOrEmpty())
            {
                return;
            }
            // Compare instance to avoid infinity-loop
            FinalizeNotifiables.Where(x => !Equals(x, this)).NotifyAll();
        }

        protected virtual async UniTask OnFinalizeAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (AsyncFinalizeNotifiables.IsNullOrEmpty())
            {
                return;
            }
            // Compare instance to avoid infinity-loop
            await AsyncFinalizeNotifiables.Where(x => !Equals(x, this)).NotifyAsyncAll(cancellationToken);
        }

        protected virtual void OnError(Exception exception)
        {
            UnityEngine.Debug.LogException(exception);
            CancellationTokenSource.Cancel();
            throw exception;
        }

        void IInitializable.Initialize()
        {
            // ReSharper disable once ConvertIfStatementToNullCoalescingAssignment
            if (CancellationTokenSource == default)
            {
                CancellationTokenSource = new CancellationTokenSource();
            }

            OnInitialize();
            OnInitializeAsync(CancellationTokenSource.Token)
                .Forget(OnError);
        }

        void IDisposable.Dispose()
        {
            if (isDisposed)
            {
                return;
            }

            isDisposed = true;
            OnFinalize();
            OnFinalizeAsync(CancellationTokenSource.Token)
                .ContinueWith(() => CancellationTokenSource.Dispose())
                .Forget(OnError);
        }
    }
}