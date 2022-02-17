using System.Collections.Generic;

namespace net.roomful.api.story
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
