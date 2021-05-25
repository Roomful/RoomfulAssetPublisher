using System;
using UnityEngine.UI;

// Copyright Roomful 2013-2020. All rights reserved.

namespace net.roomful.api {
    
    public interface IAnswer {
        
        IAnswerModel Model { get; set; }
        string AnswerValue { get; set; }
        RawImage AnswerImage { get; set; }
        Action<bool> Callback { get; set; }
        void Init(IAnswerModel value, ToggleGroup group, Action<IAnswerModel> onQuestionFinished = null);
        void OnDestroy();
    }
}