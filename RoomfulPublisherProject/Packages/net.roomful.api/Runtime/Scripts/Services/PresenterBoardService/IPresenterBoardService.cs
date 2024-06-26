using UnityEngine;

namespace net.roomful.api.presentation.board
{
    /// <summary>
    /// Service controls presentation boards.
    /// </summary>
    public interface IPresenterBoardService
    {
        /// <summary>
        /// Register prop that contains presentation board.
        /// </summary>
        /// <param name="gameObject">Gameobject inside the prop object.</param>
        void Register(GameObject gameObject);

        /// <summary>
        /// Unregisters prop that contains presentation board.
        /// </summary>
        /// <param name="gameObject">Gameobject inside the prop object.</param>
        void Unregister(GameObject gameObject);
    }
}
