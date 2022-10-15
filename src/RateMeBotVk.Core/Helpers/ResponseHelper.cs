using RateMeBotVk.Extensions;
using System;
using VkNet.Model.RequestParams;

namespace RateMeBotVk.Helpers;

public static class ResponseHelper
{
    public static MessagesSendParams Start => new()
    {
        Message =  "Добро пожаловать!\n" +
                   "Этот бот позволяет ставить оценки пользователям Вконтакте\n\n" +
                   "Из оценок будет формироваться рейтинг, по которому можно будет примерно понять, " +
                   "хороший ли это человек или нет\n\n" + 
                   "Чтобы поставить человеку оценку или посмотреть его рейтинг, просто отправьте username " +
                   "или ссылку на вк страничку этого человека",
        Keyboard = KeyboardHelper.GetMain(),
        RandomId = GetRandomId()
    };

    public static MessagesSendParams Subscribe() => new()
    {
        Message =  $"Подпишитесь на нашу [https://vk.com/rate_me_bot|группу]. " +
                    "В ней мы выкладываем " +
                    "все свежие новости о боте, а также в обсуждениях Вы сможете написать " +
                    "о пожеланиях и рекомендациях",
        Keyboard = KeyboardHelper.GetMain(),
        RandomId = GetRandomId()
    };

    public static MessagesSendParams AboutMe(float rating = 0f, int ratesCount = 0) => new()
    {
        Message =   $"Информация о Вашем профиле\n" +
                    $"Рейтинг: {rating}\n" +
                    $"Кол-во отзывов: {ratesCount}",
        Keyboard = KeyboardHelper.GetMain(),
        RandomId = GetRandomId()
    };

    public static MessagesSendParams UserProfile(
        string fullName, 
        string username, 
        float rating = 0f, 
        int ratesCount = 0) => new()
        {
        Message =   $"Информация о профиле [{username}|{fullName}]\n" +
                    $"Рейтинг: {rating}\n" +
                    $"Кол-во отзывов: {ratesCount}",
        Keyboard = KeyboardHelper.GetRateKeyboard().AsInline(),
        RandomId = GetRandomId()
        };

    public static MessagesSendParams UserWithoutRating(string fullName, string username) => new()
    {
        Message = $"Информация о профиле [{username}|{fullName}]\n" +
                  $"Об этом пользователе ещё нет отзывов\n" + 
                  $"Будьте первыми - оставьте отзыв!",
        Keyboard = KeyboardHelper.GetRateKeyboard().AsInline(),
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
