namespace net.roomful.api
{
    /// <summary>
    /// Location update arguments.
    /// </summary>
    public struct LocationUpdateArgs
    {
        /// <summary>
        /// Current user location.
        /// </summary>
        public IUserLocationInfo Current { get; }

        /// <summary>
        /// Previous user location.
        /// Can be `null` if there is no info about user previous location.
        /// </summary>
        public IUserLocationInfo Previous { get; }

        public LocationUpdateArgs(IUserLocationInfo previous, IUserLocationInfo current) {
            Current = current;
            Previous = previous;
        }
    }
}
