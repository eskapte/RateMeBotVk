using VkNet.Model.Keyboard;

namespace MudaBot.Core.Extensions;

public static class MessageKeyboardExtensions
{
    public static MessageKeyboard AsInline(this MessageKeyboard keyboard)
    {
        keyboard.Inline = true;
        return keyboard;
    }

    public static MessageKeyboard AsOneTime(this MessageKeyboard keyboard)
    {
        keyboard.OneTime = true;
        return keyboard;
    }
}
