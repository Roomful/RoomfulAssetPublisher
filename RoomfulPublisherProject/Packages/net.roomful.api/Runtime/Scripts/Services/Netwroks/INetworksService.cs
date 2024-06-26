using System;
using net.roomful.api.socket;

namespace net.roomful.api.networks
{
    public interface INetworksService
    {
        event Action<INetworkTemplate> OnNetworkSwitchRequest;
        INetworkTemplate Network { get; }
        void SwitchNetwork(INetworkTemplate network);
        void SubscribeToNetwork(INetworkTemplate network, Action<SocketRequestCallback> callback);
        void JoinNetwork(IUserRelatedNetworkTemplate tpl, Action<NetworkJointResult> callback);
    }
}
