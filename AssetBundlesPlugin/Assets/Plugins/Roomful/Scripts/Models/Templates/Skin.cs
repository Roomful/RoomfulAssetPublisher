using System.Collections.Generic;
using UnityEngine;

namespace RF.AssetWizzard
{
    public class Skin
    {
        public Texture2D PreviewIcon { get; set; }

        public readonly List<Material> Materials;

        public string Name { get; private set; }

        public Skin(string name, IEnumerable<Material> materials)
        {
            Name = name;
            Materials = new List<Material>(materials);
        }

        public Material GetMaterial(int index)
        {
            return Materials[index];
        }
    }
}
