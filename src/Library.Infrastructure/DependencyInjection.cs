using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Library.Domain.Ports.Out;
using Library.Infrastructure.Persistence;
using Library.Infrastructure.Persistence.Context;
using Library.Infrastructure.Persistence.Repositories;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace Library.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            var host = Environment.GetEnvironmentVariable("BD_HOST");
            var port = Environment.GetEnvironmentVariable("DB_PORT");
            var database = Environment.GetEnvironmentVariable("DB_NAME");
            var user = Environment.GetEnvironmentVariable("DB_USER");
            var password = Environment.GetEnvironmentVariable("DB_PASSWORD");

            var connectionString = $"Server={host};Port={port};Database={database};User={user};Password={password};";
            Console.WriteLine($"Connection String: {connectionString}");

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 0))));

            // Register DbContext as the non-generic service so repositories requesting DbContext are resolved
            services.AddScoped<Microsoft.EntityFrameworkCore.DbContext>(sp => sp.GetRequiredService<ApplicationDbContext>());

            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<ILoanRepository, LoanRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
