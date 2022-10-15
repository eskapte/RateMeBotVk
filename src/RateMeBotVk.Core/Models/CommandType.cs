namespace RateMeBotVk.Core.Models;

public enum CommandType : byte
{
    Start = 1,
    Search,
    Rate,
    RateSetValue,
    RatesWatch,
    Settings,
    AboutMe,
    Back,
    Continue,
    SubcribeOnUpdates,
    UnsubcribeOnUpdates,
}
