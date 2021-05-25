using System.Collections.Generic;

namespace net.roomful.api {

    public interface IPropEventsActionsModel : ITemplate {

        Dictionary<string, object> ToDictionary();
        string ObjectSettingsId { get; }
        PropEventTrigger EnumEventName { get; set; }
        PropAction EnumActionName { get; set; }
        ContentType ObjectContentType { get; }
        List<IPropEventsActionsModel> ChildActions { get; }
        //Data fields
        string UsedPropId { get; set; }
        string UsedRoomId { get; set; }
        string UsedNetworkId { get; }
        string UsedStorylineId { get; set; }
        int UsedQuestionId { get; set; }
        int UsedQuizId { get; }
        string UsedSegmentId { get; set; }
        Dictionary<string, object> AnimationData { get; }
        void Clear();
    }
}
