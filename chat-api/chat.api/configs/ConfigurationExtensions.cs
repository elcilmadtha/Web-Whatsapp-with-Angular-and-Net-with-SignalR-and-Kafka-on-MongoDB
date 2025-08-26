using Chat.Api.Hubs;
using Chat.Api.Models.MongoDB;
using Chat.Data.Repositories.ChatMessageRepository;
using Chat.Data.Repositories.UserRepository;
using Chat.Shared.Kafka.Topic;
using Chat.Shared.Publisher;
using Microsoft.AspNetCore.SignalR;

namespace Chat.Api.configs
{
    public static class ConfigurationExtensions
    {
        public static void AddAppConfigurations(this WebApplicationBuilder builder)
        {
            // Bind MongoDbSettings
            builder.Services.Configure<MongoDbSettings>(
                builder.Configuration.GetSection("MongoDbSettings")
            );

            // Override with environment variable if exists
            var mongoConnection = Environment.GetEnvironmentVariable("CHAT_API_MONGO_CONNECTION");
            if (!string.IsNullOrEmpty(mongoConnection))
            {
                builder.Configuration["MongoDbSettings:ConnectionString"] = mongoConnection;
            }

            var jwtSecret = Environment.GetEnvironmentVariable("CHAT_API_JWT_SECRET");
            if (!string.IsNullOrEmpty(jwtSecret))
            {
                builder.Configuration["JwtSettings:SecretKey"] = jwtSecret;
            }

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAngularApp",
                    policy => { policy.WithOrigins("http://localhost:50211", "https://chat.taaindia.in").AllowAnyHeader().AllowAnyMethod().AllowCredentials(); });
            });
        }


        public static void AddAppServices(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(PublishMessagestoEntityCommand).Assembly));
            services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();
            services.AddSingleton<IChatMessageRepository, ChatMessageRepository>();
            services.AddSingleton<IUserRepository, UserRepository>();
            services.AddSingleton<IKafkaPublisher, KafkaPublisher>();
        }
    }
}
