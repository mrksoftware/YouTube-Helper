using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace youtubehelper.Models
{
    public class VideoInfo
    {
        public string title { get; set; }
        public string id { get; set; }
        public string link { get; set; }
        public string thumbnails { get; set; }
        public string views { get; set; }
        public string duration { get; set; }
    }
}