using AngleSharp;
using System.IO.Compression;
using System.Text;
using TestParser.Extentions;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace TestParser.Parsers
{
    public class HtmlParser: IHtmlParser
    {
        private readonly IConfiguration _configuration;

        public HtmlParser(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task Parse()
        {
            var htmlContent = await GetWebPageAsync(_configuration["BaseUrl"]+ _configuration["Url"]);

            var config = Configuration.Default.WithDefaultLoader();
            var context = BrowsingContext.New(config);
            var document = await context.OpenAsync(req => req.Content(htmlContent));

            var anchorElements = document.QuerySelectorAll("a");
            var href= anchorElements.Where(anchor => anchor.GetAttribute("title") == _configuration["WebElementToDownload"])
                .FirstOrDefault()?
                .GetAttribute("href");

            if(href != null)
            {
                await GetWebFileAsync(_configuration["BaseUrl"] + href, _configuration["ExcelFileName"]);
            }
        }

        public async Task<string> GetWebPageAsync(string url)
        {
            using HttpClient httpClient = new HttpClient();

            AddHttpClientHeaders(httpClient);

            var response = await httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var byteArray = await response.Content.ReadAsByteArrayAsync();
                using (var memoryStream = new MemoryStream(byteArray))
                {

                    using (var outputStream = new MemoryStream())
                    {
                        using (var decompressStream = new GZipStream(memoryStream, CompressionMode.Decompress))
                        {
                            decompressStream.CopyTo(outputStream);
                        }

                        var result = Encoding.UTF8.GetString(outputStream.ToArray());
                        return result;
                    }
                }
            }

            return string.Empty;
        }

        public async Task GetWebFileAsync(string url, string fileName)
        {
            using HttpClient httpClient = new HttpClient();

            AddHttpClientHeaders(httpClient);

            DeleteIfExist(fileName);

            await httpClient.DownloadFileTaskAsync(url, fileName);
        }

        private void DeleteIfExist(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        private void AddHttpClientHeaders(HttpClient httpClient)
        {
            httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/123.0.0.0 Safari/537.36");
            httpClient.DefaultRequestHeaders.Add("Accept", "application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7");
            httpClient.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br, zstd");
            httpClient.DefaultRequestHeaders.Add("Connection", "keep-alive");
        }
    }
}
