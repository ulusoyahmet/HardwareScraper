using HardwareScrapper.Business.Abstractions;
using HardwareScrapper.Business.Services;
using HardwareScrapper.DataAccess;
using HardwareScrapper.DataAccess.Abstractions;
using HardwareScrapper.DataAccess.Contexts;
using HardwareScrapper.DataAccess.Repositories;
using HardwareScrapper.Services.Abstractions;
using HardwareScrapper.Services.Services;
using HardwareScrapper.UI.Forms;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Windows.Forms;

namespace HardwareScrapper
{
    internal static class Program
    {
        public static IServiceProvider ServiceProvider { get; private set; }
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var services = new ServiceCollection();
            ConfigureServices(services);
            ServiceProvider = services.BuildServiceProvider();

            Application.Run(ServiceProvider.GetRequiredService<MainForm>());
        }

        private static void ConfigureServices(ServiceCollection services)
        {
            // Configuration
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            // Register DB context
            services.AddDbContext<HardwareDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // Register repositories
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Register services
            services.AddScoped<IHardwareService, HardwareService>();
            services.AddScoped<IManufacturerService, ManufacturerService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IScrapingService, ScrapingService>();

            // Register forms
            services.AddScoped<MainForm>();
            services.AddTransient<DashboardForm>();
            services.AddTransient<HardwareListForm>();
            services.AddTransient<ManufacturerForm>();
            services.AddTransient<CategoryForm>();
            services.AddTransient<ScrapingForm>();
            services.AddTransient<LogViewerForm>();
        }
    }
}