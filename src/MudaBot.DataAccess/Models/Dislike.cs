using MudaBot.DataAccess.Infrastructure;
using System;
using System.ComponentModel.DataAnnotations;

namespace MudaBot.DataAccess.Models;
public class Dislike : TimeAccounting
{
    [Key]
    public long Id { get; set; }

    public long RatedUserId { get; set; }
    public User? RatedUser { get; set; }

    public long RatingUserId { get; set; }
    public User? RatingUser { get; set; }

    public override DateTime Created { get; set; }
    public override DateTime Updated { get; set; }
}
