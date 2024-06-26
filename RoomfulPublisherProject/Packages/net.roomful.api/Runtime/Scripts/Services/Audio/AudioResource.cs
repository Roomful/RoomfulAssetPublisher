
namespace net.roomful.api.audio
{
    public class AudioResource
    {
        public IResource Resource { get; }
        public AudioMetadata AudioMetadata { get; }

        public string Id => Resource.Id;

        public AudioResource(IResource resource, AudioMetadata audioMetadata) {
            Resource = resource;
            AudioMetadata = audioMetadata;
        }
    }
}
