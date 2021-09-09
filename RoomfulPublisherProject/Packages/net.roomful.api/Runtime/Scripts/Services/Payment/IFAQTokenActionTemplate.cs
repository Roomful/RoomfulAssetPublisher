namespace net.roomful.api.payment
{
    public interface IFAQTokenActionTemplate
    {
        float Price { get; }
        string Category { get; }

        string Action { get; }

        float Token { get; }
        TransactionFilterType Type { get; }
    }
}
