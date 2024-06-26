namespace net.roomful.api.media
{
    public interface IMediaService
    {
        void AddDelegate(IMediaSourceDelegate @delegate);
        void RemoveDelegate(IMediaSourceDelegate @delegate);
        
        void ActivateSource(IMediaSource source);
        void DeactivateSource(IMediaSource source);
    }
    
    
}
