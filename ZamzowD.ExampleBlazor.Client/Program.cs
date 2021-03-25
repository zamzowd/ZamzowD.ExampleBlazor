using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ZamzowD.ExampleBlazor.Client.Repositories;
using ZamzowD.ExampleBlazor.Client.ViewModels;
using ZamzowD.ExampleBlazor.Models.Interfaces;

namespace ZamzowD.ExampleBlazor.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddTransient(p => new HttpClient { BaseAddress = new Uri("https://jsonplaceholder.typicode.com/") });

            builder.Services.AddSingleton<ITodoRepository, TodoRepository>();

            builder.Services.AddTransient<IndexViewModel>();

            await builder.Build().RunAsync();
        }
    }
}
