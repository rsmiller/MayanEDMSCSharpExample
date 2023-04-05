using MayanEDMSCSharpExample.MayanEDMS.Models;
using System;

namespace MayanEDMSCSharpExample.MayanEDMS.ApiResponses
{
    public class CreatedDocumentResponse
    {
        public DateTime datetime_created { get; set; }
        public string description { get; set; }
        public string document_change_type_url { get; set; }
        public MayanDocumentType document_type { get; set; }
        public string file_list_url { get; set; }
        public int id { get; set; }
        public string label { get; set; }
        public string language { get; set; }
        public object file_latest { get; set; }
        public string url { get; set; }
        public string uuid { get; set; }
        public object version_active { get; set; }
        public string version_list_url { get; set; }
    }

}
