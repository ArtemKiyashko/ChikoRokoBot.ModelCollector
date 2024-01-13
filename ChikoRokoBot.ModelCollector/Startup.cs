using System;
using System.Net;
using System.Net.Http;
using Azure.Data.Tables;
using Azure.Identity;
using ChikoRokoBot.ModelCollector.Clients;
using ChikoRokoBot.ModelCollector.Options;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

[assembly: FunctionsStartup(typeof(ChikoRokoBot.ModelCollector.Startup))]
namespace ChikoRokoBot.ModelCollector
{
	public class Startup : FunctionsStartup
    {
        private IConfigurationRoot _functionConfig;
        private ModelCollectorOptions _modelCollectorOptions = new();

        public override void Configure(IFunctionsHostBuilder builder)
        {
            _functionConfig = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build();

            builder.Services.Configure<ModelCollectorOptions>(_functionConfig.GetSection(nameof(ModelCollectorOptions)));
            _functionConfig.GetSection(nameof(ModelCollectorOptions)).Bind(_modelCollectorOptions);

            builder.Services.AddAzureClients(clientBuilder => {
                clientBuilder.UseCredential(new DefaultAzureCredential());

                if (Uri.TryCreate(_modelCollectorOptions.TableServiceConnection, UriKind.Absolute, out var tableServiceUri))
                    clientBuilder.AddTableServiceClient(tableServiceUri);
                else
                    clientBuilder.AddTableServiceClient(_modelCollectorOptions.TableServiceConnection);
            });

            builder.Services.AddScoped<TableClient>(factory => {
                var serviceClient = factory.GetRequiredService<TableServiceClient>();
                var options = factory.GetRequiredService<IOptions<ModelCollectorOptions>>().Value;
                var tableClient = serviceClient.GetTableClient(options.DropsTableName);
                tableClient.CreateIfNotExists();
                return tableClient;
            });

            builder.Services
                .AddHttpClient<ChikoRokoClient>(client => client.BaseAddress = _modelCollectorOptions.ChikoRokoBaseAddress)
                .ConfigurePrimaryHttpMessageHandler(() =>
                {
                    var messageHandler = new HttpClientHandler();
                    var cookieContainer = new CookieContainer();

                    cookieContainer.Add(_modelCollectorOptions.ChikoRokoBaseAddress, new Cookie("sessionid", _modelCollectorOptions.ChikoRokoSessionId));

                    messageHandler.CookieContainer = cookieContainer;
                    messageHandler.UseCookies = true;

                    return messageHandler;
                });
        }
    }
}

