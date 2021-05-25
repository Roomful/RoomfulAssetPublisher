// Copyright Roomful 2013-2020. All rights reserved.

namespace net.roomful.api {

    public interface IPollAnswer {
        
        string LocalizedMessage { get; }
        int ResponsesCount { get; set; }
    }
}