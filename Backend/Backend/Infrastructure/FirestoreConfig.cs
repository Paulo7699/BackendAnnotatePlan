using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.Infrastructure;

public static class FirestoreConfig
{
    public static IServiceCollection AddFirestoreDb(this IServiceCollection services, IConfiguration configuration)
    {
        var credentialsPath = Path.Combine(Directory.GetCurrentDirectory(), "Infrastructure", "keys", "service-account.json");
        var builder = new FirestoreDbBuilder
        {
            ProjectId = configuration["Firestore:ProjectId"],
            JsonCredentials = File.ReadAllText(credentialsPath)
        };

        
        var db = builder.Build();
        services.AddSingleton(db);
        return services;
    }
    
    public static IServiceCollection AddFirebaseAuth(this IServiceCollection services, IConfiguration configuration)
    {
        var credentialsPath = Path.Combine(Directory.GetCurrentDirectory(), "Infrastructure", "keys", "service-account.json");
        
        var app = FirebaseApp.Create(new AppOptions
        {
            ProjectId = configuration["Firebase:ProjectId"],
            Credential = GoogleCredential.FromJson(File.ReadAllText(credentialsPath))
        });
        
        services.AddSingleton(app);
        return services;
    }
}