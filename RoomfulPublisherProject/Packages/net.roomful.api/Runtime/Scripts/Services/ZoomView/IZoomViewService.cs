namespace net.roomful.api.zoom
{
    public interface IZoomViewService
    {
        void RegisterNextPreviousOverride(NextPreviousBehaviourOverrideContext context);
        void UnRegisterNextPreviousOverride(NextPreviousBehaviourOverrideContext context);
    }
}
