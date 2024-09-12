using System.Collections.Generic;
using net.roomful.assets.serialization;
using UnityEngine;

namespace net.roomful.assets
{
    public class SceneStyle : MonoBehaviour
    {
        [SerializeField] List<Material> m_Materials = new List<Material>();
        [SerializeField] List<string> m_MaterialShaders = new List<string>();
        [SerializeField] List<string> m_MaterialKeywords = new List<string>();
        
        public IReadOnlyList<Material> Materials => m_Materials;
        public IReadOnlyList<string> MaterialShaders => m_MaterialShaders;
        public IReadOnlyList<string> MaterialKeywords => m_MaterialKeywords;
        
        [SerializeField] List<SerializedReflectiveFloor> m_ReflectiveFloors = new List<SerializedReflectiveFloor>();
        public  List<SerializedReflectiveFloor> ReflectiveFloors => m_ReflectiveFloors;
        

        internal void Clear()
        {
            m_Materials.Clear();
            m_MaterialShaders.Clear();
            m_MaterialKeywords.Clear();
            m_ReflectiveFloors.Clear();
        }
        
        internal void AddMaterial(Material material)
        {
            if (!m_Materials.Contains(material))
            {
                m_Materials.Add(material);
                m_MaterialShaders.Add(material.shader.name);
                m_MaterialKeywords.Add(string.Join("_", material.shaderKeywords));
            }
        }

        internal void SetReflectiveFloors(List<SerializedReflectiveFloor> floors)
        {
            m_ReflectiveFloors = floors;
        }
    }
}
