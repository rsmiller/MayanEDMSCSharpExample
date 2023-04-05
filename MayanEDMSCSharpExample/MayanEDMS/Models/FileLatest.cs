using System;

namespace MayanEDMSCSharpExample.MayanEDMS.Models
{
    public class FileLatest
    {
        public string checksum { get; set; }
        public string comment { get; set; }
        public int document_id { get; set; }
        public string document_url { get; set; }
        public string download_url { get; set; }
        public string encoding { get; set; }
        public string file { get; set; }
        public string filename { get; set; }
        public int id { get; set; }
        public string mimetype { get; set; }
        public string page_list_url { get; set; }
        public PagesFirst pages_first { get; set; }
        public int size { get; set; }
        public DateTime timestamp { get; set; }
        public string url { get; set; }
    }
}
