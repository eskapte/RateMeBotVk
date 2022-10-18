using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace MudaBot.Core.Models;

[Serializable]
public class Payload
{
    [JsonConverter(typeof(StringEnumConverter))]
    public CommandType Command { get; set; } = CommandType.Search;
    public UserProfile? UserProfile { get; set; }

    public Payload(CommandType command, UserProfile? userProfile = null)
    {
        Command = command;
        UserProfile = userProfile;
    }

    //public Payload ShowNotification(string text)
    //{
    //    Type = MessageEventType.SnowSnackbar;
    //    Text = text;

    //    return this;
    //}
}
