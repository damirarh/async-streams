namespace AsyncStreams.GitHub
{
    public class GitHubIssue
    {
        public int Number { get; set; }
        public string? Title { get; set; }

        public override string ToString() => $"{Number}: {Title}";
    }
}
