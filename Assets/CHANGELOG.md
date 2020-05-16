# Changelog

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

