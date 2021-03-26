using Microsoft.Reactive.Testing;
using NSubstitute;
using ReactiveUI.Testing;
using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Xunit;
using ZamzowD.ExampleBlazor.Client.ViewModels;
using ZamzowD.ExampleBlazor.Models;
using ZamzowD.ExampleBlazor.Models.Interfaces;

namespace ZamzowD.ExampleBlazor.Client.Tests
{
    public class IndexViewModelTests : ReactiveTest
    {
        private ITodoRepository _mockTodoRepository;

        public IndexViewModelTests()
        {
            _mockTodoRepository = Substitute.For<ITodoRepository>();
        }

        [Fact]
        public void LoadTodos_UpdatesIsExecuting()
        {
            new TestScheduler().With((scheduler) =>
            {
                var vm = new IndexViewModel(_mockTodoRepository);

                scheduler.ScheduleAbsolute(Subscribed + 10, () => vm.LoadTodo.Execute().Subscribe());

                scheduler.ScheduleAbsolute(Subscribed + 110, () => vm.LoadTodo.Execute().Subscribe());

                _mockTodoRepository.GetTodoAsync(default, default)
                    .ReturnsForAnyArgs(async (_) =>
                    {
                        await scheduler.Sleep(TimeSpan.FromTicks(50));
                        //scheduler.Sleep(50);
                        return new Todo { Id = 1, UserId = 1, Title = "title", Completed = true };
                    });

                var results = scheduler.Start(() => vm.LoadTodo.IsExecuting, Created, Subscribed, Disposed);

                var expected = new Recorded<Notification<bool>>[]
                {
                    OnNext(Subscribed, false),
                    OnNext(Subscribed + 11, true),
                    OnNext(Subscribed + 61, false),
                    OnNext(Subscribed + 111, true),
                    OnNext(Subscribed + 161, false)
                };
                ReactiveAssert.AreElementsEqual(expected, results.Messages);
            });
        }

        [Fact]
        public void LoadTodos_UpdatesLoading()
        {
            new TestScheduler().With((scheduler) =>
            {
                var vm = new IndexViewModel(_mockTodoRepository);

                scheduler.ScheduleAbsolute(Subscribed + 10, () => vm.LoadTodo.Execute().Subscribe());

                scheduler.ScheduleAbsolute(Subscribed + 110, () => vm.LoadTodo.Execute().Subscribe());

                _mockTodoRepository.GetTodoAsync(default, default)
                    .ReturnsForAnyArgs(async (_) =>
                    {
                        await scheduler.Sleep(TimeSpan.FromTicks(50));
                        //scheduler.Sleep(50);
                        return new Todo { Id = 1, UserId = 1, Title = "title", Completed = true };
                    });

                var todosObservable = Observable.FromEventPattern<PropertyChangedEventArgs>(vm, nameof(IndexViewModel.PropertyChanged), scheduler)
                    .Where((propertyChangedEvent) => propertyChangedEvent.EventArgs.PropertyName == nameof(IndexViewModel.Loading))
                    .Select((propertyChangedEvent) => ((IndexViewModel)propertyChangedEvent.Sender).Loading);

                var results = scheduler.Start(() => todosObservable, Created, Subscribed, Disposed);

                var expected = new Recorded<Notification<bool>>[]
                {
                    OnNext(Subscribed + 11, true),
                    OnNext(Subscribed + 61, false),
                    OnNext(Subscribed + 111, true),
                    OnNext(Subscribed + 161, false)
                };
                ReactiveAssert.AreElementsEqual(expected, results.Messages);
            });
        }

        [Fact]
        public void LoadTodos_UpdatesTodos()
        {
            new TestScheduler().With((scheduler) =>
            {
                var vm = new IndexViewModel(_mockTodoRepository);

                scheduler.ScheduleAbsolute(10, () => vm.LoadTodo.Execute().Subscribe());

                scheduler.ScheduleAbsolute(Subscribed + 10, () => vm.LoadTodo.Execute().Subscribe());

                scheduler.ScheduleAbsolute(Subscribed + 110, () => vm.LoadTodo.Execute().Subscribe());

                _mockTodoRepository.GetTodoAsync(default, default)
                    .ReturnsForAnyArgs(async (_) =>
                    {
                        await scheduler.Sleep(TimeSpan.FromTicks(50));
                        //scheduler.Sleep(50);
                        return new Todo { Id = 1, UserId = 1, Title = "title", Completed = true };
                    });

                var todosObservable = Observable.FromEventPattern<PropertyChangedEventArgs>(vm, nameof(IndexViewModel.PropertyChanged), scheduler)
                    .Where((propertyChangedEvent) => propertyChangedEvent.EventArgs.PropertyName == nameof(IndexViewModel.Todo))
                    .Select((propertyChangedEvent) => ((IndexViewModel)propertyChangedEvent.Sender).Todo);

                var results = scheduler.Start(() => todosObservable, Created, Subscribed, Disposed);

                var expected = new Recorded<Notification<Todo>>[]
                {
                    OnNext(Subscribed + 10, (Todo)null),
                    OnNext(Subscribed + 60, new Todo { Id = 1, UserId = 1, Title = "title", Completed = true }),
                    OnNext(Subscribed + 110, (Todo)null),
                    OnNext(Subscribed + 160, new Todo { Id = 1, UserId = 1, Title = "title", Completed = true })
                };
                ReactiveAssert.AreElementsEqual(expected, results.Messages);
            });
        }
    }
}
