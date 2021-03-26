# ZamzowD.ExampleBlazor

A simple example Blazor WebAssembly project to demonstrate issues with testing.

## Unit Test Property from IsExecuting of Command from Task

Attempting to test the timing of a property mapped from IsExecuting of a Command created from a task.

The task is mocked with a delay using NSubstitute.

### await scheduler.Sleep(TimeSpan.FromTicks(50))

Results in only seeing the property change to `true`. While the process does continue to run, the unit test does not see subsequent changes from `IsExecuting`.

#### Expected

```
[OnNext(True)@211, OnNext(False)@261, OnNext(True)@311, OnNext(False)@361]
```

#### Actual

```
[OnNext(True)@211]
```

### scheduler.Sleep(50)

Results in seeing the property changing to `true` only after the Sleep period, since it does not actually release the thread.

(Using `await scheduler.Yield()` results in the same behavior as `await scheduler.Sleep(TimeSpan.FromTicks(50))`.)

#### Expected

```
[OnNext(True)@211, OnNext(False)@261, OnNext(True)@311, OnNext(False)@361]
```

#### Actual

```
[OnNext(True)@260, OnNext(False)@262, OnNext(True)@360, OnNext(False)@362]
```