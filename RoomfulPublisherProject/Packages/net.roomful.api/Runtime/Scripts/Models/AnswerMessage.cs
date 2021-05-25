// Copyright Roomful 2013-2020. All rights reserved.

namespace net.roomful.api {
    
    public class AnswerMessage : IAnswerMessage {
        
        public int Id { get; set; }
        public string LocalizedMessage { get; set; }
    }
}