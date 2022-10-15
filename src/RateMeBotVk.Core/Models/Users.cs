namespace RateMeBotVk.Models.Users;

public abstract class AbstractUser
{
    public string Username { get; set; }

    public AbstractUser(string username)
    {
        Username = username;
    }
}

public class UserWithRating : AbstractUser
{
    public string FullName { get; set; } = string.Empty;
    public float Rating { get; set; }
    public int RatesCount { get; set; }

    public UserWithRating(string username) : base(username) { }
}

public class UserWithoutRating : AbstractUser
{
    public string FullName { get; set; } = string.Empty;

    public UserWithoutRating(string username) : base(username) { }
}

public class NotFoundUser : AbstractUser
{
    public NotFoundUser(string username) : base(username) { }
}
