using System;

namespace ConoHaDNS.Models
{
    internal class Records
    {
        public Record[] records { get; set; }
    }

    public class Record
    {
        public DateTime created_at { get; set; }
        public string data { get; set; }
        public object description { get; set; }
        public string domain_id { get; set; }
        public object gslb_check { get; set; }
        public object gslb_region { get; set; }
        public object gslb_weight { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public int? priority { get; set; }
        public object ttl { get; set; }
        public string type { get; set; }
        public DateTime? updated_at { get; set; }
    }

}
