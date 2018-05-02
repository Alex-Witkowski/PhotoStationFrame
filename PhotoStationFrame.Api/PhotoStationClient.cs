using Newtonsoft.Json;
using PhotoStationFrame.Api.Models;
using System;
using System.Collections.Generic;
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

        public async Task ListAlbums()
        {
            if(this.sessionId == null)
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

            var response = await httpClient.PostAsync($"photo/webapi/album.php?SynoToken={sessionId}", content);
            var responseString = await response.Content.ReadAsStringAsync();
            var loginResponse = JsonConvert.DeserializeObject<ListAlbumsResponse>(responseString);

        }

        public async Task<bool> LoginAsync()
        {
            try
            {
                httpClient.BaseAddress = new Uri(url);
                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("api", "SYNO.PhotoStation.Auth"),
                    new KeyValuePair<string, string>("enable_syno_token", "true"),
                    new KeyValuePair<string, string>("method", "photo_login"),
                    new KeyValuePair<string, string>("password", this.password),
                    new KeyValuePair<string, string>("username", this.username),
                    new KeyValuePair<string, string>("version", "1")
                });

                var response = await httpClient.PostAsync("photo/webapi/auth.php", content);

                var responseString = await response.Content.ReadAsStringAsync();

                var loginResponse = JsonConvert.DeserializeObject<LoginResponse>(responseString);
                if(loginResponse?.success != true || loginResponse?.data?.sid == null)
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
