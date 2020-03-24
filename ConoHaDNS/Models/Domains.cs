using System;

namespace ConoHaDNS.Models
{
    public class Domains
    {
        public Domain[] domains { get; set; }
    }

    public class Domain
    {
        public DateTime created_at { get; set; }
        public object description { get; set; }
        public string email { get; set; }
        public int gslb { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public int serial { get; set; }
        public int ttl { get; set; }
        public DateTime? updated_at { get; set; }
    }

}
