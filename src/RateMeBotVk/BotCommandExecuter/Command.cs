using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace RateMeBotVk.BotCommandExecuter;

[Serializable]
public class Command
{
    [JsonConverter(typeof(StringEnumConverter))]
    public CommandType CommandType { get; set; }

    public Command(CommandType type)
    {
        CommandType = type;
    }
}
