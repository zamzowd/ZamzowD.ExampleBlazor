using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using ZamzowD.ExampleBlazor.Models;
using ZamzowD.ExampleBlazor.Models.Interfaces;

namespace ZamzowD.ExampleBlazor.Client.ViewModels
{
    public class IndexViewModel : ReactiveObject, IDisposable
    {
        public ReactiveCommand<int, Unit> LoadTodo { get; }

        private Todo _todo;
        public Todo Todo
        {
            get => _todo;
            private set => this.RaiseAndSetIfChanged(ref _todo, value);
        }

        private ObservableAsPropertyHelper<bool> _loading;
        public bool Loading => _loading.Value;

        private CompositeDisposable _disposables { get; } = new CompositeDisposable();

        public IndexViewModel(ITodoRepository todoRepository)
        {
            LoadTodo = ReactiveCommand.CreateFromTask<int>(
                async (id, cancellationToken) =>
                {
                    Todo = null;
                    Todo = await todoRepository.GetTodoAsync(id, cancellationToken);
                },
                outputScheduler: RxApp.MainThreadScheduler,
                canExecuteScheduler: RxApp.MainThreadScheduler
            );

            _loading = LoadTodo.IsExecuting
                .ToProperty(this, x => x.Loading, scheduler: RxApp.MainThreadScheduler);
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
