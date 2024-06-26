namespace net.roomful.api.networks
{
    public interface INetworkSubscriptionTemplate
    {
        NetworkSubscribtionType SubscribtionType { get; }

        int TokenActionCost { get; }

        string LocalizedPrice { get; }
    }
}
