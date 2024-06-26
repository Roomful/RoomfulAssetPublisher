namespace net.roomful.api.media
{
    public interface IMediaSourceDelegate
    {
        string Id { get; }
        
        void OnSourceActivated(IMediaSource source);
        void OnSourceDeactivated(IMediaSource source);
    }

}
