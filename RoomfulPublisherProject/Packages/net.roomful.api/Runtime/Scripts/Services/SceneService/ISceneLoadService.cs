using System;

namespace net.roomful.api.scenes
{
    /// <summary>
    /// Service is used to load additive scenes in Roomful.
    /// </summary>
    public interface ISceneService
    {
        /// <summary>
        /// Loads additive scene by it's name.
        /// You should also specify scene controller that is located on one of the scene root objects.
        /// </summary>
        /// <param name="sceneName">The name of the scene you would like to load.</param>
        /// <param name="callback">Scene load callback</param>
        /// <typeparam name="T">The type of the scene controller that is located on one of the scene root objects.</typeparam>
        void Load<T>(string sceneName, Action<T> callback) where T : class;

        /// <summary>
        /// Loads additive scene by it's name.
        /// You should also specify scene controller that is located on one of the scene root objects.
        /// </summary>
        /// <param name="sceneName">The name of the scene you would like to load.</param>
        /// <param name="callback">Scene load callback</param>
        /// <typeparam name="T">The type of the scene controller that is located on one of the scene root objects.</typeparam>
        void LoadWithPreloader<T>(string sceneName, Action<T> callback) where T : class;
    }
}
