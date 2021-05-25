using System.Collections.Generic;

// Copyright Roomful 2013-2020. All rights reserved.

namespace net.roomful.api {

    public interface IAnswerModel {

        int QuestionId { get; }
        IValueModel AnswerValue { get; }
        Dictionary<string, object> ToDictionary();
    }
}