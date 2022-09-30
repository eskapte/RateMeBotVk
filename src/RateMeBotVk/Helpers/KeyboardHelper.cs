using Newtonsoft.Json;
using RateMeBotVk.BotCommandExecuter.Commands;
using VkNet.Enums.SafetyEnums;
using VkNet.Model.Keyboard;

namespace RateMeBotVk.Helpers;
public static class KeyboardHelper
{
    private static KeyboardBuilder _builder = new KeyboardBuilder();
    private static MessageKeyboard _mainKeyboard;

    public static MessageKeyboard GetMain()
    {
        if (_mainKeyboard is not null)
            return _mainKeyboard;

        var settingsBtn = CreateButtonOptions("Настройки", new Command(CommandType.Settings));
        var aboutMeBtn = CreateButtonOptions("Обо мне", new Command(CommandType.AboutMe));

        _mainKeyboard = _builder
            .AddButton(settingsBtn, KeyboardButtonColor.Default)
            .AddButton(aboutMeBtn, KeyboardButtonColor.Positive)
            .Build();

        return _mainKeyboard;
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
