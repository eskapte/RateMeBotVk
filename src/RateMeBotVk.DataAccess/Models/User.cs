using RateMeBotVk.DataAccess.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RateMeBotVk.DataAccess.Models;

public class User : Updatable
{
    [Key]
    public long Id { get; set; }

    [StringLength(32, MinimumLength = 5)]
    public string Username { get; set; } = default!;

    public DateTime Created { get; set; }
    public override DateTime Updated { get; set; }

    public ICollection<Rate> GotRates { get; set; }
    public ICollection<Rate> SentRates { get; set; }

    public User()
    {
        GotRates = new List<Rate>();
        SentRates = new List<Rate>();
    }
}
