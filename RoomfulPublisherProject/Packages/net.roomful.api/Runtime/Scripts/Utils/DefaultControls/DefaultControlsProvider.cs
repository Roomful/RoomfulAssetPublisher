using System.Collections.Generic;
using UnityEngine;

namespace net.roomful.api
{
    public interface IControlsProvider
    {
        GameObject GetControlPrefab(string key);
        void RegisterControlPrefab(string name, GameObject gameObject);
    }

    public class DefaultControlsProvider : MonoBehaviour, IControlsProvider
    {
        [SerializeField] private GameObject m_missingControl = default;

        // TODO: Implement proper dictionary serialization
        [SerializeField] private List<string> m_keys;
        [SerializeField] private List<GameObject> m_values;
        
        private GameObject MissingControl => m_missingControl;
        

        public GameObject GetControlPrefab(string key) {

            var controlIndex = m_keys.IndexOf(key);
            if (controlIndex < 0) {
                return MissingControl;
            } 
            
            return m_values[controlIndex];
        }

        public void RegisterControlPrefab(string controlName, GameObject controlGameObject) {
            m_keys.Add(controlName);
            m_values.Add(controlGameObject);
        }
    }
}

