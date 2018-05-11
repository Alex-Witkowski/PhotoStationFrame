using Newtonsoft.Json;
using PhotoStationFrame.Api.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PhotoStationFrame.Api
{
    public class PhotoStationClient
    {
        private HttpClient httpClient;
        private string url;
        private string username;
        private string password;
        private string sessionId;

        public PhotoStationClient()
        {
            httpClient = new HttpClient();
        }

        public void Initialize(IApiSettings apiSettings)
        {
            this.url = apiSettings.Url;
            this.username = apiSettings.Username;
            this.password = apiSettings.Password;
        }

        public async Task<ListAlbumsResponse> ListAlbumsAsync()
        {
            if (this.sessionId == null)
            {
                throw new Exception("Please login first.");
            }


            var content = new FormUrlEncodedContent(new[]
              {
                    new KeyValuePair<string, string>("api", "SYNO.PhotoStation.Album"),
                    new KeyValuePair<string, string>("limit", "-1"),
                    new KeyValuePair<string, string>("method", "list"),
                    new KeyValuePair<string, string>("offset", "0"),
                    new KeyValuePair<string, string>("recursive", "true"),
                    new KeyValuePair<string, string>("sort_by", "preference"),
                    new KeyValuePair<string, string>("sort_direction", "asc"),
                    new KeyValuePair<string, string>("type", "album"),
                    new KeyValuePair<string, string>("version", "1")
                });

            var response = await httpClient.PostAsync($"photo/webapi/album.php?SynoToken={sessionId}", content).ConfigureAwait(false);
            var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var listAlbumsResponse = JsonConvert.DeserializeObject<ListAlbumsResponse>(responseString);
            return listAlbumsResponse;
        }

        public async Task<SmartAlbumsResponse> ListSmartAlbumsAsync()
        {
            if (this.sessionId == null)
            {
                throw new Exception("Please login first.");
            }

            var content = new FormUrlEncodedContent(new[]
              {
                    //new KeyValuePair<string, string>("additional", "thumb_size"),
                    new KeyValuePair<string, string>("api", "SYNO.PhotoStation.SmartAlbum"),
                    new KeyValuePair<string, string>("limit", "50"),
                    new KeyValuePair<string, string>("method", "list"),
                    new KeyValuePair<string, string>("offset", "0"),
                    new KeyValuePair<string, string>("sort_by", "title"),
                    new KeyValuePair<string, string>("sort_direction", "asc"),
                    new KeyValuePair<string, string>("version", "1")
                });

            var response = await httpClient.PostAsync($"photo/webapi/smart_album.php?SynoToken={sessionId}", content).ConfigureAwait(false);
            var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var listAlbumsResponse = JsonConvert.DeserializeObject<SmartAlbumsResponse>(responseString);
            return listAlbumsResponse;
        }

        public async Task<ListItemResponse> ListSmartAlbumItemsAsync(string smartAlbumId, int offset = 0, int limit = 100)
        {
            if (this.sessionId == null)
            {
                throw new Exception("Please login first.");
            }

            var content = new List<KeyValuePair<string, string>>
              {
                    //new KeyValuePair<string, string>("additional", "photo_exif,video_codec,video_quality,thumb_size"),
                    new KeyValuePair<string, string>("additional", "photo_exif,thumb_size"),
                    new KeyValuePair<string, string>("api", "SYNO.PhotoStation.Photo"),
                    new KeyValuePair<string, string>("filter_smart", smartAlbumId),
                    new KeyValuePair<string, string>("limit", limit.ToString()),
                    new KeyValuePair<string, string>("method", "list"),
                    new KeyValuePair<string, string>("offset", offset.ToString()),
                    //new KeyValuePair<string, string>("sort_by", "filename"),
                    new KeyValuePair<string, string>("sort_by", "takendate"),
                    new KeyValuePair<string, string>("sort_direction", "desc"),
                    //new KeyValuePair<string, string>("sort_direction", "asc"),
                    //new KeyValuePair<string, string>("type", "album,photo,video"),
                    new KeyValuePair<string, string>("type", "photo"),
                    new KeyValuePair<string, string>("version", "1")
                };

            var requestContent = new FormUrlEncodedContent(content);

            var response = await httpClient.PostAsync($"photo/webapi/photo.php?SynoToken={sessionId}", requestContent).ConfigureAwait(false);
            var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var listItemsResponse = JsonConvert.DeserializeObject<ListItemResponse>(responseString);
            return listItemsResponse;
        }

        public async Task<ListItemResponse> ListPhotosAsync(string parentId, int offset = 0, int limit = 100)
        {
            if (this.sessionId == null)
            {
                throw new Exception("Please login first.");
            }

            var content = new List<KeyValuePair<string, string>>
              {
                    new KeyValuePair<string, string>("additional", "album_permission,photo_exif,video_codec,video_quality,thumb_size,file_location"),
                    new KeyValuePair<string, string>("api", "SYNO.PhotoStation.Album"),
                    new KeyValuePair<string, string>("limit", limit.ToString()),
                    new KeyValuePair<string, string>("method", "list"),
                    new KeyValuePair<string, string>("offset", offset.ToString()),
                    new KeyValuePair<string, string>("recursive", "false"),
                    //new KeyValuePair<string, string>("sort_by", "filename"),
                    new KeyValuePair<string, string>("sort_by", "takendate"),
                    new KeyValuePair<string, string>("sort_direction", "desc"),
                    //new KeyValuePair<string, string>("sort_direction", "asc"),
                    //new KeyValuePair<string, string>("type", "album,photo,video"),
                    new KeyValuePair<string, string>("type", "photo"),
                    new KeyValuePair<string, string>("version", "1")
                };

            if (parentId != null)
            {
                content.Add(new KeyValuePair<string, string>("id", parentId));
            }

            var requestContent = new FormUrlEncodedContent(content);


            var response = await httpClient.PostAsync($"photo/webapi/album.php?SynoToken={sessionId}", requestContent).ConfigureAwait(false);
            var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var listItemsResponse = JsonConvert.DeserializeObject<ListItemResponse>(responseString);
            return listItemsResponse;
        }

        public string GetThumbnailUrl(PhotoItem item)
        {
            return $"{this.url}/photo/webapi/thumb.php?api=SYNO.PhotoStation.Thumb&method=get&version=1&size=small&id={item.id}&rotate_version=1&thumb_sig={item.additional.thumb_size.sig}&mtime={item.additional.thumb_size.small.mtime}&SynoToken={this.sessionId}";
        }

        public string GetBiglUrl(PhotoItem item)
        {
            return $"{this.url}/photo/webapi/thumb.php?api=SYNO.PhotoStation.Thumb&method=get&version=1&size=large&id={item.id}&rotate_version=1&thumb_sig={item.additional.thumb_size.sig}&mtime={item.additional.thumb_size.large.mtime}&SynoToken={this.sessionId}";
        }

        public async Task<Stream> GetThumbnailData(string thumbUrl)
        {
            var result = await httpClient.GetStreamAsync(thumbUrl);
            return result;
        }

        public async Task<bool> LoginAsync()
        {
            try
            {
                httpClient.CancelPendingRequests();
                httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(url);
                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("api", "SYNO.PhotoStation.Auth"),
                    new KeyValuePair<string, string>("enable_syno_token", "true"),
                    new KeyValuePair<string, string>("method", "photo_login"),
                    new KeyValuePair<string, string>("password", this.password),
                    new KeyValuePair<string, string>("remember_me", "on"),
                    new KeyValuePair<string, string>("username", this.username),
                    new KeyValuePair<string, string>("version", "1")
                });

                var response = await httpClient.PostAsync("photo/webapi/auth.php", content).ConfigureAwait(false);

                var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                var loginResponse = JsonConvert.DeserializeObject<LoginResponse>(responseString);
                if (loginResponse?.success != true || loginResponse?.data?.sid == null)
                {
                    return false;
                }

                this.sessionId = loginResponse.data.sid;
                return true;
            }
            catch (Exception e)
            {

            }

            return false;
        }

    }
}
