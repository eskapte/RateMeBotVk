using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RateMeBotVk.DataAccess.Models;
public class Rate
{
    public long Id { get; set; }

    [Range(1, 5)]
    public float RateValue { get; set; }

    public long RatedUserId { get; set; }

    [ForeignKey(nameof(RatedUserId))]
    public User RatedUser { get; set; }

    public long RatingUserId { get; set; }

    [ForeignKey(nameof(RatedUserId))]
    public User RatingUser { get; set; }

    public DateTime Date { get; set; }
}
