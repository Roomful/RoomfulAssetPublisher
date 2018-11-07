using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



namespace SA.Productivity.SceneValidator
{
    public class SV_SceneComponentsCache
    {


        private Dictionary<Type, List<Component>> m_componentsCache = new Dictionary<Type, List<Component>>();

     

        public void CacheComponent(Type type, Component component) {
            
            List<Component> componentsCache;
            if (m_componentsCache.ContainsKey(type)) {
                componentsCache = m_componentsCache[type];
            } else {
                componentsCache = new List<Component>();
                m_componentsCache.Add(type, componentsCache);
            }

            if (!componentsCache.Contains(component)) {
                componentsCache.Add(component);
            }

        }


        public List<Component> GetComponents(Type type) {
            if(m_componentsCache.ContainsKey(type)) {
                return m_componentsCache[type];
            } else {
                return null;
            }
        }


        public void CleanUp() {
            foreach (var typePair in m_componentsCache) {
                List<Component> components = typePair.Value;
                components.RemoveAll(item => item == null);
            }
        }

    }
}