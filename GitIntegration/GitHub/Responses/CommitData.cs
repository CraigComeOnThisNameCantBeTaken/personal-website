using System;
using System.Collections.Generic;
using System.Text;

namespace GitIntegration.GitHub.Responses
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
