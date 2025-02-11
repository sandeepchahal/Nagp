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

            var cloudId = configuration["ElasticSearch:CloudId"]; // Use CloudId instead of CloudUrl
            var apiKey = configuration["ElasticSearch:api_key"];
            var cloudUrl = configuration["ElasticSearch:CloudUrl"];
            var defaultIndex = configuration["ElasticSearch:Index"];
            var username = configuration["ElasticSearch:Username"]; 
            var password = configuration["ElasticSearch:Password"];

            if (string.IsNullOrEmpty(cloudId))
                throw new Exception("Elasticsearch CloudId is missing in the configuration");

            if (string.IsNullOrEmpty(apiKey))
                throw new Exception("Elasticsearch API Key is missing in the configuration");

            if (string.IsNullOrEmpty(defaultIndex))
                throw new Exception("Default index is missing in the configuration");

            

            var settings = new ElasticsearchClientSettings(new Uri(cloudUrl!))
                .Authentication(new BasicAuthentication(username!,password!)).DefaultIndex(defaultIndex);
            
            var client = new ElasticsearchClient(settings);
            
            // Ping the Elasticsearch cluster to check the connection
            var pingResponse = client.Ping();
            if (!pingResponse.IsValidResponse)
            {
                throw new Exception("Failed to connect to Elasticsearch cluster. Check your credentials and network settings.");
            }
            else
            {
                Console.WriteLine("Pined db successfully");
            }
            
            return client;
        });
    }
}