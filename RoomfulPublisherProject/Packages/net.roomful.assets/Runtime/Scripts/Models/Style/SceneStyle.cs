using System.Collections.Generic;
using UnityEngine;

namespace net.roomful.assets
{
    public class SceneStyle : MonoBehaviour
    {
        [SerializeField] List<Material> m_Materials = new List<Material>();
        [SerializeField] Transform m_Graphics;

        public GameObject Graphics => m_Graphics.gameObject;
        
        public IReadOnlyList<Material> Materials => m_Materials;
        

        internal void Clear()
        {
            m_Materials.Clear();
        }

        internal void SetGraphics(Transform graphics)
        {
            m_Graphics = graphics;
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
