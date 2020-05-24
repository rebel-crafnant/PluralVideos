using DownloadVideos.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DownloadVideos.Responses
{
    public class ClipResponse
    {
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }

        [JsonProperty(PropertyName = "rankedOptions")]
        public List<UrlOptionResponse> RankedOptions { get; set; }
    }

    public class UrlOptionResponse
    {
        [JsonProperty(PropertyName = "cdn")]
        public string Cdn { get; set; }

        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }
    }
}
