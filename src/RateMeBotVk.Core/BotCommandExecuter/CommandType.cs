namespace RateMeBotVk.BotCommandExecuter;

public enum CommandType : byte
{
    Search = 1,
    Start,
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
