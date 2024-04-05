using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestParser.Extentions
{
    public static class HttpClientExtentionUtils
    {
            public static async Task DownloadFileTaskAsync(this HttpClient client, string uri, string FileName)
            {
                using (var s = await client.GetStreamAsync(uri))
                {
                    using (var fs = new FileStream(FileName, FileMode.CreateNew))
                    {
                        await s.CopyToAsync(fs);
                    }
                }
            }
    }
}
