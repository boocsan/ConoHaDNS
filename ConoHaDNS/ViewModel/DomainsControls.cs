using System;
using System.Linq;
using System.Threading.Tasks;
using ConoHaDNS.Models;
using Newtonsoft.Json;
using Sharprompt;

namespace ConoHaDNS.ViewModel
{
    internal class DomainsControls
    {
        public static (string, string)[] Domains; // select, id
        public static (string, string) Domain; // select, id

        public static async Task<int> SelectDomain()
        {
            var domains = await Api.GetApiAsync("https://dns-service.tyo1.conoha.io/v1/domains");
            var domainsJson = JsonConvert.DeserializeObject<Domains>(domains);
            Domains = domainsJson.domains.Select(x => (x.name, x.id)).ToArray();

            var domainsList = Domains.Select(x => x.Item1).ToList();
            domainsList.Insert(0, "CREATE DOMAIN");
            domainsList.Add("EXIT");
            var select = Prompt.Select("Select Domain", domainsList.Select(x => x.TrimEnd('.')));
            if (select == "CREATE DOMAIN") return 2;
            if (select == "EXIT") return 99;
            Domains.ToDictionary(x => x.Item1, x => x.Item2).TryGetValue(select + ".", out var id);
            Domain = (select, id);
            return 1;
        }

        public static int SelectControl()
        {
            var controls = new[] { "RECORDS CONTROL", "DELETE", "RETURN TO TOP" };
            var select = Prompt.Select("Select domains control", controls);
            switch (select)
            {
                case "RECORDS CONTROL": return 4;
                case "DELETE": return 3;
                default: return 0;
            }
        }

        public static async Task<int> CreateDomain()
        {
            var name = Prompt.Input<string>("Input new domain name");
            if (!Prompt.Confirm($"Add domain name is {name}. OK?")) return 0;

            var data = new { name = name + ".", email = Api.Email };
            var result = await Api.PostApiAsync("https://dns-service.tyo1.conoha.io/v1/domains", data);
            Console.WriteLine(result);

            return 0;
        }

        public static async Task<int> DeleteDomain()
        {
            if (!Prompt.Confirm($"Delete domain {Domain.Item1} OK?")) return 0;

            var result = await Api.DeleteApiAsync($"https://dns-service.tyo1.conoha.io/v1/domains/{Domain.Item2}");
            Console.WriteLine(result);

            return 0;
        }
    }
}
