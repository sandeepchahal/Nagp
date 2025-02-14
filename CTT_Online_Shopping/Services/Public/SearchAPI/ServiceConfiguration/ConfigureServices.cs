using Elastic.Clients.Elasticsearch;
using Elastic.Transport;

namespace SearchAPI.ServiceConfiguration;

public static class ConfigureServices
{
    public static async Task RegisterElasticClientServices(this IServiceCollection serviceCollection,
        ConfigurationManager configurationManager)
    {
        var elasticClient = await ConfigureConnection(configurationManager);
        serviceCollection.AddSingleton(elasticClient);
    }

    private static async Task<ElasticsearchClient> ConfigureConnection(ConfigurationManager configurationManager)
    {
        var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        Console.WriteLine($"Environment is  {env}");
            var configuration = configurationManager;
            ElasticsearchClientSettings settings;
            string? cloudUrl, defaultIndex, username, password;
            if (env == "Development")
            {
                var cloudId = configuration["ElasticSearch:CloudId"];
                var apiKey= configuration["ElasticSearch:api_key"];
                cloudUrl = configuration["ElasticSearch:CloudUrl"];
                defaultIndex = configuration["ElasticSearch:Index"];
                username = configuration["ElasticSearch:Username"]; 
                password = configuration["ElasticSearch:Password"];

                if (string.IsNullOrEmpty(cloudId))
                    throw new Exception("Elasticsearch CloudId is missing in the configuration");

                if (string.IsNullOrEmpty(apiKey))
                    throw new Exception("Elasticsearch API Key is missing in the configuration");
            }
            else
            {
                Console.WriteLine("Reading from product file");
                cloudUrl = configuration["ElasticSearch:CloudUrl"];
                username = configuration["ElasticSearch:Username"];
                password = configuration["ElasticSearch:Password"];
                defaultIndex = configuration["ElasticSearch:Index"];
            }
            
            if (string.IsNullOrEmpty(defaultIndex))
                throw new Exception("Default index is missing in the configuration");
                
            if (string.IsNullOrEmpty(cloudUrl))
                throw new Exception("Elasticsearch URL is missing in the configuration");

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                throw new Exception("Elasticsearch username or password is missing");

            settings = new ElasticsearchClientSettings(new Uri(cloudUrl))
                    .Authentication(new BasicAuthentication(username,password)).DefaultIndex(defaultIndex);
            
            
            var client = new ElasticsearchClient(settings);
            
            // Ping the Elasticsearch cluster to check the connection
            var pingResponse = await client.PingAsync();
            if (!pingResponse.IsValidResponse)
            {
                throw new Exception("Failed to connect to Elasticsearch cluster. Check your credentials and network settings.");
            }
            Console.WriteLine("Pined db successfully");
            return client;
    }
}