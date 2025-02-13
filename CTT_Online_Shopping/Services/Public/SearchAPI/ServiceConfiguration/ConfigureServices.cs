using Elastic.Clients.Elasticsearch;
using Elastic.Transport;

namespace SearchAPI.ServiceConfiguration;

public static class ConfigureServices
{
    public static async Task RegisterElasticClientServices(this IServiceCollection serviceCollection,
        ConfigurationManager configurationManager)
    {
        serviceCollection.AddSingleton<ElasticsearchClient>(_ =>
        {
            var configuration = configurationManager;

            var cloudUrl = configuration["ElasticSearch:CloudUrl"];
            var username = configuration["ElasticSearch:Username"];
            var password = configuration["ElasticSearch:Password"];
            var defaultIndex = configuration["ElasticSearch:Index"];

            if (string.IsNullOrEmpty(cloudUrl))
                throw new Exception("Elasticsearch URL is missing in the configuration");

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                throw new Exception("Elasticsearch username or password is missing");

            var settings = new ElasticsearchClientSettings(new Uri(cloudUrl))
                .Authentication(new BasicAuthentication(username, password))
                .DefaultIndex(defaultIndex);

            var client = new ElasticsearchClient(settings);
            
            // Ping the Elasticsearch cluster to check the connection
            var pingResponse = client.Ping();
            if (!pingResponse.IsValidResponse)
            {
                throw new Exception("Failed to connect to Elasticsearch cluster. Check your credentials and network settings.");
            }
            Console.WriteLine("Pined db successfully");
            return client;
        });
    }
}