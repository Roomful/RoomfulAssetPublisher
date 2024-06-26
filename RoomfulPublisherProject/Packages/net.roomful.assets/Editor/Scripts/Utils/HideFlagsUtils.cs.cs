using UnityEngine;
using UnityEditor;

public static class HideFlagsUtils
{
    [MenuItem("Roomful/Assets Publisher/Utils/Show Hidden Scene Objects")]
    private static void ShowThemAll()
    {
        var allGameObjects = Object.FindObjectsOfType<GameObject>();
        foreach (var go in allGameObjects)
        {
            switch (go.hideFlags)
            {
                case HideFlags.HideAndDontSave:
                    go.hideFlags = HideFlags.DontSave;
                    break;
                case HideFlags.HideInHierarchy:
                case HideFlags.HideInInspector:
                    go.hideFlags = HideFlags.None;
                    break;
            }
        }
    }
}