using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.ComponentModel.DataAnnotations;

namespace RateMeBotVk.BotCommandExecuter;

[Serializable]
public class Command
{
    [JsonConverter(typeof(StringEnumConverter))]
    public CommandType CommandType { get; set; }

    [Range(1, 5)]
    public short? RateValue { get; set; }

    public Command(CommandType type, short? rateValue = null)
    {
        CommandType = type;
        RateValue = rateValue;
    }
}
