using RateMeBotVk.DataAccess.Infrastructure;
using System;
using System.ComponentModel.DataAnnotations;

namespace RateMeBotVk.DataAccess.Models;
public class Rate : Updatable
{
    [Key]
    public long Id { get; set; }
    public bool IsLike { get; set; }

    [StringLength(300)]
    public string? Comment { get; set; }

    public long RatedUserId { get; set; }
    public User? RatedUser { get; set; }

    public long RatingUserId { get; set; }
    public User? RatingUser { get; set; }

    public DateTime Created { get; set; }
    public override DateTime Updated { get; set; }
}
