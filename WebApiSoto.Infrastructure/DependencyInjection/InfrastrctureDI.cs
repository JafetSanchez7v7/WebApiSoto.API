using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WebApiSoto.Application.Interfaces;
using WebApiSoto.Application.Providers;
using WebApiSoto.Infrastructure.Context;
using WebApiSoto.Infrastructure.DbTrigger;
using WebApiSoto.Infrastructure.Repositories;

namespace WebApiSoto.Infrastructure.DependencyInjection
{
    public static class InfrastrctureDI
    {
       
        public static IServiceCollection AddInfrastructure (this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMemoryCache();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ISupplierRepository, SupplierRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICustomersRepository, CustomerRepository>();
            services.AddScoped<ITokenProvider, TokenProvider>();
            services.AddScoped<IDbInicializador, DbInicializador>();
            services.AddScoped<IOptionRepository, OptionRepository>();
            services.AddScoped<IInventoryRepository, InventoryRepository>();
            services.AddScoped<IPurchasesRepository, PurchaseRespository>();
            services.AddScoped<ISalesRepository, SaleRepository>();
            services.AddSingleton<IInvoiceRepository, InvoiceRepository>();
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),b=> b.MigrationsAssembly("WebApiSoto.Infrastructure"));
            });
            

            return services;
        }
    }
}
