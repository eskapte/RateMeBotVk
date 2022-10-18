using System;

namespace MudaBot.DataAccess.Infrastructure;

public abstract class TimeAccounting
{
    public abstract DateTime Created { get; set; }
    public abstract DateTime Updated { get; set; }

    public void OnCreate() => Created = DateTime.Now;

    public void OnUpdate() => Updated = DateTime.Now;
}
