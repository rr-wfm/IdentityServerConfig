namespace IdentityServerConfig.Models;

public class AuditLog
{
    public int ID { get; set; }
    public string UserId { get; set; }
    public string UserName { get; set; }
    public DateTime Timestamp { get; set; }
    public string Action { get; set; }
    public IDictionary<string,string> Data { get; set; }
}
