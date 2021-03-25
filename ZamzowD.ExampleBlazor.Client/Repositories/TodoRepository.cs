using ZamzowD.ExampleBlazor.Models;
using ZamzowD.ExampleBlazor.Models.Interfaces;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ZamzowD.ExampleBlazor.Client.Repositories
{
    public class TodoRepository : ITodoRepository
    {
        private readonly HttpClient _http;

        public TodoRepository(HttpClient http)
        {
            _http = http;
        }

        public Task<IEnumerable<Todo>> ListTodosAsync(CancellationToken cancellationToken = default)
        {
            return _http.GetFromJsonAsync<IEnumerable<Todo>>("todos", cancellationToken);
        }
    }
}
