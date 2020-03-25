![](https://github.com/upm-packages/CAFU.Core/workflows/Publish%20UPM%20Package/badge.svg)

# CAFU Core

## Installation

```bash
upm add package dev.upm-packages.cafu-core
```

Note: `upm` command is provided by [this repository](https://github.com/upm-packages/upm-cli).

You can also edit `Packages/manifest.json` directly.

```jsonc
{
  "dependencies": {
    // (snip)
    "dev.upm-packages.cafu-core": "[latest version]",
    // (snip)
  },
  "scopedRegistries": [
    {
      "name": "Unofficial Unity Package Manager Registry",
      "url": "https://upm-packages.dev",
      "scopes": [
        "dev.upm-packages"
      ]
    }
  ]
}
```

## Usages

### Interfaces for object lifecycle

This package provides below interfaces.

- `IInitializeNotifiable`
- `IAsyncInitializeNotifiable`
- `IFinalizeNotifiable`
- `IAsyncFinalizeNotifiable`

Call them each at the appropriate time.

```cs
using System;
using System.Collections.Generic;
using System.Linq;
using UniRx.Async;
using Zenject;
using CAFU.Core;

class Foo : IInitializable, IDisposable
{
    [Inject] private IEnumerable<IInitializeNotifiable> InitializeNotifiables { get; }
    [Inject] private IEnumerable<IAsyncInitializeNotifiable> AsyncInitializeNotifiables { get; }
    [Inject] private IEnumerable<IFinalizeNotifiable> FinalizeNotifiables { get; }
    [Inject] private IEnumerable<IAsyncFinalizeNotifiable> AsyncFinalizeNotifiables { get; }

    void IInitializable.Initialize()
    {
        // Invoke synchronous
        if (InitializeNotifiables != default && InitializeNotifiables.Any())
        {
            foreach (var initializeNotifiable in InitializeNotifiables)
            {
                initializeNotifiable.Notify();
            }
        }

        // Invoke asynchronous (Recommend strongly to manage CancellationToken)
        if (AsyncInitializeNotifiables != default && AsyncInitializeNotifiables.Any())
        {
            foreach (var asyncInitializeNotifiable in AsyncInitializeNotifiables)
            {
                asyncInitializeNotifiable.NotifyAsync().Forget(UnityEngine.Debug.LogException);
            }
        }
    }

    void IDisposable.Dispose()
    {
        // Invoke synchronous
        if (FinalizeNotifiables != default && FinalizeNotifiables.Any())
        {
            foreach (var finalizeNotifiable in FinalizeNotifiables)
            {
                finalizeNotifiable.Notify();
            }
        }

        // Invoke asynchronous (Recommend strongly to manage CancellationToken)
        if (AsyncFinalizeNotifiables != default && AsyncFinalizeNotifiables.Any())
        {
            foreach (var asyncFinalizeNotifiable in AsyncFinalizeNotifiables)
            {
                asyncFinalizeNotifiable.NotifyAsync().Forget(UnityEngine.Debug.LogException);
            }
        }
    }
}
```

### Extension methods

This package provides below extension methods.

- `IEnumerable<IInitializeNotifiable>.NotifyAll()`
- `IEnumerable<IAsyncInitializeNotifiable>.NotifyAsyncAll()`
- `IEnumerable<IFinalizeNotifiable>.NotifyAll()`
- `IEnumerable<IAsyncFinalizeNotifiable>.NotifyAsyncAll()`

These methods internally check the NULL of IEnumerable and so on, so the caller doesn't have to do it.

```cs
using System;
using System.Collections.Generic;
using UniRx.Async;
using Zenject;
using CAFU.Core;

class Foo : IInitializable, IDisposable
{
    [Inject] private IEnumerable<IInitializeNotifiable> InitializeNotifiables { get; }
    [Inject] private IEnumerable<IAsyncInitializeNotifiable> AsyncInitializeNotifiables { get; }
    [Inject] private IEnumerable<IFinalizeNotifiable> FinalizeNotifiables { get; }
    [Inject] private IEnumerable<IAsyncFinalizeNotifiable> AsyncFinalizeNotifiables { get; }

    void IInitializable.Initialize()
    {
        // Invoke synchronous
        InitializeNotifiables.NotifyAll();
        // Invoke asynchronous (Recommend strongly to manage CancellationToken)
        AsyncInitializeNotifiables.NotifyAsyncAll().Forget(UnityEngine.Debug.LogException);
    }

    void IDisposable.Dispose()
    {
        // Invoke synchronous
        FinalizeNotifiables.NotifyAll();
        // Invoke asynchronous (Recommend strongly to manage CancellationToken)
        AsyncFinalizeNotifiables.NotifyAsyncAll().Forget(UnityEngine.Debug.LogException);
    }
}
```

### Base class for Controller layer

This package provides base class for Controller layer of CAFU: Clean Architecture For Unity.

Since it often triggers the initialization of the Controller as the starting point for processing, we provide a Base Class.

This is the base class for implicitly executing the Initialize/Finalize operations in the Controller layer, such as the sample code above.

```cs
using CAFU.Core

class Foo : ControllerBase
{
    // Invoke methods automatically on initialize/finalize if interfaces are bounds
}
```
