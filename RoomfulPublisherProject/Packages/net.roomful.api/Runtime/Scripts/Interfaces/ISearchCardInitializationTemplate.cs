
// Copyright Roomful 2013-2020. All rights reserved.

namespace net.roomful.api {

    public interface ISearchCardInitializationTemplate {
        JSONData SearchCardTemplate { get; set; }
        string SearchEventName { get; set; }
        JSONData NetworkQuestionaryTemplate { get; set; }
        string SearchCardUpdatedEventName { get; set; }
    }
}
