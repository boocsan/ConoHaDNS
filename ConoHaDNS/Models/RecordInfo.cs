using System;

namespace ConoHaDNS.Models
{
    internal class RecordInfo
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
        public object priority { get; set; }
        public object ttl { get; set; }
        public string type { get; set; }
        public object updated_at { get; set; }
    }
}
