namespace net.roomful.api.props
{
    /// <summary>
    /// Allows to override application prop click behaviour
    /// </summary>
    public interface IPropsInput
    {
        void RegisterShortClickHandler(IPropShortClickHandler handler);
        void UnregisterShortClickHandler(IPropShortClickHandler handler);

        void OverridePropLongClick(IPropLongClickHandler handler);
        void RemovePropLongClickOverride(IPropLongClickHandler handler);
    }
}
