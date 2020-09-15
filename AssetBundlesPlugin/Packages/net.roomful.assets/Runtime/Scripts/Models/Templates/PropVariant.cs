using System.Collections.Generic;
using UnityEngine;

namespace net.roomful.assets
{
    public class PropVariant
    {
        public string Name { get; private set; }

        public readonly List<Renderer> Renderers;

        private readonly List<Material> m_materials;
        public readonly Dictionary<Renderer, List<Material>> MaterialDictionary;

        public IEnumerable<Material> Materials => m_materials;

        private readonly List<Skin> m_skins = new List<Skin>();

        public PropVariant(string name, IEnumerable<Renderer> renderers)
        {
            Name = name;
            Renderers = new List<Renderer>(renderers);
            MaterialDictionary = new Dictionary<Renderer, List<Material>>();

            m_materials = new List<Material>();
            foreach (var renderer in Renderers)
            {
                MaterialDictionary.Add(renderer, new List<Material>());
                foreach(var material in renderer.sharedMaterials)
                {
                    m_materials.Add(material);
                    MaterialDictionary[renderer].Add(material);
                }          
            }
        }

        public void AddSkin(Skin skin)
        {
            m_skins.Add(skin);
        }

        public void RemoveSkin(Skin skin)
        {
            m_skins.Remove(skin);
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

        public Skin DefaultSkin => m_skins[0];
        public IEnumerable<Skin> Skins => m_skins;
    }
}
