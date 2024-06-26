namespace net.roomful.api.props
{
    public interface IPropDelegate
    {
        void OnPropAssetLoadStarted();
        void OnPropAssetLoadCompleted();
        void OnPropAssetAndSkinsLoadCompleted();
        void OnPropScaleChanged(float scale);
    }
}
