using System;
using System.Collections.Generic;
using System.Text;

namespace DownloadVideos.Entities
{
    public class User
    {
        public RegisteredDevice DeviceInfo { get; set; }

        public AuthenticationToken AuthToken { get; set; }

        public string UserHandle { get; set; }
    }
}
