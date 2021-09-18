using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reactive.Linq;
using System.Text.Json;

namespace AsyncStreams.GitHub
{
    public class ObservableGitHubClient : GitHubClientBase
    {
        public ObservableGitHubClient(HttpClient httpClient)
            : base(httpClient)
        {
        }

        public IObservable<GitHubIssue> GetIssues(string owner, string repo)
        {
            return Observable.Create<GitHubIssue>(async (observer, cancellationToken) =>
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
                        observer.OnNext(issue);
                    }

                    url = GetNextPageUrl(response);
                    page++;
                }
            });
        }
    }
}
