
// Copyright Roomful 2013-2020. All rights reserved.

namespace net.roomful.api {

    public interface ICommentTemplate {
        
        void Setup(IComment comment);
        void Hide();
        void Reset();
    }
}
