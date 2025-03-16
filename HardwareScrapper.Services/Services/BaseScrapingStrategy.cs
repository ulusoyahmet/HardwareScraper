using System.Text.RegularExpressions;
using HardwareScrapper.Domain.Entities;
using HardwareScrapper.Services.Abstractions;
using HardwareScrapper.Services.Configs;
using HtmlAgilityPack;

namespace HardwareScrapper.Services.Services
{
    public class BaseScrapingStrategy : IScrapingStrategy
    {
        public virtual List<HardwareComponent> Extract(HtmlDocument document, string url, ScrapingConfig config,
            Dictionary<string, int> categoryMap, Dictionary<string, int> manufacturerMap)
        {
            var components = new List<HardwareComponent>();

            // Select all product elements from the page
            var productNodes = document.DocumentNode.SelectNodes(config.ProductListSelector);

            if (productNodes == null)
                return components;

            foreach (var productNode in productNodes)
            {
                try
                {
                    // Extract basic info using the provided selectors
                    string name = ExtractTextFromNode(productNode, config.NameSelector);
                    string priceText = ExtractTextFromNode(productNode, config.PriceSelector);
                    string model = ExtractTextFromNode(productNode, config.ModelSelector);
                    string imageUrl = ExtractAttributeFromNode(productNode, config.ImageSelector, "src");
                    string description = ExtractTextFromNode(productNode, config.DescriptionSelector);
                    string manufacturerName = ExtractTextFromNode(productNode, config.ManufacturerSelector);

                    // Skip if essential info is missing
                    if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(priceText))
                        continue;

                    // Process and clean extracted data
                    decimal price = ExtractPrice(priceText);
                    bool inStock = DetermineInStock(productNode, config.InStockSelector);

                    // Determine component type and manufacturer
                    var componentInfo = DetermineComponentType(name, description);
                    string category = componentInfo.Item1;
                    Type componentType = componentInfo.Item2;

                    // Skip if we couldn't determine the category
                    if (string.IsNullOrWhiteSpace(category) || !categoryMap.ContainsKey(category))
                        continue;

                    // Set default manufacturer if not found
                    if (string.IsNullOrWhiteSpace(manufacturerName) || !manufacturerMap.ContainsKey(manufacturerName))
                    {
                        manufacturerName = DetermineManufacturer(name, description);
                        if (string.IsNullOrWhiteSpace(manufacturerName) || !manufacturerMap.ContainsKey(manufacturerName))
                            manufacturerName = "Unknown";
                    }

                    // Create the appropriate component type
                    HardwareComponent component = CreateComponent(componentType, name, model, price, url,
                        imageUrl, description, inStock, categoryMap[category],
                        manufacturerMap.ContainsKey(manufacturerName) ? manufacturerMap[manufacturerName] : manufacturerMap["Unknown"]);

                    if (component != null)
                        components.Add(component);
                }
                catch (Exception ex)
                {
                    // Log exception but continue processing other products
                    Console.WriteLine($"Error processing product: {ex.Message}");
                }
            }

            return components;
        }

        protected string ExtractTextFromNode(HtmlNode parentNode, string selector)
        {
            if (string.IsNullOrWhiteSpace(selector))
                return string.Empty;

            var node = parentNode.SelectSingleNode(selector);
            return node != null ? node.InnerText.Trim() : string.Empty;
        }

        protected string ExtractAttributeFromNode(HtmlNode parentNode, string selector, string attributeName)
        {
            if (string.IsNullOrWhiteSpace(selector) || string.IsNullOrWhiteSpace(attributeName))
                return string.Empty;

            var node = parentNode.SelectSingleNode(selector);
            return node != null ? node.GetAttributeValue(attributeName, string.Empty) : string.Empty;
        }

        protected decimal ExtractPrice(string priceText)
        {
            if (string.IsNullOrWhiteSpace(priceText))
                return 0;

            // Remove currency symbols, commas, and other non-numeric characters except decimal point
            string numericString = Regex.Replace(priceText, @"[^\d\.]", "");

            if (decimal.TryParse(numericString, out decimal price))
                return price;

            return 0;
        }

        protected bool DetermineInStock(HtmlNode productNode, string inStockSelector)
        {
            if (string.IsNullOrWhiteSpace(inStockSelector))
                return true; // Default to in-stock if no selector provided

            var stockNode = productNode.SelectSingleNode(inStockSelector);
            if (stockNode == null)
                return true;

            string stockText = stockNode.InnerText.ToLower();
            return !stockText.Contains("out of stock") && !stockText.Contains("unavailable");
        }

