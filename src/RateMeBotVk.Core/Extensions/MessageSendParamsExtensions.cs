using VkNet.Model.Attachments;
using VkNet.Model.RequestParams;
using System;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Linq;

namespace RateMeBotVk.Extensions;

public static class MessageSendParamsExtensions
{
    public static MessagesSendParams ToPeer(this MessagesSendParams message, long peerId)
    {
        message.PeerId = peerId;
        return message;
    }

    public static MessagesSendParams AddPhoto(this MessagesSendParams message, Uri url)
    {
        var photo = new Photo
        {
            Url = url,
            BigPhotoSrc = url,
            OwnerId = message.PeerId.Value,
        };

        var list = new List<MediaAttachment>(message.Attachments ?? new List<MediaAttachment>())
        {
            photo
        };

        message.Attachments = list;

        return message;
    }
}
