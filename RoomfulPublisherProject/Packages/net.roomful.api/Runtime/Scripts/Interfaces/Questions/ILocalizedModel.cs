// Copyright Roomful 2013-2020. All rights reserved.

namespace net.roomful.api {
    
    public interface ILocalizedStringModel {

        int StringId { get; }
        string Local { get; } //actually its locale {EN/RU/etc}
        string StringValue { get; set; } //answer on the question
    }
}
