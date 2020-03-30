namespace GitIntegration.OnDemand.GitHub.Responses
{
    internal class CommitBody
    {
        public string Message { get; set; }
    }

    internal class CommitData
    {
        public string Sha { get; set; }

        public CommitBody Commit { get; set; }
    }
}
