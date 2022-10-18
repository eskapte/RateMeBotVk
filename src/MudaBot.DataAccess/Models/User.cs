using MudaBot.DataAccess.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MudaBot.DataAccess.Models;

public class User : TimeAccounting
{
    [Key]
    public long Id { get; set; }

    [StringLength(32, MinimumLength = 5)]
    public string Username { get; set; } = default!;

    public override DateTime Created { get; set; }
    public override DateTime Updated { get; set; }

    public ICollection<Dislike> GotDislikes { get; set; }
    public ICollection<Dislike> SentDislikes { get; set; }

    public ICollection<Comment> GotComments { get; set; }
    public ICollection<Comment> SendComments { get; set; }

    public User()
    {
        GotDislikes = new List<Dislike>();
        SentDislikes = new List<Dislike>();
        GotComments = new List<Comment>();
        SendComments = new List<Comment>();
    }
}
