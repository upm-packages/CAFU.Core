using System;
using System.Collections;
using System.Linq;
using NSubstitute;
using UniRx.Async;
using UnityEngine.TestTools;
using Zenject;

namespace CAFU.Core.Tests.Runtime
{
    public class 基本インタフェース : ZenjectIntegrationTestFixture
    {
        [UnityTest]
        public IEnumerator IInitializeNotifiable_Notifyが呼ばれる() => UniTask.ToCoroutine(async () =>
        {
            InstallBasicInterfaceBindings<IInitializeNotifiable>();
            await UniTask.DelayFrame(1);
            var mock = Container.Resolve<IInitializeNotifiable>();
            mock.Received(1).Notify();
        });

        [UnityTest]
        public IEnumerator IAsyncInitializeNotifiable_NotifyAsyncが呼ばれる() => UniTask.ToCoroutine(async () =>
        {
            InstallBasicInterfaceBindings<IAsyncInitializeNotifiable>();
            await UniTask.DelayFrame(1);
            var mock = Container.Resolve<IAsyncInitializeNotifiable>();
            await mock.ReceivedWithAnyArgs(1).NotifyAsync();
        });

        [UnityTest]
        public IEnumerator IFinalizeNotifiable_Notifyが呼ばれる() => UniTask.ToCoroutine(async () =>
        {
            InstallBasicInterfaceBindings<IFinalizeNotifiable>();
            Container.ResolveAll<IDisposable>().First(x => x.GetType() == typeof(TestController)).Dispose();
            await UniTask.DelayFrame(1);
            var mock = Container.Resolve<IFinalizeNotifiable>();
            mock.Received(1).Notify();
        });

        [UnityTest]
        public IEnumerator IAsyncFinalizeNotifiable_NotifyAsyncが呼ばれる() => UniTask.ToCoroutine(async () =>
        {
            InstallBasicInterfaceBindings<IAsyncFinalizeNotifiable>();
            Container.ResolveAll<IDisposable>().First(x => x.GetType() == typeof(TestController)).Dispose();
            await UniTask.DelayFrame(1);
            var mock = Container.Resolve<IAsyncFinalizeNotifiable>();
            await mock.ReceivedWithAnyArgs(1).NotifyAsync();
        });

        private void InstallBasicInterfaceBindings<TInterface>() where TInterface : class
        {
            PreInstall();
            Container.BindInterfacesTo<TestController>().AsSingle().NonLazy();
            Container.Bind<TInterface>().FromInstance(Substitute.For<TInterface>()).AsSingle();
            PostInstall();
        }

        private class TestController : ControllerBase
        {
        }
    }
}