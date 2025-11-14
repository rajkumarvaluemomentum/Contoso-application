namespace VirtualAssistant.API.Models
{
    public class DeploymentDto
    {
        public long Id { get; set; }
        public string Sha { get; set; }
        public string Ref { get; set; }
        public string Environment { get; set; }
        public string Creator { get; set; }
        public string CreatedAt { get; set; }
        public string Status { get; set; }
    }
}
