using CodeChallenge.Data;

using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CodeChallenge.Config
{
    public static class WebApplicationBuilderExt
    {
        private static readonly string DB_NAME = "EmployeeDB";
        private static readonly string COMP_DB_NAME = "CompensationDB";
        public static void UseEmployeeDB(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<EmployeeContext>(options =>
            {
                options.UseInMemoryDatabase(DB_NAME);
            });
        }

        /// <summary>
        /// Method to tell the app to use the compensation database
        /// </summary>
        /// <param name="builder">the base WebApplicationBuilder class</param>
        public static void UseCompensationDB(this WebApplicationBuilder builder)
        {
            //passing the builder a new context using the in-memory database with the provided name
            builder.Services.AddDbContext<CompensationContext>(options =>
            {
                options.UseInMemoryDatabase(DB_NAME);
            });
        }
    }
}
