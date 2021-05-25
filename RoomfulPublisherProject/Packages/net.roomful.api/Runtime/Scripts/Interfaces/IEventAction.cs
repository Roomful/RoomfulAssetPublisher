// Copyright Roomful 2013-2020. All rights reserved.

namespace net.roomful.api {

    public interface IEventAction {
        
        string Id { get; set; }
        string ParentId { get; set; }
        string ObjectSettingsId { get; set; }
        string GetEventsSelectedItem { get; }
        string GetActionsSelectedItem{ get; }
        ContentType ObjectContentType { get; set; }
    }
}