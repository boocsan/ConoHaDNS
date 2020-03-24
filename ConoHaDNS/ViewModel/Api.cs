using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ConoHaDNS.Models;
using Newtonsoft.Json;

namespace ConoHaDNS.ViewModel
{
    internal class Api
    {
        public static string Email;
        private static readonly HttpClient Client = new HttpClient();

        public static async void GetToken()
        {
            if (!File.Exists("config.json"))
            {
                File.CreateText("config.json");
                Console.WriteLine("Please Edit config.json.");
                Console.WriteLine("{");
                Console.WriteLine("    \"Username\": xxxxxxx,");
                Console.WriteLine("    \"Password\": xxxxxxx,");
                Console.WriteLine("    \"TenantName\": xxxxxxx,");
                Console.WriteLine("    \"MailAddress\": xxxxxxx,");
                Console.WriteLine("}");
                Environment.Exit(0);
            }
            var config = JsonConvert.DeserializeObject<Config>(File.ReadAllText("config.json"));
            Email = config.MailAddress;

            var data = new
            {
                auth = new
                {
                    passwordCredentials = new
                    {
                        username = config.Username,
                        password = config.Password
                    },
                    tenantName = config.TenantName
                }
            };
            var c = new StringContent(JsonConvert.SerializeObject(data));
            c.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var t = await Client.PostAsync("https://identity.tyo1.conoha.io/v2.0/tokens", c).Result.Content.ReadAsStringAsync();
            t = JsonConvert.DeserializeObject<ConoHaToken>(t).access.token.id;
            Client.DefaultRequestHeaders.Add("X-Auth-Token", t);
        }

        public static async Task<string> GetApiAsync(string url)
        {
            return await Client.GetStringAsync(url);
        }
        
        public static async Task<string> PostApiAsync(string url, dynamic data)
        {
            Console.WriteLine(JsonConvert.SerializeObject(data));
            var c = new StringContent(JsonConvert.SerializeObject(data));
            c.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var statusCode = (await Client.PostAsync(url, c)).StatusCode;
            return $"{(int)statusCode} {statusCode}";
        }

        public static async Task<string> PutApiAsync(string url, dynamic data)
        {
            var c = new StringContent(JsonConvert.SerializeObject(data));
            c.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var statusCode = (await Client.PutAsync(url, c)).StatusCode;
            return $"{(int)statusCode} {statusCode}";
        }

        public static async Task<string> DeleteApiAsync(string url)
        {
            var statusCode = (await Client.DeleteAsync(url)).StatusCode;
            return $"{(int)statusCode} {statusCode}";
        }
    }
}
