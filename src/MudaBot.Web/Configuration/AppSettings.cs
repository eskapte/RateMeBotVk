namespace MudaBot.Web.Configuration;

public class AppSettings
{
    public short ApiVersion { get; set; }
    public ulong GroupId { get; set; }
    public int OnErrorDelay { get; set; }
    public string ConfirmString { get; set; } = string.Empty;
}
