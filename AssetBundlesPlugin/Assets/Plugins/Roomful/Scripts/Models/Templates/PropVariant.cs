using System.Collections.Generic;
using UnityEngine;

namespace RF.AssetWizzard
{
    public class PropVariant
    {
        public string Name { get; private set; }

        public readonly List<Renderer> Renderers;

        readonly List<Material> m_Materials;

        public IEnumerable<Material> Materials
        {
            get { return m_Materials; }
        }

        readonly List<Skin> m_Skins = new List<Skin>();

        public PropVariant(string name, IEnumerable<Renderer> renderers)
        {
            Name = name;
            Renderers = new List<Renderer>(renderers);

            m_Materials = new List<Material>();
            foreach (var renderer in Renderers)
            {
                m_Materials.Add(renderer.sharedMaterial);
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
            // OMG. Ya v ahue s etogo koda 0_o
            // PLEASE, UNSEE *_*

            int index = 0;
            foreach (var renderer in Renderers)
            {
                renderer.material = skin.GetMaterial(index);
                index++;
            }
        }

        public static IEnumerable<string> GetHierarchyForObject(GameObject go)
        {
            List<string> hierarchy = new List<string>();

            Transform t = go.transform.parent;
            while (t != null)
            {
                hierarchy.Add(t.name);
                t = t.parent;
            }

            hierarchy.Reverse();
            return hierarchy;
        }

        public IEnumerable<Skin> Skins
        {
            get
            {
                return m_Skins;
            }
        }

        public Skin DefaultSkin
        {
            get
            {
                return m_Skins[0];
            }
        }
    }
}
