using System.Text.RegularExpressions;
using HardwareScrapper.Domain.Entities;
using HtmlAgilityPack;

namespace HardwareScrapper.Services.Extensions
{
    public static class ScrapingExtensions
    {
        /// <summary>
        /// Extract specifications from a product page and add them to a component
        /// </summary>
        public static List<Specification> ExtractSpecifications(this HtmlDocument document, string specificationSelector)
        {
            var specs = new List<Specification>();

            if (string.IsNullOrWhiteSpace(specificationSelector))
                return specs;

            var specNodes = document.DocumentNode.SelectNodes(specificationSelector);
            if (specNodes == null)
                return specs;

            foreach (var specNode in specNodes)
            {
                try
                {
                    // This implementation depends on the specific HTML structure of spec tables
                    // Common formats are key-value pairs in tables or definition lists
                    string name = specNode.SelectSingleNode(".//th") != null ?
                        specNode.SelectSingleNode(".//th").InnerText.Trim() :
                        specNode.SelectSingleNode(".//dt")?.InnerText.Trim();

                    string value = specNode.SelectSingleNode(".//td") != null ?
                        specNode.SelectSingleNode(".//td").InnerText.Trim() :
                        specNode.SelectSingleNode(".//dd")?.InnerText.Trim();

                    if (!string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(value))
                    {
                        specs.Add(new Specification
                        {
                            Name = name,
                            Value = value,
                            CreatedDate = DateTime.UtcNow,
                            IsActive = true
                        });
                    }
                }
                catch
                {
                    // Skip this specification if we can't parse it
                    continue;
                }
            }

            return specs;
        }

        /// <summary>
        /// Extract CPU-specific details from product information
        /// </summary>
        public static void ExtractCPUDetails(this CPU cpu, string description)
        {
            // Extract CPU details using regex patterns
            cpu.CoreCount = ExtractIntegerValue(description, @"(\d+)\s*cores?", 0);
            cpu.ThreadCount = ExtractIntegerValue(description, @"(\d+)\s*threads?", cpu.CoreCount);
            cpu.BaseClock = ExtractDecimalValue(description, @"(\d+(\.\d+)?)\s*GHz", 0);
            cpu.BoostClock = ExtractDecimalValue(description, @"(?:boost|turbo|max).*?(\d+(\.\d+)?)\s*GHz", cpu.BaseClock);

            // Extract socket information
            var socketMatch = Regex.Match(description, @"Socket\s*([A-Z0-9\-]+)", RegexOptions.IgnoreCase);
            if (socketMatch.Success)
                cpu.Socket = socketMatch.Groups[1].Value;

            // Extract TDP
            cpu.TDP = ExtractIntegerValue(description, @"(\d+)\s*W\s*TDP", 0);

            // Extract architecture
            var archMatch = Regex.Match(description, @"(?:Architecture|Micro[-\s]?architecture):\s*([A-Za-z0-9\s]+)", RegexOptions.IgnoreCase);
            if (archMatch.Success)
                cpu.Architecture = archMatch.Groups[1].Value.Trim();

            // Extract cache size
            cpu.CacheSize = ExtractIntegerValue(description, @"(\d+)\s*MB\s*(?:L3|Cache)", 0);
        }

        /// <summary>
        /// Extract GPU-specific details from product information
        /// </summary>
        public static void ExtractGPUDetails(this GPU gpu, string description)
        {
            // Extract memory size
            gpu.MemorySize = ExtractIntegerValue(description, @"(\d+)\s*GB", 0);

            // Extract memory type
            var memMatch = Regex.Match(description, @"(GDDR\d+X?)", RegexOptions.IgnoreCase);
            if (memMatch.Success)
                gpu.MemoryType = memMatch.Groups[1].Value;

            // Extract clock speeds
            gpu.CoreClock = ExtractDecimalValue(description, @"(?:core|base).*?(\d+(\.\d+)?)\s*(?:MHz|GHz)", 0);
            gpu.BoostClock = ExtractDecimalValue(description, @"(?:boost|game).*?(\d+(\.\d+)?)\s*(?:MHz|GHz)", gpu.CoreClock);

            // Convert MHz to GHz if needed
            if (gpu.CoreClock > 100) gpu.CoreClock /= 1000;
            if (gpu.BoostClock > 100) gpu.BoostClock /= 1000;

            // Extract stream processors/CUDA cores
            gpu.StreamProcessors = ExtractIntegerValue(description, @"(\d+)\s*(?:Stream Processors|CUDA Cores)", 0);

            // Extract interface
            var ifMatch = Regex.Match(description, @"PCI(?:e|Express)\s*(\d+(?:\.\d+)?)\s*x(\d+)", RegexOptions.IgnoreCase);
            if (ifMatch.Success)
                gpu.Interface = $"PCIe {ifMatch.Groups[1].Value} x{ifMatch.Groups[2].Value}";

            // Extract TDP
            gpu.TDP = ExtractIntegerValue(description, @"(\d+)\s*W", 0);

            // Extract architecture
            var archMatch = Regex.Match(description, @"(?:Architecture|Arch):\s*([A-Za-z0-9\s]+)", RegexOptions.IgnoreCase);
            if (archMatch.Success)
                gpu.Architecture = archMatch.Groups[1].Value.Trim();

            // Extract ports
            var portsMatch = Regex.Match(description, @"(?:Ports?|Outputs?|Displays?):\s*([A-Za-z0-9\s,]+)", RegexOptions.IgnoreCase);
            if (portsMatch.Success)
                gpu.Ports = portsMatch.Groups[1].Value.Trim();
        }

        /// <summary>
        /// Helper method to extract integer values using regex
        /// </summary>
        private static int ExtractIntegerValue(string text, string pattern, int defaultValue)
        {
            if (string.IsNullOrWhiteSpace(text))
                return defaultValue;

            var match = Regex.Match(text, pattern, RegexOptions.IgnoreCase);
            if (match.Success && int.TryParse(match.Groups[1].Value, out int value))
                return value;

            return defaultValue;
        }

        /// <summary>
        /// Helper method to extract decimal values using regex
        /// </summary>
        private static decimal ExtractDecimalValue(string text, string pattern, decimal defaultValue)
        {
            if (string.IsNullOrWhiteSpace(text))
                return defaultValue;

            var match = Regex.Match(text, pattern, RegexOptions.IgnoreCase);
            if (match.Success && decimal.TryParse(match.Groups[1].Value, out decimal value))
                return value;

            return defaultValue;
        }
    }
}
