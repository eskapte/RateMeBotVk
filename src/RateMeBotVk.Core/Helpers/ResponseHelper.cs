using RateMeBotVk.Core.Models;
using RateMeBotVk.Extensions;
using System;
using System.Collections.Generic;
using VkNet.Model.Attachments;
using VkNet.Model.RequestParams;

namespace RateMeBotVk.Helpers;

public static class ResponseHelper
{
    public static MessagesSendParams Start => new()
    {
        Message =  "Чтобы воспользоваться ботом напишите и отправьте id пользователя или ссылку на его страницу\n" +
                   "К примеру: @username, username, https://vk.com/username, id229294556, 229294556",
        Keyboard = KeyboardHelper.GetMain(),
        RandomId = GetRandomId()
    };

    public static MessagesSendParams UserProfile(UserProfile user) => new()
    {
        Message =   $"Информация о Вашем профиле\n" +
                    $"Положительных: {user.Likes} 👍🏻\n" +
                    $"Отрицательных: {user.Dislikes} 👎🏻\n" +
                    $"Всего оценок: {user.RatesCount}",
        Keyboard = KeyboardHelper.GetCurrentProfileActionsKeyboard(user).AsInline(),
        RandomId = GetRandomId()
    };

    public static MessagesSendParams FoundUserProfile(UserProfile user) => new()
    {
        Message =   $"Профиль [{user.Username}|{user.FullName}]\n" +
                    $"Всего оценок: {user.RatesCount}",
        Keyboard = KeyboardHelper.GetProfileActionsKeyboard(user).AsInline(),
        RandomId = GetRandomId()
    };

    public static MessagesSendParams CommandNotFount => new()
    {
        Message = "Такой команды не существует",
        RandomId = GetRandomId()
    };

    public static MessagesSendParams NotFoundUser => new()
    {
        Message = "Не удалось найти такого пользователя",
        RandomId = GetRandomId()
    };

    private static long GetRandomId() => DateTime.Now.Ticks;
}
