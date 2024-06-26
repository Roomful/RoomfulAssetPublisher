namespace net.roomful.api.ui
{
    /// <summary>
    /// UI element view.
    /// </summary>
    public interface IElementView
    {
        /// <summary>
        /// Use to hide or show button view.
        /// </summary>
        /// <param name="value">Set `true` to show button view and `false` otherwise.</param>
        void SetActive(bool value);

        /// <summary>
        /// Returns 'true' if element is active or `false` otherwise.
        /// </summary>
        bool IsActive { get; }
    }
}
