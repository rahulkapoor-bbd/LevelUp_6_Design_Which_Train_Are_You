using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using WhichTrainAreYouAPI.DataAccess;
using WhichTrainAreYouAPI.Middleware;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using WhichTrainAreYouAPI.Utils;
using System.IdentityModel.Tokens.Jwt;

namespace WhichTrainAreYouAPI
{
    public class Startup
    {
        private readonly IConfiguration Configuration;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var appSettings = Configuration.GetSection("AppSettings");
            var jwtSecretKey = Environment.GetEnvironmentVariable("WhichTrainAreYouJWTKey");
            var issuer = appSettings["Issuer"];
            var audience = appSettings["Audience"];

            // Add DbContext
            var connectionString = Environment.GetEnvironmentVariable("WhichTrainAreYouDBConnectionString");
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Add controllers
            services.AddControllers();
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddSingleton<JWTHelper>();

            services.AddHealthChecks();


            // Add authentication
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = issuer,
                        ValidAudience = audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretKey == null ? " " : jwtSecretKey))
                    };
                });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Whichtrainareyou API", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Enable routing
            app.UseRouting();

            // Enable authentication
            app.UseAuthentication();
            app.UseAuthorization();

            // Add a custom middleware to handle errors
            app.UseMiddleware<ErrorHandlingMiddleware>();

            // Define the API endpoints
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseHealthChecks("/health");
        }
    }
}
