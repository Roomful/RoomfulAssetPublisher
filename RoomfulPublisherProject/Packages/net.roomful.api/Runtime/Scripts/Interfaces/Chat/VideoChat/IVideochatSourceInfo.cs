namespace net.roomful.api
{
    /// <summary>
    /// Id of object video chat is related to.
    /// </summary>
    public interface IVideochatSourceInfo
    {
        /// <summary>
        /// Contains room id chat is related to.
        /// If chat wasn't started in the room, returns <see cref="string.Empty"/>
        /// </summary>
        string RoomId { get; }
        
        /// <summary>
        /// Contains prop id chat is related to.
        /// If chat wasn't started in the prop, returns <see cref="string.Empty"/>
        /// </summary>
        string PropId { get; }
    }
}