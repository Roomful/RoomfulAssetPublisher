using System.Collections.Generic;
using net.roomful.api.assets;

namespace net.roomful.api {

    public interface IPropEventsActionsModel : ITemplate {

        Dictionary<string, object> ToDictionary();
        string ObjectSettingsId { get; }
        PropEventTrigger EnumEventName { get; set; }
        PropAction EnumActionName { get; set; }
        ContentType ObjectContentType { get; }
        //Data fields
        string UsedPropId { get; set; }
        string UsedRoomId { get; set; }
        string UsedStorylineId { get; set; }
        string UsedSegmentId { get; set; }
        void Clear();
    }
}
