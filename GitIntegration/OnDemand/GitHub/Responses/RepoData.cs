﻿using Newtonsoft.Json;

namespace GitIntegration.OnDemand.GitHub.Responses
{
    internal class RepoData
    {
        public string Name { get; set; }

        public string Description { get; set; }

        [JsonProperty("html_url")]
        public string HtmlUrl { get; set; }
    }
}
