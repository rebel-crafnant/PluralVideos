using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DownloadVideos.Entities
{
    public class Course
    {
        [JsonProperty(PropertyName = "header")]
        public Header Header { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "modules")]
        public virtual ICollection<Module> Modules { get; set; }

        public Course()
        {
            Modules = new Collection<Module>();
        }
    }
}
