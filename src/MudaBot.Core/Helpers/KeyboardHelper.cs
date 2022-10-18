using Newtonsoft.Json;
using MudaBot.Core.Models;
using VkNet.Enums.SafetyEnums;
using VkNet.Model.Keyboard;

namespace MudaBot.Core.Helpers;

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
        var showCommentsBtn = CreateButtonOptions(
            $"Комментарии {user.CommentsCount}", new Payload(CommandType.ShowComments));
        var writeCommentBtn = CreateButtonOptions("Оставить комментарий", new Payload(CommandType.WriteComment));
        var dislikeBtn = CreateButtonOptions($"👎🏻 {user.Dislikes}", new Payload(CommandType.Dislike, user));

        return new KeyboardBuilder()
            .AddButton(showCommentsBtn, KeyboardButtonColor.Default)
            .AddLine()
            .AddButton(writeCommentBtn, KeyboardButtonColor.Primary)
            .AddLine()
            .AddButton(dislikeBtn, KeyboardButtonColor.Negative)

            .Build();
    }

    public static MessageKeyboard GetSelfProfileActionsKeyboard(UserProfile user)
    {
        var showCommentsBtn = CreateButtonOptions(
            $"Комментарии {user.CommentsCount}", new Payload(CommandType.ShowComments));

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
