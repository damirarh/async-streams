using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace AsyncStreams.GitHub
{
    public abstract class GitHubClientBase
    {
        protected readonly int maxPages = 5;
        protected readonly JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        
        protected readonly HttpClient httpClient;

        protected GitHubClientBase(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        protected string? GetNextPageUrl(HttpResponseMessage response)
        {
            try
            {
                var linkHeaderValue = response.Headers.GetValues("Link").FirstOrDefault();
                var match = Regex.Match(linkHeaderValue ?? string.Empty, @"<(?<url>[^;]*)>; rel=""next""");
                return match?.Groups?["url"]?.Value;
            }
            catch
            {
                return null;
            }
        }
    }
}
