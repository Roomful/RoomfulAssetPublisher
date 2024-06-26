using net.roomful.api.appMenu;

namespace net.roomful.api.profile
{
    public struct ProfileViewInitContext
    {
        public ICustomizableView SidePanel;
    }

    public struct ProfileViewContext
    {
        public string UserId;
    }

    public interface IProfileViewController
    {
        void Init(ProfileViewInitContext context);
        void ActivateWithContext(ProfileViewContext context);
        void Deactivate();
    }
}