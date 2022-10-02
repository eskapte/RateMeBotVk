using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RateMeBotVk.DataAccess.Models;

public class User
{
    public long Id { get; set; }

    [StringLength(32, MinimumLength = 5)]
    public string Username { get; set; }

    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }

    [Range(1, 5)]
    public float Rating { get; set; }
    public int RatesCount { get; set; }
    public List<Rate> Rates { get; set; }


}
