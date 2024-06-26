using System;

namespace net.roomful.api.appMenu
{
    public interface ISideMenuDelegate
    {
        event Action OnClosed;
    }
}