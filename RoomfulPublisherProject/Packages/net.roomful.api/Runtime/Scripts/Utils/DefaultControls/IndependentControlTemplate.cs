using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace net.roomful.api
{
    public class IndependentControlTemplate : IControlTemplate
    {
        public string Title { get; }
        
        private readonly Func<GameObject> m_createPrefabDelegate;

        protected GameObject Prefab => m_createPrefabDelegate.Invoke();
        
        public GameObject Create(Transform parent)
        {
            if (Prefab == null) {
                Debug.LogError($"Prefab is null for {Title}!");
                return null;
            }

            var control = Object.Instantiate(Prefab, parent);

            return control;
        }

        public IndependentControlTemplate(string title, Func<GameObject> createPrefabDelegate)
        {
            m_createPrefabDelegate = createPrefabDelegate;
            Title = title;
        }
    }
}
