using UnityEngine.SceneManagement;

internal static class SA_Extensions_Scene
{
    public static T GetComponentInScene<T>(this Scene scene) {
        foreach (var gameObject in scene.GetRootGameObjects()) {
            var component = gameObject.GetComponentInChildren<T>();
            if (component != null) {
                return component;
            }
        }

        return default;
    }
}
