namespace IdentityServerConfig.Models;

public class ReferenceTokenDataModel
{
    public enum Status
    {
        NotChecked,
        Invalid,
        Active,
        Expired
    }
    public DateTime? CreationTime { get; set; }
    public DateTime? Expiration { get; set; }
    public string? ClientId { get; set; }
}