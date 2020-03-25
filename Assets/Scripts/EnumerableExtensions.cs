using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using JetBrains.Annotations;
using UniRx.Async;

namespace CAFU.Core
{
    [PublicAPI]
    [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
    public static class EnumerableExtensions
    {
        // Move to another package such as `CAFU.Utility`, if you want to expose as public.
        internal static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
        {
            return enumerable == null || !enumerable.Any();
        }

        public static void NotifyAll(this IEnumerable<IInitializeNotifiable> initializeNotifiables)
        {
            if (initializeNotifiables.IsNullOrEmpty())
            {
                return;
            }

            foreach (var initializeNotifiable in initializeNotifiables)
            {
                initializeNotifiable.Notify();
            }
        }

        public static async UniTask NotifyAsyncAll(this IEnumerable<IAsyncInitializeNotifiable> asyncInitializeNotifiables, CancellationToken cancellationToken = default)
        {
            await asyncInitializeNotifiables.NotifyAsyncAll(true, cancellationToken);
        }

        public static async UniTask NotifyAsyncAll(this IEnumerable<IAsyncInitializeNotifiable> asyncInitializeNotifiables, bool runInParallel, CancellationToken cancellationToken = default)
        {
            if (asyncInitializeNotifiables.IsNullOrEmpty())
            {
                return;
            }

            cancellationToken.ThrowIfCancellationRequested();
            if (runInParallel)
            {
                await UniTask.WhenAll(asyncInitializeNotifiables.Select(x => x.NotifyAsync(cancellationToken)));
            }
            else
            {
                foreach (var asyncInitializeNotifiable in asyncInitializeNotifiables)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    await asyncInitializeNotifiable.NotifyAsync(cancellationToken);
                }
            }
        }

        public static void NotifyAll(this IEnumerable<IFinalizeNotifiable> finalizeNotifiables)
        {
            if (finalizeNotifiables.IsNullOrEmpty())
            {
                return;
            }

            foreach (var finalizeNotifiable in finalizeNotifiables)
            {
                finalizeNotifiable.Notify();
            }
        }

        public static async UniTask NotifyAsyncAll(this IEnumerable<IAsyncFinalizeNotifiable> asyncFinalizeNotifiables, CancellationToken cancellationToken = default)
        {
            await asyncFinalizeNotifiables.NotifyAsyncAll(true, cancellationToken);
        }

        public static async UniTask NotifyAsyncAll(this IEnumerable<IAsyncFinalizeNotifiable> asyncFinalizeNotifiables, bool runInParallel, CancellationToken cancellationToken = default)
        {
            if (asyncFinalizeNotifiables.IsNullOrEmpty())
            {
                return;
            }

            cancellationToken.ThrowIfCancellationRequested();
            if (runInParallel)
            {
                await UniTask.WhenAll(asyncFinalizeNotifiables.Select(x => x.NotifyAsync(cancellationToken)));
            }
            else
            {
                foreach (var asyncFinalizeNotifiable in asyncFinalizeNotifiables)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    await asyncFinalizeNotifiable.NotifyAsync(cancellationToken);
                }
            }
        }
    }
}