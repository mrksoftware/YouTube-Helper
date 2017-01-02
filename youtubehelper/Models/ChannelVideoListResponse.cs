using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace youtubehelper.Models
{
    public class ChannelVideoListResponse
    {
        public IList<VideoInfo> response { get; set; }
        public string nextPageToken { get; set; }
        
    }
}