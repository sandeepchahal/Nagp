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
        var cloudUrl = configurationManager["ElasticSearch:CloudUrl"];
        var username = configurationManager["ElasticSearch:Username"];
        var password = configurationManager["ElasticSearch:Password"];
        var defaultIndex = configurationManager["ElasticSearch:Index"];
        if (string.IsNullOrEmpty(defaultIndex))
            throw new Exception("Default index is missing in the configuration");

        if (string.IsNullOrEmpty(cloudUrl))
            throw new Exception("Elasticsearch URL is missing in the configuration");

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            throw new Exception("Elasticsearch username or password is missing");

        var settings = new ElasticsearchClientSettings(new Uri(cloudUrl))
            .Authentication(new BasicAuthentication(username, password)).DefaultIndex(defaultIndex);


        var client = new ElasticsearchClient(settings);

        // Ping the Elasticsearch cluster to check the connection
        var pingResponse = await client.PingAsync();
        if (!pingResponse.IsValidResponse)
        {
            throw new Exception(
                "Failed to connect to Elasticsearch cluster. Check your credentials and network settings.");
        }

        Console.WriteLine("Pined db successfully");
        return client;
    }
}