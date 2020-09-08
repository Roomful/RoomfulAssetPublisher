using System.Collections.Generic;
using UnityEngine;

namespace net.roomful.assets
{
    public class Skin
    {
        public Texture2D PreviewIcon { get; set; }

        public readonly Dictionary<Renderer, List<Material>> MaterialDictionary;

        public string Name { get; private set; }

        public Skin(string name, Dictionary<Renderer, List<Material>> materialDictionary)
        {
            Name = name;
            MaterialDictionary = new Dictionary<Renderer, List<Material>>(materialDictionary);
        }
    }
}
