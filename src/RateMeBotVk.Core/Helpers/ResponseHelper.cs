using RateMeBotVk.Extensions;
using VkNet.Model.RequestParams;

namespace RateMeBotVk.Helpers;

public static class ResponseHelper
{
    public static MessagesSendParams Start => new()
    {
        Message =  "Добро пожаловать!\n" +
                   "Этот бот позволяет оставлять и просматривать отзывы о пользователях Вконтакте\n" +
                   "Это может быть очень полезно при знакомстве через интернет, так Вы будете " +
                   "лучше понимать, с каким человеком общаетесь\n" +
                   "Оставлять можно как хорошие, так и плохие отзывы\n" +
                   "Чтобы просмотреть или оставить отзывы о ком-то, просто напишите мне username пользователя " +
                   "или его ссылку на вк\n" +
                   "Вы также будете получать уведомления от бота, если кто-то оставит о Вас отзыв. " +
                   "Чтобы отключить их перейдите в настройки и нажмите \"Отписаться\"",
        Keyboard = KeyboardHelper.GetMain()
    };

    public static MessagesSendParams Subscribe() => new()
    {
        Message =  $"Подпишитесь на нашу [https://vk.com/rate_me_bot|группу]. " +
                    "В ней мы выкладываем " +
                    "все свежие новости о боте, а также в обсуждениях Вы сможете написать " +
                    "о пожеланиях и рекомендациях",
        Keyboard = KeyboardHelper.GetMain()
    };

    public static MessagesSendParams SetRateValue(bool isValid = true)
    {
        var message = $"{(isValid ? "" : "Некорректное значение\n")}" +
                      $"Как бы вы оценили этого пользователя от 1 до 5?";

        return new()
        {
            Message = message,
            Keyboard = KeyboardHelper.GetRateValuesKeyboard().AsInline()
        };
    }

    public static MessagesSendParams SetRateText(short rateValue) => new()
    {
        Message = "Вы можете обосновать свою оценку комментарием до 300 символов\n" +
                  "Это не обязательно",
        Keyboard = KeyboardHelper.GetContinueKeyboard(rateValue).AsOneTime()
    };

    public static MessagesSendParams Settings(bool isSubcribed) => new()
    {
        Message =   "Описание команд:\n" +
                    "\n" +
                    (isSubcribed 
                        ? "Отписаться - не получать уведомлений от бота\n" 
                        : "Подписаться - получать уведомления от бота\n"),
        Keyboard = KeyboardHelper.GetSettings()
    };

    public static MessagesSendParams AboutMe(float rating = 0f, int ratesCount = 0) => new()
    {
        Message =   $"Информация о Вашем профиле\n" +
                    $"Рейтинг: {rating}\n" +
                    $"Кол-во отзывов: {ratesCount}",
        Keyboard = KeyboardHelper.GetMain()
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
        Keyboard = KeyboardHelper.GetRateKeyboard().AsInline()
    };

    public static MessagesSendParams UserWithoutRating(string fullName, string username) => new()
    {
        Message = $"Информация о профиле [{username}|{fullName}]\n" +
                  $"Об этом пользователе ещё нет отзывов\n" + 
                  $"Будьте первыми - оставьте отзыв!",
        Keyboard = KeyboardHelper.GetRateKeyboard().AsInline()
    };

    public static MessagesSendParams CommandNotFount => new()
    {
        Message = "Такой команды не существует"
    };

    public static MessagesSendParams NotFoundUser => new()
    {
        Message = "Не удалось найти такого пользователя"
    };

    public static MessagesSendParams Back => new()
    {
        Message = "Возвращаемся",
        Keyboard = KeyboardHelper.GetMain()
    };

    public static MessagesSendParams Success => new()
    {
        Message = "Спасибо за ваш отзыв!",
        Keyboard = KeyboardHelper.GetMain()
    };
}
