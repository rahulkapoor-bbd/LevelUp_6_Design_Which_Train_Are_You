using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using WhichTrainAreYouAPI.DataAccess;
using WhichTrainAreYouAPI.Middleware;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace WhichTrainAreYouAPI
{
    public class Startup
    {
        private readonly IConfiguration Configuration;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
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
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Add DbContext
            string connectionString = Configuration.GetConnectionString("DbConnection")!;
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Add controllers
            services.AddControllers();

            // Add authentication (if needed)
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = "your_issuer", // Replace with your issuer URL or identifier
                        ValidAudience = "your_audience", // Replace with your audience URL or identifier
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your_secret_key")) // Replace with your secret key
                    };
                });
        }
    }
}
