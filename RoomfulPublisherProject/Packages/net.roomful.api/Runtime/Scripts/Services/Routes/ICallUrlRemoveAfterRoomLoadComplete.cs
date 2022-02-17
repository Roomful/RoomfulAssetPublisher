namespace net.roomful.api
{
    /// <summary>
    /// If <see cref="IRouteHandler"/> is inherited from this interface,
    /// and <see cref="IRouteHandler.OnUrlKeysRemoved"/> should be called for this handler,
    /// it will be called when  new route processing si completed, as opposite to the default behaviour,
    /// where it will be called in the beginning of processing a new route.
    /// </summary>
    public interface ICallUrlRemoveAfterRoomLoadComplete
    {

    }
}
