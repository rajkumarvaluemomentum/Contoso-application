namespace Contoso_application.Models
{
    public class AppConfiguration
    {
        public string ApplicationName { get; set; }
        public Dictionary<string, string> Settings { get; set; } = new();
    }
}
