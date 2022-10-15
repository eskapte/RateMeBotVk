using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel.DataAnnotations;

namespace RateMeBotVk.Web.Models.Common;

[Serializable]
public class UpdateRequest
{
    [Required]
    public string Type { get; set; } = string.Empty;

    [JsonProperty("object")]
    public JObject? Object { get; set; }

    [Required]
    [JsonProperty("group_id")]
    public ulong GroupId { get; set; }
}
