using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ZamzowD.ExampleBlazor.Models.Interfaces
{
    public interface ITodoRepository
    {
        Task<IEnumerable<Todo>> ListTodosAsync(CancellationToken cancellationToken = default);
    }
}
