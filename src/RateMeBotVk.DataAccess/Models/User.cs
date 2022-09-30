using System.ComponentModel.DataAnnotations;

namespace RateMeBotVk.DataAccess.Models;

public class User
{
    public int Id { get; set; }

    [MinLength(5)]
    public string Username { get; set; }

    [MaxLength(30)]
    public string FirstName { get; set; }

    [MaxLength(30)]
    public string LastName { get; set; }
}
