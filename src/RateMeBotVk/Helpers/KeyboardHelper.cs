using Newtonsoft.Json;
using RateMeBotVk.BotCommandExecuter;
using VkNet.Enums.SafetyEnums;
using VkNet.Model.Keyboard;

namespace RateMeBotVk.Helpers;
public static class KeyboardHelper
{
    private static MessageKeyboard _mainKeyboard;
    private static MessageKeyboard _settingKeyboard;

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

    public static MessageKeyboard GetSettings()
    {
        if (_settingKeyboard is not null)
            return _settingKeyboard;

        var unsubscribeBtn = CreateButtonOptions("Отписаться", new Command(CommandType.UnsubcribeOnUpdates));
        var backBtn = CreateButtonOptions("Назад", new Command(CommandType.Back));

        _settingKeyboard = new KeyboardBuilder()
            .AddButton(unsubscribeBtn, KeyboardButtonColor.Negative)
            .AddLine()
            .AddButton(backBtn, KeyboardButtonColor.Default)
            .Build();

        return _settingKeyboard;
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
