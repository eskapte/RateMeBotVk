namespace RateMeBotVk
{
    public static class ResponseTemplates
    {
        public const string StartMessage = "Добро пожаловать!\n" +
                                           "Этот бот позволяет оставлять и просматривать отзывы о пользователях Вконтакте\n" +
                                           "Это может быть очень полезно при знакомстве через интернет, так Вы будете " +
                                           "лучше понимать, с каким человеком общаетесь\n" +
                                           "Оставлять можно как хорошие, так и плохие отзывы\n" + 
                                           "Чтобы просмотреть или оставить отзывы о ком-то, просто напишите мне username пользователя " +
                                           "или его ссылку на вк\n" +
                                           "Вы также будете получать уведомления от меня, если кто-то оставит о Вас отзыв. " +
                                           "Чтобы отключить их перейдите в настройки и нажмите \"Отключить уведомления\"";

        public static string SubcribeGroup(string name) => $"{name}, подпишитесь на нашу [https://vk.com/rate_me_bot|группу]. " +
                                                            "В ней мы выкладываем " +
                                                            "все свежие новости о боте, а также в обсуждениях Вы сможете написать " +
                                                            "о пожеланиях и рекомендациях";

        public static string AboutMe(
            string name, 
            float rating = 0f, 
            int ratesCount = 0) => $"{name}, информация о Вашем профиле\n" +
                                                $"Рейтинг: {rating}\n" +
                                                $"Кол-во отзывов: {ratesCount}";

        public static string GetUserProfile(
            string name,
            string profileUrl,
            float rating = 0f,
            int ratesCount = 0) => $"Информация о профиле {name}\n" +
                                                $"{profileUrl}\n" + 
                                                $"Рейтинг: {rating}\n" +
                                                $"Кол-во отзывов: {ratesCount}";

        public const string CommandNotFound = "Такой команды не существует";
    }
}
