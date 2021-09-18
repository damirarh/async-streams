using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using AsyncStreams.GitHub;
using System.Threading;

namespace AsyncStreams.Tests
{
    public class GitHubClientTests
    {
        private HttpClient GetHttpClient()
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.UserAgent.TryParseAdd("request");

            // TODO: replace with your username and personal access token
            // https://docs.github.com/en/rest/guides/getting-started-with-the-rest-api#using-personal-access-tokens
            var authString = "username:token";
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes(authString)));
            return httpClient;
        }

        [Test]
        public async Task AsyncStreamClient()
        {
            var client = new AsyncStreamGitHubClient(GetHttpClient());

            var cancellationTokenSource = new CancellationTokenSource();
            await foreach (var issue in client.GetIssues("dotnet", "runtime")
                .WithCancellation(cancellationTokenSource.Token))
            {
                Console.WriteLine(issue.ToString());
            }
        }

        [Test]
        public Task ObservableClient()
        {
            var taskCompletionSource = new TaskCompletionSource<bool>();
            
            var client = new ObservableGitHubClient(GetHttpClient());

            var observable = client.GetIssues("dotnet", "runtime");
            observable.Subscribe(
                issue =>
                {
                    Console.WriteLine(issue.ToString());
                },
                () => taskCompletionSource.SetResult(true));

            return taskCompletionSource.Task;
        }
    }
}
