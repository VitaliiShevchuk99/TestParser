using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestParser.Converters;
using TestParser.Parsers;

namespace TestParser.Processors
{
    public class HtmlParsingProcessor
    {
        private readonly IHtmlParser _parser;
        private readonly IFormatConverter _formatConverter;
        private readonly IConfiguration _configuration;

        public HtmlParsingProcessor(IConfiguration configuration, IHtmlParser htmlParser, IFormatConverter formatConverter)
        {
            _parser = htmlParser;
            _formatConverter = formatConverter;
            _configuration = configuration;
        }

        public async Task ProcessParsing()
        {
            await _parser.Parse();

            var data =  _formatConverter.ConvertFromExcel(_configuration["ExcelFileName"]);
            var csvContent = _formatConverter.ConvertToCsv(data);

            if (string.IsNullOrEmpty(csvContent))
                return;

            WriteToCsv(_configuration["CsvFileName"], csvContent);
        }

        private void WriteToCsv(string filePath, string csvContent)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            File.WriteAllText(filePath, csvContent);
        }
    }
}
