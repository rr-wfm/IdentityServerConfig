namespace IdentityServerConfig.Models;

public class ReferenceTokenTableData
{
    public string ClientId { get; set; }
    public DateTime? Expiration { get; set; }
    public DateTime CreationTime { get; set; }
    public int ClientTableId { get; set; }
    public long PersistedGrantTableId { get; set; }
    public string ClientName { get; set; }
}