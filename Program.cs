using System;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace vimeo_sdk
{
    public class Vimeo
    {
        private const string VIMEO_BASE_URL = "https://api.vimeo.dev";
        private string client_id = "1b678714cef81fc8c2d421b4e1335d2a491c5933";
        private string client_secret = "HwNGms9jQJeW44WW1iF82oEt3ShUe/qZSCi8k9CzU/znUIbu5yGIykfW/JzCFuc1gX7YMMJbWKCmoXQQpVPKDBH9MaEIdjkBL4EyXDe9XQ7HJaqiVn33VPjlNNNuyC/E";
		protected HttpClient client = new HttpClient();

        public Vimeo()
        {
            // var body = new Dictionary<string, string>
            // {
            //     {"grant_type", "client_credentials"},
            //     {"scope", "public private"}
            // };
            // var content = new FormUrlEncodedContent(body);
            // var response = CallVimeoApi(content);
		}

        public string Base64encode(string text)
        {
            var text_to_encode = Encoding.UTF8.GetBytes(text);
            return System.Convert.ToBase64String(text_to_encode);
        }

        public AccessToken GetUnauthenticatedToken(string scopes = "public")
        {
            var body = new Dictionary<string, string>
            {
                {"grant_type", "client_credentials"},
                {"scope", scopes}
            };

            var payload = BuildJsonPayload(body);

            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", "Basic " + Base64encode(client_id + ":" + client_secret));
            client.DefaultRequestHeaders.Add("Accept", "application/vnd.vimeo.*+json");

            var token = JsonConvert.DeserializeObject<AccessToken>(CallVimeoApi("POST", "/oauth/authorize/client", payload).Result);
            return token;
        }

        private HttpContent BuildJsonPayload(Dictionary<string, string> body)
        {
            return new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
        }

        public AccessToken GetAuthenticatedToken(string redirect_uri = "", string scope = "public private", string state = "")
        {

            var body = new Dictionary<string, string>
            {
                {"response_type", "code"},
                {"client_id", client_id},
                {"redirect_uri", redirect_uri},
                {"scope", scope},
                {"state", state}
            };

            var payload = BuildJsonPayload(body);

            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Accept", "application/vnd.vimeo.*+json");
            
            var response = CallVimeoApi("GET", "/oauth/authorize?client_id=");
            return null;
        }

        public async Task<string> CallVimeoApi(string method = null, string url = null, HttpContent parameters = null)
        {
            HttpResponseMessage callTask;
            Task<string> response = null;
            
            //string to lower or upper the method
            switch(method){
                case "GET":
                callTask = await client.GetAsync(VIMEO_BASE_URL + url);
                response = callTask.Content.ReadAsStringAsync();
                break;
                case "POST":
                callTask = await client.PostAsync(VIMEO_BASE_URL + url, parameters);
                response = callTask.Content.ReadAsStringAsync();
                break;
                case "PUT":
                callTask = await client.PutAsync(VIMEO_BASE_URL + url, parameters);
                response = callTask.Content.ReadAsStringAsync();
                break;
                case "DELETE":
                callTask = await client.DeleteAsync(VIMEO_BASE_URL + url);
                //response for DELETE 
                break;
            }
            
            return response.Result;

            // new MediaTypeWithQualityHeaderValue
            // try
            // {
            // // client.BaseAddress = new Uri(VIMEO_BASE_URL);
            // client.DefaultRequestHeaders.Clear();
            // // client.DefaultRequestHeaders.Add("Authorization", "Basic " + Base64encode(client_id + ":" + client_secret));
            // // var callTask = await client.PostAsync(VIMEO_BASE_URL + "/oauth/authorize/client", parameters);
            // client.DefaultRequestHeaders.Add("Authorization", "Bearer a51183782a705043113b134ca221a1e2");
            // var callTask = await client.GetAsync(VIMEO_BASE_URL + "/me");
            // // var response = await callTask;
            // var response = callTask.Content.ReadAsStringAsync();
            // Console.Write("Hellololol ");
            // Console.Write(response);
            // }
            // catch (Exception e) {

            // }
        }
		
       public static void Main(string[] args)
		{
            var vimeo = new Vimeo();
            var response = vimeo.GetUnauthenticatedToken();
            Console.WriteLine(response.Access_token);
		}
    }


}
