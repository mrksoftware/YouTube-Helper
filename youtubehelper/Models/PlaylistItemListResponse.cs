using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace youtubehelper.Models
{
    public class PlaylistItemListResponse
    {
        public string kind { get; set; }
        public string etag { get; set; }
        public string nextPageToken { get; set; }
        public PageinfoPlaylist pageInfo { get; set; }
        public ItemPlaylist[] items { get; set; }
    }

    public class PageinfoPlaylist
    {
        public int totalResults { get; set; }
        public int resultsPerPage { get; set; }
    }

    public class ItemPlaylist
    {
        public string kind { get; set; }
        public string etag { get; set; }
        public string id { get; set; }
        public ContentdetailsPlaylist contentDetails { get; set; }
    }

    public class ContentdetailsPlaylist
    {
        public string videoId { get; set; }
        public DateTime videoPublishedAt { get; set; }
    }

}
