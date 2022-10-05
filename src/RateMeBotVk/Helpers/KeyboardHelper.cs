using Newtonsoft.Json;
using RateMeBotVk.BotCommandExecuter;
using System.Dynamic;
using System.Linq;
using VkNet.Enums.SafetyEnums;
using VkNet.Model.Keyboard;

namespace RateMeBotVk.Helpers;
public static class KeyboardHelper
{
    private static MessageKeyboard _mainKeyboard;
    private static MessageKeyboard _rateKeyboard;
    private static MessageKeyboard _rateValuesKeyboard;
    private static MessageKeyboard _continueKeyboard;

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

    public static MessageKeyboard GetRateValuesKeyboard()
    {
        if (_rateValuesKeyboard is not null)
            return _rateValuesKeyboard;

        var rate1 = CreateButtonOptions("1", new Command(CommandType.RateSetValue, 1));
        var rate2 = CreateButtonOptions("2", new Command(CommandType.RateSetValue, 2));
        var rate3 = CreateButtonOptions("3", new Command(CommandType.RateSetValue, 3));
        var rate4 = CreateButtonOptions("4", new Command(CommandType.RateSetValue, 4));
        var rate5 = CreateButtonOptions("5", new Command(CommandType.RateSetValue, 5));


        _rateValuesKeyboard = new KeyboardBuilder()
            .AddButton(rate1, KeyboardButtonColor.Negative)
            .AddButton(rate2, KeyboardButtonColor.Default)
            .AddButton(rate3, KeyboardButtonColor.Default)
            .AddLine()
            .AddButton(rate4, KeyboardButtonColor.Default)
            .AddButton(rate5, KeyboardButtonColor.Positive)
            .Build();

        return _rateValuesKeyboard;
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

    public static MessageKeyboard GetContinueKeyboard(short rateValue)
    {
        if (_continueKeyboard is not null)
            return _continueKeyboard;

        var backBtn = CreateButtonOptions("Назад", new Command(CommandType.Back, rateValue));
        var continueBtn = CreateButtonOptions("Пропустить", new Command(CommandType.Continue, rateValue));

        _continueKeyboard = new KeyboardBuilder()
            .AddButton(backBtn, KeyboardButtonColor.Default)
            .AddButton(continueBtn, KeyboardButtonColor.Primary)
            .Build();

        return _continueKeyboard;
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
