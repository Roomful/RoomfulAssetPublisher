using System.Collections.Generic;

// Copyright Roomful 2013-2020. All rights reserved.

namespace net.roomful.api {
    
    public interface IQuestionModel : IQuestionnaire {

        int QuestionOwnerId { get; set; }
        QuizQuestionType QuestionType { get; }
        ViewType QuestionViewType { get; }
        IMultiChoiceScale Scale { get; set; }
        SourceType SourceType { get; set; }
        string SourceId { get; set; }
        string AnswerAlignType { get; set; }
        //previous question data
        IAnswerModel QuestionTakedAnswer { get; set; }
        bool WithPrevAnswer { get; set; }
        Dictionary<string, object> ToDictionary();
    }
}