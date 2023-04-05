using System;

namespace MayanEDMSCSharpExample.MayanEDMS.Models
{
    public class VersionActive
    {
        public bool active { get; set; }
        public string comment { get; set; }
        public int document_id { get; set; }
        public string document_url { get; set; }
        public string export_url { get; set; }
        public int id { get; set; }
        public string page_list_url { get; set; }
        public PagesFirst pages_first { get; set; }
        public DateTime timestamp { get; set; }
        public string url { get; set; }
    }
}
