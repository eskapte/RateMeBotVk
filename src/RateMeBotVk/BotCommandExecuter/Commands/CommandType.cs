using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace RateMeBotVk.BotCommandExecuter.Commands;

public enum CommandType : byte
{
    Settings = 1,
    AboutMe,
    Search
}
