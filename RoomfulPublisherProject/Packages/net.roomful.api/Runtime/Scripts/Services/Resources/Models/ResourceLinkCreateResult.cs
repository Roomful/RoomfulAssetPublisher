using StansAssets.Foundation.Models;

namespace net.roomful.api.resources
{
    public class ResourceLinkCreateResult : Result
    {
        public IResource ResourceLink { get; }

        public ResourceLinkCreateResult(IResource resource) {
            ResourceLink = resource;
        }

        public ResourceLinkCreateResult(Error error) : base(error) { }
    }
}
