using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace net.roomful.api
{
    public class IndependentControlTemplate : IControlTemplate
    {
        public string Title { get; }
        
        private readonly Func<GameObject> m_createPrefabDelegate;
        private GameObject m_control;

        protected GameObject Prefab => m_createPrefabDelegate.Invoke();
        
        public GameObject Create(Transform parent)
        {
            if (Prefab == null) {
                Debug.LogError($"Prefab is null for {Title}!");
                return null;
            }

            m_control = Object.Instantiate(Prefab, parent, false);
            //control.transform.localScale = Vector3.one;

            return m_control;
        }

        public void HideControl() {
            m_control.SetActive(false);
        }

        public void DisableInteraction() {
            throw new NotImplementedException();
        }

        public IndependentControlTemplate(string title, Func<GameObject> createPrefabDelegate)
        {
            m_createPrefabDelegate = createPrefabDelegate;
            Title = title;
        }
    }
}
