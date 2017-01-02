using GoogleAnalyticsTracker.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using youtubehelper.Helper;
using youtubehelper.Models;

namespace youtubehelper.Controllers
{
    [RoutePrefix("api/youtube")]
    public class YouTubeController : ApiController
    {
        /// <summary>
        /// Retrieves a list of videos from the specified channel
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="playlistId"></param>
        /// <param name="maxResults"></param>
        /// <param name="pageToken"></param>
        /// <returns></returns>
        [Route("channelvideos")]
        public async Task<ChannelVideoListResponse> GetChannelVideos(string apiKey, string playlistId, string maxResults, string pageToken = null)
        {
            Uri url;
            if (pageToken != null)
            {
                url = new Uri($"https://www.googleapis.com/youtube/v3/playlistItems?part=contentDetails&maxResults={maxResults}&playlistId={playlistId}&key={apiKey}&pageToken={pageToken}", UriKind.Absolute);
            }
            else
            {
                url = new Uri($"https://www.googleapis.com/youtube/v3/playlistItems?part=contentDetails&maxResults={maxResults}&playlistId={playlistId}&key={apiKey}", UriKind.Absolute);
            }

            var client = new HttpClient();
            var playlistItemListResponse = await client.GetJsonObject<PlaylistItemListResponse>(url);

            var ids = string.Join(",", playlistItemListResponse.items.Select(x => x.contentDetails.videoId));

            //get videos stats
            url = new Uri($"https://www.googleapis.com/youtube/v3/videos?part=snippet,contentDetails,statistics&id={ids}&maxResults={maxResults}&key={apiKey}");
            var videoListResponse = await client.GetJsonObject<VideoListResponse>(url);

            using (Tracker tracker = new Tracker("UA-55372977-14", "www.youtubehelper.apphb.com"))
            {
                var res = await tracker.TrackPageViewAsync(Request, "channelvideos");
            }

            return new ChannelVideoListResponse
            {
                response = videoListResponse.items.Select(x => new VideoInfo
                {
                    id = x.id,
                    title = x.snippet.title,
                    thumbnails = x.snippet.thumbnails.high.url,
                    duration = this.GetFormattedDuration(x.contentDetails.duration),
                    views = $"{x.statistics.viewCount} visualizzazioni",
                    link = $"http://www.youtube.com/watch?v={x.id}"
                }).ToList(),
                nextPageToken = playlistItemListResponse.nextPageToken
            };
        }

        #region Private Method
        public string GetFormattedDuration(string duration)
        {
            var reptms = new Regex(@"^PT(?:(\d+)H)?(?:(\d+)M)?(?:(\d+)S)?$");
            double hours = 0, minutes = 0, seconds = 0, totalseconds = 0;

            if (reptms.IsMatch(duration))
            {
                var matches = reptms.Match(duration).Groups;
                hours = string.IsNullOrWhiteSpace(matches[1].ToString()) ? 0 : Convert.ToDouble(matches[1].ToString());
                minutes = string.IsNullOrWhiteSpace(matches[2].ToString()) ? 0 : Convert.ToDouble(matches[2].ToString());
                seconds = string.IsNullOrWhiteSpace(matches[3].ToString()) ? 0 : Convert.ToDouble(matches[3].ToString());
                totalseconds = hours * 3600 + minutes * 60 + seconds;
            }

            if (hours == 0)
                return $"{minutes.ToString().PadLeft(2, '0')}:{seconds.ToString().PadLeft(2, '0')}";
            else
                return $"{hours}:{minutes.ToString().PadLeft(2, '0')}:{seconds.ToString().PadLeft(2, '0')}";
        }

        #endregion
    }
}