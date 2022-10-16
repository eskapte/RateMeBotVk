using Newtonsoft.Json;
using RateMeBotVk.Core.Models;
using VkNet.Enums.SafetyEnums;
using VkNet.Model.Keyboard;

namespace RateMeBotVk.Helpers;

public static class KeyboardHelper
{
    private static MessageKeyboard? _mainKeyboard;

    public static MessageKeyboard GetMain()
    {
        if (_mainKeyboard is not null)
            return _mainKeyboard;

        var aboutMeBtn = CreateButtonOptions("Мой профиль", new Payload(CommandType.UserProfile), false);

        _mainKeyboard = new KeyboardBuilder()
            .AddButton(aboutMeBtn, KeyboardButtonColor.Positive)
            .Build();

        return _mainKeyboard;
    }

    public static MessageKeyboard GetProfileActionsKeyboard(UserProfile user)
    {
        var dislikeBtn = CreateButtonOptions($"👎🏻 {user.Dislikes}", new Payload(CommandType.Rate, user, false));
        var likeBtn = CreateButtonOptions($"👍🏻 {user.Likes}", new Payload(CommandType.Rate, user, true));
        var writeCommentBtn = CreateButtonOptions("Оставить комментарий", new Payload(CommandType.WriteComment));
        var showCommentsBtn = CreateButtonOptions(
            $"Комментарии {user.RatesCount}", new Payload(CommandType.ShowComments));

        return new KeyboardBuilder()
            .AddButton(dislikeBtn, user.Dislikes > user.Likes 
                ? KeyboardButtonColor.Negative : KeyboardButtonColor.Default)
            .AddButton(likeBtn, user.Likes > user.Dislikes 
                ? KeyboardButtonColor.Positive : KeyboardButtonColor.Default)
            .AddLine()
            .AddButton(writeCommentBtn, KeyboardButtonColor.Primary)
            .AddLine()
            .AddButton(showCommentsBtn, KeyboardButtonColor.Default)
            .Build();
    }

    public static MessageKeyboard GetCurrentProfileActionsKeyboard(UserProfile user)
    {
        var showCommentsBtn = CreateButtonOptions(
            $"Комментарии {user.RatesCount}", new Payload(CommandType.ShowComments));

        return new KeyboardBuilder()
            .AddButton(showCommentsBtn, KeyboardButtonColor.Default)
            .Build();
    }


    private static MessageKeyboardButtonAction CreateButtonOptions(
        string text, 
        Payload command, 
        bool isCallback = true)
    {
        var payload = JsonConvert.SerializeObject(command);

        return new MessageKeyboardButtonAction
        {
            Label = text,
            Payload = payload,
            Type = isCallback ? KeyboardButtonActionType.Callback : KeyboardButtonActionType.Text
        };
    }
}
