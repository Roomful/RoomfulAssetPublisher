// Copyright Roomful 2013-2020. All rights reserved.

namespace net.roomful.api {

    public interface IQuestionnaire {

        int Id { get; set; }
        string Tag { get; set; }
        string LocalizedMessage { get; set; }
    }
}
