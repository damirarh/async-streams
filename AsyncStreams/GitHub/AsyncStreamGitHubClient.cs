using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading;

namespace AsyncStreams.GitHub
{
    public class AsyncStreamGitHubClient : GitHubClientBase
    {
        public AsyncStreamGitHubClient(HttpClient httpClient)
            : base(httpClient)
        {
        }

        public async IAsyncEnumerable<GitHubIssue> GetIssues(string owner, string repo, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var url = $"https://api.github.com/repos/{owner}/{repo}/issues";
            var page = 1;

            while (page <= maxPages && url != null)
            {
                Console.WriteLine($"Retrieving URL: {url}");
                var response = await httpClient.GetAsync(url, cancellationToken);
                var json = await response.Content.ReadAsStringAsync();
                var issues = JsonSerializer.Deserialize<List<GitHubIssue>>(json, jsonSerializerOptions) ?? new();

                foreach (var issue in issues)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    yield return issue;
                }

                url = GetNextPageUrl(response);
                page++;
            }
        }
    }
}
