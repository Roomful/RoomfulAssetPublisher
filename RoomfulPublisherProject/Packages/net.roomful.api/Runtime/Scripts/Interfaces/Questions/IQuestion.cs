using System;
using System.Collections.Generic;
using UnityEngine;

// Copyright Roomful 2013-2020. All rights reserved.

namespace net.roomful.api {
    
    public interface IQuestion
    {
        IQuestionModel Model { get; set; }
        int QuestionId { get; set; }
        string QuestionMessage { get; set; }
        QuizQuestionType QuestionType { get; set; }
        ViewType QuestionViewType { get; set; }
        SourceType SourceType { get; set; }
        List<GameObject> QuestionAnswers { get; set; }
        Action<IAnswerModel> QuestionCallback { get; set; }
        void Init(IQuestionModel question, Action<IAnswerModel> onQuestionFinishedCallback = null, string data = "");
        void OnDestroy();
        void UpdateCountryFlag();
    }
}