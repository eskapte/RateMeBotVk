using MudaBot.DataAccess.Infrastructure;
using System;
using System.ComponentModel.DataAnnotations;

namespace MudaBot.DataAccess.Models;

public class Comment : TimeAccounting
{
    [Key]
    public long Id { get; set; }

    [StringLength(4096)]
    public string Text { get; set; } = string.Empty;

    public long OwnerId { get; set; }
    public User? Owner { get; set; }

    public long ReceivingUserId { get; set; }
    public User? ReceivingUser { get; set; }


    public override DateTime Created { get; set; }
    public override DateTime Updated { get; set; }
}
