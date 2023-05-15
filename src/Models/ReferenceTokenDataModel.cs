namespace IdentityServerConfig.Models;

public class ReferenceTokenDataModel
{
    public enum StatusCode
    {
        NotChecked,
        Invalid,
        Active,
        Expired
    }

    public StatusCode Status { get; set; }
    public DateTime? CreationTime { get; set; }
    public DateTime? Expiration { get; set; }
}