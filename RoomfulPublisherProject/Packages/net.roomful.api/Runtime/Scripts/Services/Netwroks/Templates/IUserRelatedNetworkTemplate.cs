namespace net.roomful.api.networks
{
    public interface IUserRelatedNetworkTemplate : ITemplate
    {
        INetworkTemplate Network { get; }

        bool IsSubscribed { get; }

        INetworkSubscriptionTemplate SubscribtionOption { get; }
    }
}
