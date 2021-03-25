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
        public ReactiveCommand<Unit, Unit> LoadTodos { get; }

        private IEnumerable<Todo> _todos = Enumerable.Empty<Todo>();
        public IEnumerable<Todo> Todos
        {
            get => _todos;
            private set => this.RaiseAndSetIfChanged(ref _todos, value);
        }

        private ObservableAsPropertyHelper<bool> _loading;
        public bool Loading => _loading.Value;

        private CompositeDisposable _disposables { get; } = new CompositeDisposable();

        public IndexViewModel(ITodoRepository todoRepository)
        {
            LoadTodos = ReactiveCommand.CreateFromTask(async (cancellationToken) =>
            {
                Todos = Enumerable.Empty<Todo>();
                Todos = await todoRepository.ListTodosAsync(cancellationToken);
            });

            _loading = LoadTodos.IsExecuting
                .ToProperty(this, x => x.Loading);
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
