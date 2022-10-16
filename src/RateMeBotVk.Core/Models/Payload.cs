using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using VkNet.Enums.SafetyEnums;

namespace RateMeBotVk.Core.Models;

[Serializable]
public class Payload
{
    [JsonConverter(typeof(StringEnumConverter))]
    public CommandType Command { get; set; } = CommandType.Search;
    public UserProfile? UserProfile { get; set; }
    public bool? IsLike { get; set; }

    public Payload(CommandType command, UserProfile? userProfile = null, bool? isLike = null)
    {
        Command = command;
        UserProfile = userProfile;
        IsLike = isLike;
    }

    //public Payload ShowNotification(string text)
    //{
    //    Type = MessageEventType.SnowSnackbar;
    //    Text = text;

    //    return this;
    //}
}
