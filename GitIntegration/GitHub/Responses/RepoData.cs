using Newtonsoft.Json;

namespace GitIntegration.GitHub.Responses
{
    public class RepoData
    {
        public string Name { get; set; }

        public string Description { get; set; }

        [JsonProperty("html_url")]
        public string HtmlUrl { get; set; }
    }
}
