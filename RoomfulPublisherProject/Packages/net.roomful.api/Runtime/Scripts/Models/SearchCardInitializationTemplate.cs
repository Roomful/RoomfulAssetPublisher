
// Copyright Roomful 2013-2020. All rights reserved.

namespace net.roomful.api {
    
    public class SearchCardInitializationTemplate : ISearchCardInitializationTemplate {
        public JSONData SearchCardTemplate { get; set; }
        public string SearchEventName { get; set; }
        public JSONData NetworkQuestionaryTemplate { get; set; }
        public string SearchCardUpdatedEventName { get; set; }
    }
}
