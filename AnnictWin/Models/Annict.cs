using GraphQL.Types;
using IdentityModel.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace AnnictWin.Models
{
    class Annict
    {
        public async void test()
        {
            HttpClient client = new HttpClient();
            string endpoint = "https://api.annict.com/oauth/authorize";
            Uri uri = new Uri(endpoint);
            string queryString = uri.Query;
            NameValueCollection query = HttpUtility.ParseQueryString(queryString, Encoding.UTF8);

            #region 認証
            query["client_id"] = MyResource.OAuthClientID;
            query["response_type"] = "code";
            query["redirect_uri"] = "http://localhost:8888/";

            System.Diagnostics.Process.Start(endpoint + "?" + query.ToString());

            HttpListener listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:8888/");
            listener.Start();
            //while (true)
            //{
            HttpListenerContext context = listener.GetContext();
            HttpListenerResponse res = context.Response;
            HttpListenerRequest req = context.Request;

            string urlPath = req.RawUrl;
            Console.WriteLine(urlPath);

            res.StatusCode = 200;
            byte[] content = Encoding.UTF8.GetBytes("HELLO");
            res.OutputStream.Write(content, 0, content.Length);
            res.Close();
            //}

            #endregion 認証

            #region アクセストークン取得
            var accessTokenGetQuery = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "client_id", MyResource.OAuthClientID},
                {"client_secret", MyResource.OAuthClientSecret},
                {"grant_type", "authorization_code"},
                {"redirect_uri", "http://localhost:8888/"},
                { "code", urlPath.Replace("/?code=", "")}
            });

            var accessTokenGetResponse = await client.PostAsync("https://api.annict.com/oauth/token", accessTokenGetQuery);
            string accessTokenStr = await accessTokenGetResponse.Content.ReadAsStringAsync();
            AccessToken accessTokenJson = JsonConvert.DeserializeObject<AccessToken>(accessTokenStr);

            Console.WriteLine(accessTokenJson.access_token);
            #endregion

            #region API呼び出し
            var schema = new Schema { Query = new StarWarsQuery() };
            #endregion
        }
    }

    public class StarWarsQuery : ObjectGraphType
    {
        public StarWarsQuery()
        {
            
        }
    }

    /// <summary>
    /// アクセストークン取得レスポンスJSON
    /// </summary>
    [JsonObject("token")]
    public class AccessToken
    {
        [JsonProperty("access_token")]
        public string access_token { get; set; }
        [JsonProperty("token_type")]
        public string token_type { get; set; }
        [JsonProperty("scope")]
        public string scope { get; set; }
        [JsonProperty("created_at")]
        public int created_at { get; set; }
    }
}
