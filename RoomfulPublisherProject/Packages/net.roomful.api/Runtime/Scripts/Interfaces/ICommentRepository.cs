using System;
using System.Collections.Generic;

namespace net.roomful.api {
    
    public interface ICommentsRepository {
        void LoadTextComments(IResource resource, Action<List<IComment>> callback, int count = 10, int offset = 0);
        void LoadAudioComments(IResource resource, Action<List<IComment>> callback, int count = 10, int offset = 0);
        void LoadPickedComments(IResource resource, Action<List<IComment>> callback, List<IPickedComment> commentIds);
        void Clear();
    }  
}


