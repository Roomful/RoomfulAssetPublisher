namespace net.roomful.api.ui
{
    /// <summary>
    /// Generic panel interface with minimum freedom to affect it.
    /// </summary>
    public interface IPanelView
    {
        /// <summary>
        /// Show panel.
        /// </summary>
        void Show();

        /// <summary>
        /// Hide panel.
        /// </summary>
        void Hide();
    }
}
