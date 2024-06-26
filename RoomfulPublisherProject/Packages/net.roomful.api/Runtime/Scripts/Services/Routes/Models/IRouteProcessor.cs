namespace net.roomful.api
{
    /// <summary>
    /// Custom rote processor. You can register one if you need to process url for the custom schemes.
    /// </summary>
    public interface IRouteProcessor
    {
        /// <summary>
        /// Run provided url link.
        /// </summary>
        /// <param name="urlLink"></param>
        void Run(UrlLink urlLink);
    }
}
