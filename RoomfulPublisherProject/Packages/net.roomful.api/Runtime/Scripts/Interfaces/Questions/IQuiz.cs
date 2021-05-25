using System;
using System.Collections.Generic;

// Copyright Roomful 2013-2020. All rights reserved.

namespace net.roomful.api {
    
    public interface IQuiz : IQuestionnaire {
        
        List<IQuestionModel> QuizQuestions { get; set; }
        Action<IAnswerModel> OnQuizFinishedCallback { get; set; }
    }
}