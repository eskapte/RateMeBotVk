using System.ComponentModel.DataAnnotations;

namespace RateMeBotVk.DataAccess.Models;

public class User
{
    public int Id { get; set; }

    [MinLength(5)]
    public string Username { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }
}
