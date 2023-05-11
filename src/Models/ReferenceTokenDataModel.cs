namespace IdentityServerConfig.Models;

public class ReferenceTokenDataModel
{
    public string Status { get; set; } = "notChecked";
    public DateTime? CreationTime { get; set; }
    public DateTime? Expiration { get; set; }
    public string? ClientId { get; set; }
}