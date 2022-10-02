using System;

namespace RateMeBotVk.Models.Users;

public abstract record AbstractUser(string Username);

public record UserWithRating : AbstractUser
{
    public long Id { get; init; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhotoUrl { get; set; }

    public UserWithRating(string username, long id) : base(username)
    {
        Id = id;
    }
}

public record UserWithoutRating : AbstractUser
{
    public long Id { get; init; }
    public Uri PhotoUrl { get; set; }
    public string FullName { get; set; }

    public UserWithoutRating(string username) : base(username)
    {

    }
}

public record NotFoundUser(string Username) : AbstractUser(Username);
