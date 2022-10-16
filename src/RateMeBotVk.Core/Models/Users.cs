namespace RateMeBotVk.Core.Models;

public abstract class AbstractUser { };

public class NotFoundUser : AbstractUser { };

public class UserProfile : AbstractUser
{
    public string Username { get; set; }
    public string FullName { get; set; }
    public long UserId { get; set; }
    public int Likes { get; set; }
    public int Dislikes { get; set; }
    public int RatesCount { get; set; }

    public UserProfile(
        string username, 
        string fullName, 
        long userId, 
        int likes = 0, 
        int dislikes = 0, 
        int ratesCount = 0)
    {
        Username = username;
        FullName = fullName;
        UserId = userId;
        Likes = likes;
        Dislikes = dislikes;
        RatesCount = ratesCount;
    }
}
