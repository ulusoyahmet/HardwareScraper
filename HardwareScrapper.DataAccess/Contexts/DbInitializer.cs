using HardwareScrapper.Domain.Entities;

namespace HardwareScrapper.DataAccess.Contexts
{
    public static class DbInitializer
    {
        public static void Initialize(HardwareDbContext context)
        {
            // Ensure database is created
            context.Database.EnsureCreated();

            // Add seed data if database is empty
            if (!context.Categories.Any())
            {
                var categories = new List<Category>
                {
                    new Category { Name = "CPU", Description = "Central Processing Units" },
                    new Category { Name = "GPU", Description = "Graphics Processing Units" },
                    new Category { Name = "Motherboard", Description = "Computer Motherboards" },
                    new Category { Name = "RAM", Description = "Random Access Memory" },
                    new Category { Name = "Storage", Description = "Storage Devices including SSDs and HDDs" }
                };

                context.Categories.AddRange(categories);
                context.SaveChanges();
            }

            if (!context.Manufacturers.Any())
            {
                var manufacturers = new List<Manufacturer>
                {
                    new Manufacturer { Name = "Intel", WebsiteUrl = "https://www.intel.com", LogoUrl = "/images/manufacturers/intel.png" },
                    new Manufacturer { Name = "AMD", WebsiteUrl = "https://www.amd.com", LogoUrl = "/images/manufacturers/amd.png" },
                    new Manufacturer { Name = "NVIDIA", WebsiteUrl = "https://www.nvidia.com", LogoUrl = "/images/manufacturers/nvidia.png" },
                    new Manufacturer { Name = "ASUS", WebsiteUrl = "https://www.asus.com", LogoUrl = "/images/manufacturers/asus.png" },
                    new Manufacturer { Name = "MSI", WebsiteUrl = "https://www.msi.com", LogoUrl = "/images/manufacturers/msi.png" },
                    new Manufacturer { Name = "Gigabyte", WebsiteUrl = "https://www.gigabyte.com", LogoUrl = "/images/manufacturers/gigabyte.png" },
                    new Manufacturer { Name = "Corsair", WebsiteUrl = "https://www.corsair.com", LogoUrl = "/images/manufacturers/corsair.png" },
                    new Manufacturer { Name = "Kingston", WebsiteUrl = "https://www.kingston.com", LogoUrl = "/images/manufacturers/kingston.png" },
                    new Manufacturer { Name = "Western Digital", WebsiteUrl = "https://www.westerndigital.com", LogoUrl = "/images/manufacturers/wd.png" },
                    new Manufacturer { Name = "Samsung", WebsiteUrl = "https://www.samsung.com", LogoUrl = "/images/manufacturers/samsung.png" }
                };

                context.Manufacturers.AddRange(manufacturers);
                context.SaveChanges();
            }

            if (!context.ScrapingSources.Any())
            {
                var sources = new List<ScrapingSource>
                {
                    new ScrapingSource
                    {
                        Name = "Newegg",
                        BaseUrl = "https://www.newegg.com",
                        LogoUrl = "/images/sources/newegg.png",
                        IsEnabled = true,
                        ScrapeConfiguration = "{\"productListSelector\":\".product-item\",\"nameSelector\":\".product-title\",\"priceSelector\":\".price-current\"}"
                    },
                    new ScrapingSource
                    {
                        Name = "Amazon",
                        BaseUrl = "https://www.amazon.com",
                        LogoUrl = "/images/sources/amazon.png",
                        IsEnabled = true,
                        ScrapeConfiguration = "{\"productListSelector\":\".s-result-item\",\"nameSelector\":\"h2 a span\",\"priceSelector\":\".a-price .a-offscreen\"}"
                    },
                    new ScrapingSource
                    {
                        Name = "Microcenter",
                        BaseUrl = "https://www.microcenter.com",
                        LogoUrl = "/images/sources/microcenter.png",
                        IsEnabled = true,
                        ScrapeConfiguration = "{\"productListSelector\":\".product_wrapper\",\"nameSelector\":\".pDescription h2 a\",\"priceSelector\":\".price\"}"
                    }
                };

                context.ScrapingSources.AddRange(sources);
                context.SaveChanges();
            }
        }
    }
}
