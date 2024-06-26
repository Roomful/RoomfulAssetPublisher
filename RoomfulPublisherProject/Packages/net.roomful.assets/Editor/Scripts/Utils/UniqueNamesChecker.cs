using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace net.roomful.assets.editor
{
    internal static class UniqueNamesChecker
    {
        public static bool SceneObjectsNamesAreUnique() {
            var unique = true;
            var objNames = new Dictionary<string, GameObject>();
            var objectsToPing = new List<Object>();
            foreach (var renderer in Object.FindObjectsOfType<Renderer>()) {
                if (!objNames.ContainsKey(renderer.name))
                    objNames.Add(renderer.name, renderer.gameObject);
                else if (renderer.transform.parent.GetComponent<PropThumbnail>() == null && renderer.transform.parent.GetComponent<PropMeshThumbnail>()) {
                    objNames.TryGetValue(renderer.name, out var earlierAddedObj);
                    if (!objectsToPing.Contains(earlierAddedObj))
                        objectsToPing.Add(earlierAddedObj);
                    objectsToPing.Add(renderer.gameObject);
                    unique = false;
                }
            }

            Selection.objects = objectsToPing.ToArray();

            if (!unique) {
                EditorUtility.DisplayDialog("Error!",
                    "It is not allowed to have multiple GameObjects with the same name.\n" +
                    "Please take a look at highlighted GameObjects in the Hierarchy tab,\n" +
                    "and name them (or in some cases their parents) accordingly",
                    "Okay");
            }
            
            return unique;
        }
    }
}