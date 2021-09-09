using System;

namespace net.roomful.api.networks
{
    public interface INetworksService
    {
        event Action<INetworkTemplate> OnNetworkSwitchRequest;

        INetworkTemplate Network { get; }
        void SwitchNetwork(INetworkTemplate network);
        void JoinNetwork(IUserRelatedNetworkTemplate tpl, Action<NetworkJointResult> callback);
    }
}
