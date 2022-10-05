using System;
using System.ComponentModel.DataAnnotations;

namespace RateMeBotVk.DataAccess.Models;
public class Rate
{
    public long Id { get; set; }

    [Range(1, 5)]
    public float RateValue { get; set; }

    [StringLength(300)]
    public string? FeedbackText { get; set; }

    public long RatedUserId { get; set; }
    public User? RatedUser { get; set; }

    public long RatingUserId { get; set; }
    public User? RatingUser { get; set; }

    public DateTime Date { get; set; }
}
