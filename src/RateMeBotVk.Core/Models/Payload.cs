using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace RateMeBotVk.Core.Models;

[Serializable]
public class Payload
{
    [JsonConverter(typeof(StringEnumConverter))]
    public CommandType Command { get; set; } = CommandType.Search;

    public Payload(CommandType command)
    {
        Command = command;
    }
}
