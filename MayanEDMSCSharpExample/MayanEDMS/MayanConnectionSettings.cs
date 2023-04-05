
namespace MayanEDMSCSharpExample.MayanEDMS
{
    public interface IMayanConnectionSettings
    {
        string api_url { get; set; }
        string username { get; set; }
        string password { get; set; }
    }

    public class MayanConnectionSettings : IMayanConnectionSettings
    {
        public string api_url { get; set; }
        public string username { get; set; }
        public string password { get; set; }
    }
}
