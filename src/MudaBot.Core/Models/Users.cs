namespace MudaBot.Core.Models;

public abstract class AbstractUser { };

public class NotFoundUser : AbstractUser { };

public class UserProfile : AbstractUser
{
    public string Username { get; set; }
    public string FullName { get; set; }
    public long UserId { get; set; }
    public int Dislikes { get; set; }
    public int CommentsCount { get; set; }

    public UserProfile(
        string username, 
        string fullName, 
        long userId, 
        int dislikes = 0,
        int commentsCount = 0)
    {
        Username = username;
        FullName = fullName;
        UserId = userId;
        Dislikes = dislikes;
        CommentsCount = commentsCount;
    }
}
