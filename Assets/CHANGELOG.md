# Changelog

## [1.0.0-preview.8] - 2021-01-21

Reference UniRx on GitHub instead of Self-hosted registry

### Changes

- Reference UniRx on GitHub instead of Self-hosted registry
- Upgrade UniTask to v2

## [1.0.0-preview.7] - 2020-06-12

Rename interfaces and methods to automatic Load/Save repository

### Changes

- Add `Automatic` keyword into interface name and add `Automatically` keyword into method name. #7 

## [1.0.0-preview.6] - 2020-05-18

Add sync repository interfaces

* Add sync repository interfaces

## [0.1.0-preview.5] - 2020-05-17

Improve base classes

### Changes

- #5 ControllerBase が他レイヤ（UseCase）の初期化などを担うと処理が追いにくくなるので廃止

## [0.1.0-preview.4] - 2020-05-16

Add RepositoryBase

### Features

- Add `RepositoryBase` to invoke `LoadAsync()` at initialize and invoke `SaveAsync()` at finalize
- Add `IInitializeAwaitable` to explain can await initialize and provide `WaitUntilInitialized()`

### Fixes

-Shrink IL2CPP code size

## [0.1.0-preview.3] - 2020-05-14

[Destructive] Rename notify methods

### Changes

- Changes methods `I(Async)?(Initialize|Finalize)Notifiable.Notify()` to `On$2$1()`

## [0.1.0-preview.2] - 2020-05-07

Avoid warning

### Fixes

- Avoid warning for awaiting

## [0.1.0-preview.1] - 2020-03-26

Initial release

### Features

* Provides interfaces for object lifecycle
* Provides extension methods for `IEnumerable<TSomeNotifieable>`
* Provides `ControllerBase`

