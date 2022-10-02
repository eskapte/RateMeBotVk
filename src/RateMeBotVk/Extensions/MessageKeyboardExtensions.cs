using VkNet.Model.Keyboard;

namespace RateMeBotVk.Extensions;

public static class MessageKeyboardExtensions
{
    public static MessageKeyboard AsInline(this MessageKeyboard keyboard)
    {
        keyboard.Inline = true;
        return keyboard;
    }
}
