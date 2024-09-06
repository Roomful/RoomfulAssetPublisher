using System.Collections.Generic;
using UnityEngine;

namespace net.roomful.assets
{
    public class SceneStyle : MonoBehaviour
    {
        [SerializeField] List<Material> m_Materials = new List<Material>();
        public IReadOnlyList<Material> Materials => m_Materials;
        

        internal void Clear()
        {
            m_Materials.Clear();
        }
        
        internal void AddMaterial(Material material)
        {
            if (!m_Materials.Contains(material))
            {
                m_Materials.Add(material);
            }
        }
    }
}
