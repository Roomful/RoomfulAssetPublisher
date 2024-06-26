namespace net.roomful.api.settings
{
    public interface IResourcesSettingsUIService : IExtendableSettings
    {
        void Open(IResource resource, string tabName);
        void Open(IResource resource);
        
        IResource CurrentResource { get; }
        bool IsOpen { get; }
    }
}
