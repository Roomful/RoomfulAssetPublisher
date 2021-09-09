namespace net.roomful.api.payment
{
    public interface ITransactionActionTemplate
    {
        string Description { get; }

        float Amount { get; }
    }
}
