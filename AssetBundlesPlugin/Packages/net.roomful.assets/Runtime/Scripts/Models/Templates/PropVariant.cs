using System.Collections.Generic;
using UnityEngine;

namespace net.roomful.assets
{
    public class PropVariant
    {
        public string Name { get; private set; }

        public readonly List<Renderer> Renderers;

        readonly List<Material> m_Materials;
        public readonly Dictionary<Renderer, List<Material>> MaterialDictionary;

        public IEnumerable<Material> Materials => m_Materials;

        readonly List<Skin> m_Skins = new List<Skin>();

        public PropVariant(string name, IEnumerable<Renderer> renderers)
        {
            Name = name;
            Renderers = new List<Renderer>(renderers);
            MaterialDictionary = new Dictionary<Renderer, List<Material>>();

            m_Materials = new List<Material>();
            foreach (var renderer in Renderers)
            {
                MaterialDictionary.Add(renderer, new List<Material>());
                foreach(var material in renderer.sharedMaterials)
                {
                    m_Materials.Add(material);
                    MaterialDictionary[renderer].Add(material);
                }          
            }
        }

        public void AddSkin(Skin skin)
        {
            m_Skins.Add(skin);
        }

        public void RemoveSkin(Skin skin)
        {
            m_Skins.Remove(skin);
        }

        public void ApplySkin(Skin skin)
        {
            foreach(var render in Renderers)
            {
                for(var i = 0; i < MaterialDictionary[render].Count; i++)
                {
                    render.sharedMaterials[i] = skin.MaterialDictionary[render][i];
                }
            }
        }

        public static IEnumerable<string> GetHierarchyForObject(GameObject go)
        {
            var hierarchy = new List<string>();

            var t = go.transform.parent;
            while (t != null)
            {
                hierarchy.Add(t.name);
                t = t.parent;
            }

            hierarchy.Reverse();
            return hierarchy;
        }

        public IEnumerable<Skin> Skins => m_Skins;

        public Skin DefaultSkin => m_Skins[0];
    }
}
