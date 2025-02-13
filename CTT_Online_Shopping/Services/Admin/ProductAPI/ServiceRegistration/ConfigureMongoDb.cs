using System.Text;
using System.Web;
using MongoDB.Bson;
using MongoDB.Driver;
using Polly;
using ProductAPI.Models;
using ProductAPI.Models.DbModels;

namespace ProductAPI.ServiceRegistration;

public static class ConfigurationDbRegistration
{
    public static void ConfigureMongoDb(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        try
        {
            var settings = MongoClientSettings.FromConnectionString(GetConnectionString(configuration));
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);
            settings.MaxConnectionIdleTime = TimeSpan.FromSeconds(30);
            settings.ConnectTimeout = TimeSpan.FromSeconds(10);

            var retryPolicy = Policy
                .Handle<MongoConnectionException>()
                .WaitAndRetry(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

            MongoClient mongoClient = null!;
            IMongoDatabase database = null!;

            retryPolicy.Execute(() =>
            {
                mongoClient = new MongoClient(settings);
                var databaseName = configuration["MongoDB:Database"];

                if (string.IsNullOrEmpty(databaseName))
                {
                    throw new InvalidOperationException("Database name doesn't exist in the configuration");
                }

                database = mongoClient.GetDatabase(databaseName);

                // Ensure MongoDB is reachable before proceeding
                database.RunCommand<BsonDocument>(new BsonDocument("ping", 1));
            });
            // Register MongoClient as a singleton service
            serviceCollection.AddSingleton(mongoClient);
             //Register IMongoDatabase as a singleton service
            serviceCollection.AddSingleton(database);
            serviceCollection.RegisterDbCollections();
        }
        catch
        {
            throw new InvalidOperationException("Connection to Mongo Db is refused!");
        }
    }
    private static void RegisterDbCollections(this IServiceCollection services)
    {
        services.AddScoped(sp => sp.GetRequiredService<IMongoDatabase>().GetCollection<CategoryDb>("categories"));
        services.AddScoped(sp => sp.GetRequiredService<IMongoDatabase>().GetCollection<ProductDb>("products"));
        services.AddScoped(sp => sp.GetRequiredService<IMongoDatabase>().GetCollection<ProductItemDb>("productItems"));
        services.AddScoped(sp => sp.GetRequiredService<IMongoDatabase>().GetCollection<BrandDb>("brands"));

    }
    private static string GetConnectionString(IConfiguration configuration)
    {
        // Fetch database and cluster name from appsettings.json
        var database = configuration["MongoDB:Database"];
        var clusterName = configuration["MongoDB:ClusterName"];
        var mongoDbHost = configuration["MongoDB:Host"];
        var mongoDbPort = configuration["MongoDB:Port"];
        
        // Fetch sensitive data from environment variables
        var username = "sandeepchahal433";//Environment.GetEnvironmentVariable("MONGODB_USERNAME");
        var password = "rPF0elBciQaRY7Ml";// Environment.GetEnvironmentVariable("MONGODB_PASSWORD");
        
        // Ensure the required parameters are not null
        if (string.IsNullOrEmpty(database) || string.IsNullOrEmpty(clusterName) ||
            string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            throw new InvalidOperationException("Configuration values for MongoDB connection are missing.");
        }

        // Encode the username and password
        var encodedUsername = HttpUtility.UrlEncode(username, Encoding.UTF8);
        var encodedPassword = HttpUtility.UrlEncode(password, Encoding.UTF8);
        
        //mongodb+srv://sandeepchahal433:<db_password>@nagp-ctt-online-shoppin.rgktl.mongodb.net/?retryWrites=true&w=majority&appName=NAGP-CTT-Online-Shopping
        var connectionString = $"mongodb+srv://{encodedUsername}:{encodedPassword}@nagp-ctt-online-shoppin.{mongoDbHost}/?retryWrites=true&w=majority&appName={encodedUsername}";
        return connectionString;
    }
}