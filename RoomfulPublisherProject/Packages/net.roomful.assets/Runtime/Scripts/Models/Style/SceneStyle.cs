using System.Collections.Generic;
using net.roomful.assets.serialization;
using UnityEngine;

namespace net.roomful.assets
{
    public class SceneStyle : MonoBehaviour
    {
        [SerializeField] List<Material> m_Materials = new List<Material>();
        [SerializeField] List<string> m_MaterialShaders = new List<string>();
        [SerializeField] List<SerializedReflectiveFloor> m_ReflectiveFloors = new List<SerializedReflectiveFloor>();

        
        public IReadOnlyList<Material> Materials => m_Materials;
        public IReadOnlyList<string> MaterialShaders => m_MaterialShaders;
        public  List<SerializedReflectiveFloor> ReflectiveFloors => m_ReflectiveFloors;
        

        internal void Clear()
        {
            m_Materials.Clear();
            m_MaterialShaders.Clear();
            m_ReflectiveFloors.Clear();
        }
        
        internal void AddMaterial(Material material)
        {
            if (!m_Materials.Contains(material))
            {
                m_Materials.Add(material);
                m_MaterialShaders.Add(material.shader.name);
            }
        }

        internal void SetReflectiveFloors(List<SerializedReflectiveFloor> floors)
        {
            m_ReflectiveFloors = floors;
        }
    }
}
