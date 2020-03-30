namespace GitIntegration.OnDemand.GitHub.Responses
{
    public class CommitBody
    {
        public string Message { get; set; }
    }

    public class CommitData
    {
        public string Sha { get; set; }

        public CommitBody Commit { get; set; }
    }
}
