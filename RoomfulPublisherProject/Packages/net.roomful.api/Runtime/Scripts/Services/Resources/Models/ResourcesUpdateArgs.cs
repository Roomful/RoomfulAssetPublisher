namespace net.roomful.api.resources
{
    /// <summary>
    /// Resource Update type.
    /// </summary>
    public struct ResourcesUpdateArgs
    {
        /// <summary>
        /// Resource update Type.
        /// </summary>
        public ResourcesUpdateType UpdateType { get; }

        /// <summary>
        /// Target Resource.
        /// </summary>
        public IResource Resource { get; }

        public ResourcesUpdateArgs(ResourcesUpdateType updateType, IResource resource) {
            UpdateType = updateType;
            Resource = resource;
        }
    }
}
