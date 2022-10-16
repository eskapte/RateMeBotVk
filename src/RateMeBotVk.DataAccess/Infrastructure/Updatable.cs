using System;

namespace RateMeBotVk.DataAccess.Infrastructure;

public abstract class Updatable
{
    public abstract DateTime Updated { get; set; }

    public void Update()
    {
        Updated = DateTime.Now;
    }
}
