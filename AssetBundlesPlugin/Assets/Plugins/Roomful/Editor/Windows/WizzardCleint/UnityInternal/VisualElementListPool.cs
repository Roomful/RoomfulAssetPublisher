using UnityEngine;
using UnityEditor;
using UnityEngine.Experimental.UIElements;
using System.Collections.Generic;

namespace RF.AssetWizzard.Editor
{
    internal class VisualElementListPool
    {
        static ObjectPool<List<VisualElement>> pool = new ObjectPool<List<VisualElement>>(20);

        public static List<VisualElement> Copy(List<VisualElement> elements) {
            var result = pool.Get();

            result.AddRange(elements);

            return result;
        }

        public static List<VisualElement> Get(int initialCapacity = 0) {
            List<VisualElement> result = pool.Get();

            if (initialCapacity > 0 && result.Capacity < initialCapacity) {
                result.Capacity = initialCapacity;
            }
            return result;
        }

        public static void Release(List<VisualElement> elements) {
            elements.Clear();
            pool.Release(elements);
        }
    }
}