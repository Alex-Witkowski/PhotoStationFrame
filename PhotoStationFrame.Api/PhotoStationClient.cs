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

        public PhotoStationClient()
        {
            httpClient = new HttpClient();
        }

        public void Initialize(string url, string username, string password)
        {
            this.url = url;
            this.username = username;
            this.password = password;
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
            }
            catch (Exception e)
            {

            }
            
            return true;
        }

    }
}
