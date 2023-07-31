using System;
using UnityEngine;

namespace Siccity.GLTFUtility
{
    /// <summary> Defines which shaders to use in the gltf import process </summary>
    [Serializable]
    public class ShaderSettings
    {
        private static Material s_opaqueMaterial;
        private static Material s_transparentMaterial;

        public static Material OpaqueMaterial {
            get {
                if (s_opaqueMaterial == null) {
                    s_opaqueMaterial = Resources.Load<Material>("RPM_Avatar_Material_Opaque");
                }

                return s_opaqueMaterial;
            }
        }

        public static Material TransparentMaterial {
            get {
                if (s_transparentMaterial == null) {
                    s_transparentMaterial = Resources.Load<Material>("RPM_Avatar_Material_Transparent");
                }

                return s_transparentMaterial;
            }
        }
    }
}