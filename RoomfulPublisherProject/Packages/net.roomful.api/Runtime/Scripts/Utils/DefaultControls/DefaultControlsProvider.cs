using System.Collections.Generic;
using UnityEngine;

namespace net.roomful.api
{
    public interface IControlsProvider
    {
        GameObject GetControlPrefab(string key);
    }

    public class DefaultControlsProvider : MonoBehaviour, IControlsProvider
    {
        [SerializeField] private GameObject m_missingControl = default;

        // TODO: Implement proper dictionary serialization
        [SerializeField] private List<string> m_keys = default;
        [SerializeField] private List<GameObject> m_values = default;

        private Dictionary<string, GameObject> m_controls;

        private GameObject MissingControl => m_missingControl;

        private Dictionary<string, GameObject> Controls {
            get {
                if (m_controls == null) {
                    m_controls = new Dictionary<string, GameObject>();
                    for (var i = 0; i < m_keys.Count; ++i) {
                        m_controls.Add(m_keys[i], m_values[i]);
                    }
                }
                return m_controls;
            }
        }

        public GameObject GetControlPrefab(string key) {
            if (Controls.TryGetValue(key, out var prefab)) {
                return prefab;
            }
            return MissingControl;
        }
    }
}

