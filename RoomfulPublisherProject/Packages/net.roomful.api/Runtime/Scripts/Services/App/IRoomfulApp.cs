namespace net.roomful.api.app
{
    public interface IRoomfulApp
    {
        string AppStartRoute { get; set; }
        string LaunchUrl { get; set; }
        bool LaunchUrlProcessed { get; set; }
    }
}