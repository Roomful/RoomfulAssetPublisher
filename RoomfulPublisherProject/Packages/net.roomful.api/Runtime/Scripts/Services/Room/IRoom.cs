namespace net.roomful.api.room
{
    /// <summary>
    /// Model allow to read an manipulate the room.
    /// </summary>
    public interface IRoom
    {
        IRoomTemplate Template { get; }
        bool UserHasEditPermissions { get; }

        void SetParam(string paramKey, object paramValue);
        T GetParam<T>(string paramKey);
    }
}
