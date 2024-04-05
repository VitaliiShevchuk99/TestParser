using AngleSharp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OfficeOpenXml;
using TestParser.Converters;
using TestParser.Parsers;
using TestParser.Processors;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace TestParser
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var serviceProvider = new ServiceCollection()
                .AddSingleton<IHtmlParser, HtmlParser>()
                .AddSingleton<IFormatConverter, FormatConverter>()
                .AddSingleton<HtmlParsingProcessor>()
                .AddSingleton(config)
                .BuildServiceProvider();

            var processor = serviceProvider.GetService<HtmlParsingProcessor>();
            await processor.ProcessParsing();
        }
    }
}