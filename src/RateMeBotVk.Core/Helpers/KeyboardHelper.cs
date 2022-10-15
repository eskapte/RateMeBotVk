using Newtonsoft.Json;
using RateMeBotVk.Core.Models;
using VkNet.Enums.SafetyEnums;
using VkNet.Model.Keyboard;

namespace RateMeBotVk.Helpers;
public static class KeyboardHelper
{
    private static MessageKeyboard? _mainKeyboard;
    private static MessageKeyboard? _rateKeyboard;

    public static MessageKeyboard GetMain()
    {
        if (_mainKeyboard is not null)
            return _mainKeyboard;

        var aboutMeBtn = CreateButtonOptions("Мой профиль", new Payload(CommandType.AboutMe));

        _mainKeyboard = new KeyboardBuilder()
            .AddButton(aboutMeBtn, KeyboardButtonColor.Positive)
            .Build();

        return _mainKeyboard;
    }

    public static MessageKeyboard GetRateKeyboard()
    {
        if (_rateKeyboard is not null)
            return _rateKeyboard;

        var rateBtn = CreateButtonOptions("Оценить", new Payload(CommandType.Rate));

        _rateKeyboard = new KeyboardBuilder()
            .AddButton(rateBtn, KeyboardButtonColor.Primary)
            .Build();

        return _rateKeyboard;
    }


    private static MessageKeyboardButtonAction CreateButtonOptions(string text, Payload command)
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
