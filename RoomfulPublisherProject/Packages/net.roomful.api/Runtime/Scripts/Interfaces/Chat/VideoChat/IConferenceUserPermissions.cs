using System.Collections.Generic;

namespace net.roomful.api {
    public interface IConferenceUserPermissions {
        bool IsModerator { get; }
        bool IsPresenter { get;}
        bool IsPromoted { get;}

        Dictionary<string, object> ToDictionary();
    }
}