        protected Tuple<string, Type> DetermineComponentType(string name, string description)
        {
            string nameLower = name.ToLower();
            string descLower = description?.ToLower() ?? "";

            // CPU detection
            if (nameLower.Contains("processor") || nameLower.Contains("cpu") ||
                nameLower.Contains("ryzen") || nameLower.Contains("core i") ||
                descLower.Contains("socket") && descLower.Contains("ghz"))
                return new Tuple<string, Type>("CPU", typeof(CPU));

            // GPU detection
            if (nameLower.Contains("graphics card") || nameLower.Contains("gpu") ||
                nameLower.Contains("geforce") || nameLower.Contains("radeon") ||
                nameLower.Contains("rtx") || nameLower.Contains("gtx"))
                return new Tuple<string, Type>("GPU", typeof(GPU));

            // Motherboard detection
            if (nameLower.Contains("motherboard") || nameLower.Contains("mainboard") ||
                (descLower.Contains("socket") && descLower.Contains("form factor")))
                return new Tuple<string, Type>("Motherboard", typeof(Motherboard));

            // RAM detection
            if (nameLower.Contains("memory") || nameLower.Contains("ram") ||
                nameLower.Contains("ddr") || (descLower.Contains("mhz") && descLower.Contains("cl")))
                return new Tuple<string, Type>("RAM", typeof(RAM));

            // Storage detection
            if (nameLower.Contains("ssd") || nameLower.Contains("hdd") ||
                nameLower.Contains("solid state") || nameLower.Contains("hard drive") ||
                nameLower.Contains("nvme"))
                return new Tuple<string, Type>("Storage", typeof(Storage));

            // Default to base component type if we can't determine
            return new Tuple<string, Type>("Unknown", typeof(HardwareComponent));
        }

        protected string DetermineManufacturer(string name, string description)
        {
            string[] commonManufacturers = new[] {
                "Intel", "AMD", "NVIDIA", "ASUS", "MSI", "Gigabyte", "Corsair",
                "Kingston", "Western Digital", "Samsung", "Seagate", "EVGA",
                "G.Skill", "Crucial", "ASRock", "PNY", "Zotac", "Sapphire"
            };

            foreach (var manufacturer in commonManufacturers)
            {
                if (name.IndexOf(manufacturer, StringComparison.OrdinalIgnoreCase) >= 0)
                    return manufacturer;

                if (!string.IsNullOrWhiteSpace(description) &&
                    description.IndexOf(manufacturer, StringComparison.OrdinalIgnoreCase) >= 0)
                    return manufacturer;
            }

            return "Unknown";
        }

        protected HardwareComponent CreateComponent(Type componentType, string name, string model,
            decimal price, string sourceUrl, string imageUrl, string description, bool inStock,
            int categoryId, int manufacturerId)
        {
            // Base component properties
            HardwareComponent component;

            // Create the specific component type
            if (componentType == typeof(CPU))
                component = new CPU();
            else if (componentType == typeof(GPU))
                component = new GPU();
            else if (componentType == typeof(Motherboard))
                component = new Motherboard();
            else if (componentType == typeof(RAM))
                component = new RAM();
            else if (componentType == typeof(Storage))
                component = new Storage();
            else
                component = new HardwareComponent();

            // Set common properties
            component.Name = name;
            component.Model = string.IsNullOrWhiteSpace(model) ? ExtractModelFromName(name) : model;
            component.Price = price;
            component.SourceUrl = sourceUrl;
            component.ImageUrl = imageUrl;
            component.Description = description;
            component.InStock = inStock;
            component.CategoryId = categoryId;
            component.ManufacturerId = manufacturerId;
            component.ScrapedDate = DateTime.UtcNow;
            component.IsActive = true;
            component.CreatedDate = DateTime.UtcNow;

            return component;
        }

        protected string ExtractModelFromName(string name)
        {
            // Try to extract model number using common patterns
            var modelPatterns = new[] {
                @"(?i)[A-Z0-9]+-[A-Z0-9]+",  // Format like "i7-9700K" or "RTX-3080"
                @"(?i)[A-Z]+\s?[0-9]{4}",    // Format like "RTX 3080" or "RX6800"
                @"(?i)[A-Z]{2,}[0-9]{3,}"    // Format like "GTX1080" or "RX580"
            };

            foreach (var pattern in modelPatterns)
            {
                var match = Regex.Match(name, pattern);
                if (match.Success)
                    return match.Value;
            }

            return name.Length > 30 ? name.Substring(0, 30) : name;
        }
    }
}
