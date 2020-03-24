using System;
using System.Linq;
using System.Threading.Tasks;
using ConoHaDNS.Models;
using Newtonsoft.Json;
using Sharprompt;

namespace ConoHaDNS.ViewModel
{
    internal class RecordsControls
    {
        public static (string, string, string, string)[] Records; // type, name, id, data
        public static (string, string, string, string) Record; // type, name, id, data
        public static string Type;

        public static int SelectType()
        {
            var types = new[] { "A", "AAAA", "MX", "CNAME", "TXT", "SRV", "NS", "PTR" };
            Type = Prompt.Select("Select record Type", types);
            return 5;
        }

        public static async Task<int> SelectRecord()
        {
            var records = await Api.GetApiAsync($"https://dns-service.tyo1.conoha.io/v1/domains/{DomainsControls.Domain.Item2}/records");
            var recordsJson = JsonConvert.DeserializeObject<Records>(records);
            Records = recordsJson.records.Select(x => (x.type, x.name, x.id, x.data)).OrderBy(x => x.type).ThenBy(x => x.name).ToArray();

            var domainsList = Records.Where(x => x.Item1 == Type).ToArray().Select(x => $"[{x.Item1}] {x.Item2}").ToList();
            domainsList.Add("CREATE RECORD");
            domainsList.Add("RETURN TO TOP");
            var select = Prompt.Select("Select record", domainsList);
            if (select == "CREATE RECORD") return 8;
            if (select == "RETURN TO TOP") return 0;

            Record = Records.First(x => x.Item1 == Type && $"[{Type}] {x.Item2}" == select);

            return 6;
        }


        public static int SelectControl()
        {
            var controls = new[] { "VIEW INFO", "UPDATE", "DELETE", "RETURN TO TOP" };
            var select = Prompt.Select("Select record control", controls);
            switch (select)
            {
                case "VIEW INFO": return 7;
                case "UPDATE": return 9;
                case "DELETE": return 10;
                default: return 0;
            }
        }

        public static async Task<int> ViewRecord()
        {
            var info = await Api.GetApiAsync($"https://dns-service.tyo1.conoha.io/v1/domains/{DomainsControls.Domain.Item2}/records/{Record.Item3}");
            var infoJson = JsonConvert.DeserializeObject<RecordInfo>(info);
            Console.WriteLine($"Name: {infoJson.name.TrimEnd('.')}");
            Console.WriteLine($"Type: {infoJson.type}");
            Console.WriteLine($"Data: {infoJson.data}");

            return 4;
        }

        public static async Task<int> CreateRecord()
        {
            var name = Prompt.Input<string>("Record name");
            var value = Prompt.Input<string>("Record data");

            if (Type == "MX" || Type == "SRV")
            {
                var priority = Prompt.Input<int>("Record priority");
                var data = new { type = Type, name = name + "." + DomainsControls.Domain.Item1 + ".", data = value, priority };
                var result = await Api.PostApiAsync($"https://dns-service.tyo1.conoha.io/v1/domains/{DomainsControls.Domain.Item2}/records", data);
                Console.WriteLine(result);
            }
            else
            {
                var data = new { type = Type, name = name + "." + DomainsControls.Domain.Item1 + ".", data = value };
                var result = await Api.PostApiAsync($"https://dns-service.tyo1.conoha.io/v1/domains/{DomainsControls.Domain.Item2}/records", data);
                Console.WriteLine(result);
            }

            return 4;
        }

        public static async Task<int> UpdateRecord()
        {
            var value = Prompt.Input<string>("Record data");

            var data = new { data = value };
            var result = await Api.PostApiAsync($"https://dns-service.tyo1.conoha.io/v1/domains/{DomainsControls.Domain.Item2}/records/{Record.Item3}", data);
            Console.WriteLine(result);

            return 4;
        }

        public static async Task<int> DeleteRecord()
        {
            if (!Prompt.Confirm($"Delete record {Record.Item1} OK?")) return 5;

            var result = await Api.DeleteApiAsync($"https://dns-service.tyo1.conoha.io/v1/domains/{DomainsControls.Domain.Item2}/records/{Record.Item3}");
            Console.WriteLine(result);

            return 4;
        }
    }
}
