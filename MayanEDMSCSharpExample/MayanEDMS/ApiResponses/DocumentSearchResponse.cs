using MayanEDMSCSharpExample.MayanEDMS.Models;
using System.Collections.Generic;

namespace MayanEDMSCSharpExample.MayanEDMS.ApiResponses
{
    public class DocumentSearchResponse
    {
        public int count { get; set; }
        public object next { get; set; }
        public object previous { get; set; }
        public List<DocumentResult> results { get; set; }
    }
}
