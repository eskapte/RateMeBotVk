using Newtonsoft.Json;
using RateMeBotVk.BotCommandExecuter;
using VkNet.Enums.SafetyEnums;
using VkNet.Model.Keyboard;

namespace RateMeBotVk.Helpers;
public static class KeyboardHelper
{
    private static MessageKeyboard _mainKeyboard;
    private static MessageKeyboard _rateKeyboard;

    public static MessageKeyboard GetMain()
    {
        if (_mainKeyboard is not null)
            return _mainKeyboard;

        var settingsBtn = CreateButtonOptions("Настройки", new Command(CommandType.Settings));
        var aboutMeBtn = CreateButtonOptions("Обо мне", new Command(CommandType.AboutMe));

        _mainKeyboard = new KeyboardBuilder()
            .AddButton(settingsBtn, KeyboardButtonColor.Default)
            .AddButton(aboutMeBtn, KeyboardButtonColor.Positive)
            .Build();

        return _mainKeyboard;
    }

    public static MessageKeyboard GetRateKeyboard()
    {
        if (_rateKeyboard is not null)
            return _rateKeyboard;

        var rateBtn = CreateButtonOptions("Оценить", new Command(CommandType.Rate));
        var showRates = CreateButtonOptions("Отзывы", new Command(CommandType.RatesWatch));

        _rateKeyboard = new KeyboardBuilder()
            .AddButton(rateBtn, KeyboardButtonColor.Primary)
            .AddLine()
            .AddButton(showRates, KeyboardButtonColor.Default)
            .Build();

        return _rateKeyboard;
    }

    public static MessageKeyboard GetSettings()
    {
        var unsubscribeBtn = CreateButtonOptions("Отписаться", new Command(CommandType.UnsubcribeOnUpdates));
        var backBtn = CreateButtonOptions("Назад", new Command(CommandType.Back));

        var settingKeyboard = new KeyboardBuilder()
            .AddButton(unsubscribeBtn, KeyboardButtonColor.Negative)
            .AddLine()
            .AddButton(backBtn, KeyboardButtonColor.Default)
            .Build();

        return settingKeyboard;
    }

    private static MessageKeyboardButtonAction CreateButtonOptions(string text, Command command)
    {
        var payload = JsonConvert.SerializeObject(command);

        return new MessageKeyboardButtonAction
        {
            Label = text,
            Payload = payload,
            Type = KeyboardButtonActionType.Text
        };
    }
}
