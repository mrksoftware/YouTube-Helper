using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace youtubehelper.Helper
{
    public static class HttpHelper
    {
        public async static Task<T> GetJsonObject<T>(this HttpClient client, Uri url)
        {
            var result = await client.GetAsync(url);
            result.EnsureSuccessStatusCode();
            string content = await result.Content.ReadAsStringAsync();
            var objs = JsonConvert.DeserializeObject<T>(content);
            return objs;
        }
    }
}