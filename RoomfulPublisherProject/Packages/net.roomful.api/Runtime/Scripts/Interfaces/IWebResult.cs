using UnityEngine;

// Copyright Roomful 2013-2020. All rights reserved.

namespace net.roomful.api {
    
    public interface IWebResult {

        int StatusCode { get; }
        string DataAsText { get; }
        string Message { get; }
        bool IsSuccess { get; }
        byte[] DataAsByteArray { get; }
        Texture2D DataAsTexture { get; }
    }
}