using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using System.Data.Common;

namespace Mgutz.DapperPg.Dal {

    public class Startup {
        /// <summary>Configure registers DI services.</summary>
        public static void Configure(IConfiguration config, IServiceCollection services) {
            var connectionString = config.GetConnectionString("DefaultConnection");
            services.AddTransient<DbConnection>(_ => new NpgsqlConnection(connectionString));
            services.AddScoped<IProductRepository, ProductRepository>();
        }
    }

}
