// Copyright Roomful 2013-2020. All rights reserved.

using System.Collections.Generic;

namespace net.roomful.api {

    public interface IPoll {
        string QuestionMessage { get; }
        List<IPollAnswer> PollAnswers { get; }
    }
}
