namespace net.roomful.api.props
{
    /// <summary>
    /// Allows to override application prop click behaviour
    /// </summary>
    public interface IPropsInput
    {
        void OverridePropShortClick(IPropShortClickDelegate @delegate);
        void RemovePropShortClickOverride(IPropShortClickDelegate @delegate);

        void OverridePropLongClick(IPropLongClickDelegate @delegate);
        void RemovePropLongClickOverride(IPropLongClickDelegate @delegate);
    }
}
