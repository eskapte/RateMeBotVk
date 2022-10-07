namespace RateMeBotVk.Configuration;

public class AppSettings
{
    public uint LongPollVersion { get; set; }
    public ulong GroupId { get; set; }
    public int OnErrorDelay { get; set; }
    public int ProcessChatsPerSecond { get; set; }
}
