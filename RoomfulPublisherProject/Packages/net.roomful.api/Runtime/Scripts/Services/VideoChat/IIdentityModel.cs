
// Copyright Roomful 2013-2020. All rights reserved.

namespace net.roomful.api {
    public interface IIdentityModel {
        string Identity { get; }
        int Uid { get; }
        int MicrophoneStatus { get; }
    }
}