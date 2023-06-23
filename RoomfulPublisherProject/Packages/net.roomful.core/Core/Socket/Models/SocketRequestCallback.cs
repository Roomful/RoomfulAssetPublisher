namespace net.roomful.api.socket
{
    /// <summary>
    /// Default Request callback model.
    /// </summary>
    public class SocketRequestCallback : ISocketRequestCallback
    {
        public JSONData JSON { get; private set; }
        public bool IsSuccess { get; private set; }
        public SocketError Error { get; private set; }

        public bool HasError => Error != null;
        public virtual bool DisplayErrorPopup => true;

        public virtual void HandleData(JSONData data) {
            IsSuccess = true;
            JSON = data;
        }

        public virtual void HandleError(SocketError error) {
            Error = error;
            IsSuccess = false;
        }
    }
}
