using System;

namespace net.roomful.api.assets
{
    /// <summary>
    /// Assets loading queue.
    /// </summary>
    public interface IAssetsQueue
    {
        /// <summary>
        /// Number of props waiting in queue to be loaded.
        /// </summary>
		int PropsInQueue {get;}

        /// <summary>
        /// Returns `true` if queue is currently working. Otherwise `false`.
        /// </summary>
        bool IsBusy {get;}

        /// <summary>
        /// An event is fired when all assets in queue were loaded.
        /// Can be fired multiple times while in the room.
        /// For example:
        /// * Room opened.
        /// * All assets are loaded.
        /// * OnComplete is fired.
        /// * Pause....
        /// * More assets are added (new styles / props created, etc.).
        /// * All assets are loaded.
        /// * OnComplete is fired.
        ///
        /// If you need to know that room initial assist are fired.
        /// * Subscribe to room load.
        /// * Subscribe to this <see cref="OnComplete"/> event.
        /// * Unsubscribe ones  <see cref="OnComplete"/> or Room unloaded event is fired.
        /// </summary>
        event Action OnComplete;
    }
}
