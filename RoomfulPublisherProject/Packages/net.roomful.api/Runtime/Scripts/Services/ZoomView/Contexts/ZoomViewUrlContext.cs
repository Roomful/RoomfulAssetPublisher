namespace net.roomful.api.zoom
{
    public struct ZoomViewUrlContext
    {
        /// <summary>
        /// Prop Id for zoom view.
        /// </summary>
        public string PropId;

        /// <summary>
        /// Room Id where prop is located.
        /// </summary>
        public string RoomId;

        /// <summary>
        /// Prop Resourced Id Zoom view will focus on.
        /// </summary>
        public string ResourceId;

        /// <summary>
        /// Child Prop od Zoom view will focus on.
        /// </summary>
        public string InnerPropId;

        /// <summary>
        /// Focus Pointer Index Zoom view will focus on.
        /// </summary>
        public int FocusPointIndex;

        /// <summary>
        /// Create Default url context for a prop.
        /// </summary>
        /// <param name="roomId">Room Id where prop is located.</param>
        /// <param name="propId">The target prop id.</param>
        public ZoomViewUrlContext(string roomId, string propId) {
            PropId = propId;
            RoomId = roomId;
            ResourceId = string.Empty;
            InnerPropId = string.Empty;
            FocusPointIndex = ZoomViewContext.UNDEFINED_INDEX;
        }
    }
}
