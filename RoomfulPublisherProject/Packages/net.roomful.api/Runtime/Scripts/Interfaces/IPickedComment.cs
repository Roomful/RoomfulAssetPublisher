using System.Collections.Generic;

namespace net.roomful.api {
    
    public interface IPickedComment {
        Dictionary<string, object> ToDictionary();
        string CommentId { get; set; }
        float Length { get; set; }
        IComment Comment { get; set; }
    }
}