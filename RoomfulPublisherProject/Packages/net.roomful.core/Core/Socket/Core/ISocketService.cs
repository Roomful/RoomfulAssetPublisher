using System;

namespace net.roomful.api.socket
{
    /// <summary>
    /// Provides API to work with socket connection.
    /// </summary>
    public interface ISocketService
    {
        /// <summary>
        /// Socket connection state.
        /// </summary>
        bool IsConnected { get; }

        /// <summary>
        /// Current Open Socket Id.
        /// </summary>
        string SocketId { get; }

        /// <summary>
        /// Send socket request.
        /// </summary>
        /// <param name="socketRequest">Socket request to send.</param>
        /// <param name="callback">Response callback action.</param>
        /// <typeparam name="T">Type of the response.</typeparam>
        void Send<T>(ISocketRequest<T> socketRequest, Action<T> callback) where T : ISocketRequestCallback, new();

        /// <summary>
        /// Send socket package.
        /// </summary>
        /// <param name="socketPackage">Socket package to send.</param>
        void Send(ISocketPackage socketPackage);

        /// <summary>
        /// Disconnect Socket.
        /// </summary>
        /// <param name="deactivateConnectionWatcher">
        /// If connection watcher is deactivated, there will be no connection attempt.
        /// </param>
        void Disconnect(bool deactivateConnectionWatcher = true);

        /// <summary>
        /// Subscribes to the server side socket event.
        /// </summary>
        /// <param name="onSocketEvent">Action is triggered on socket event.</param>
        /// <typeparam name="T">Type of the event.</typeparam>
        void Subscribe<T>(Action<T> onSocketEvent) where T : ISocketEventListener, new();
    }
}
