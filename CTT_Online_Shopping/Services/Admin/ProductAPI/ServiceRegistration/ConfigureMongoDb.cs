using System.Text;
using System.Web;
using MongoDB.Bson;
using MongoDB.Driver;
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

            var mongoClient = new MongoClient(settings);

            // Define the constant database name
            var databaseName = configuration["MongoDB:Database"];
            if (string.IsNullOrEmpty(databaseName))
            {
                throw new InvalidOperationException("Database name doesn't exist in the configuration");
            }

            // Create the IMongoDatabase instance using the database name and MongoClient
            var database = mongoClient.GetDatabase(databaseName);

             mongoClient.GetDatabase(databaseName).RunCommand<BsonDocument>(new BsonDocument("ping", 1));

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
    }
    private static string GetConnectionString(IConfiguration configuration)
    {
        // Fetch database and cluster name from appsettings.json
        var database = configuration["MongoDB:Database"];
        var clusterName = configuration["MongoDB:ClusterName"];
        var mongoDbHost = configuration["MongoDB:Host"];
        var mongoDbPort = configuration["MongoDB:Port"];
        
        // Fetch sensitive data from environment variables
        var username = "admin";//Environment.GetEnvironmentVariable("MONGODB_USERNAME");
        var password = "admin123";// Environment.GetEnvironmentVariable("MONGODB_PASSWORD");
        
        // Ensure the required parameters are not null
        if (string.IsNullOrEmpty(database) || string.IsNullOrEmpty(clusterName) ||
            string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            throw new InvalidOperationException("Configuration values for MongoDB connection are missing.");
        }

        // Encode the username and password
        var encodedUsername = HttpUtility.UrlEncode(username, Encoding.UTF8);
        var encodedPassword = HttpUtility.UrlEncode(password, Encoding.UTF8);

        var connectionString = $"mongodb://{encodedUsername}:{encodedPassword}@{mongoDbHost}:{mongoDbPort}/{database}?authSource=admin";
        return connectionString;
    }
}