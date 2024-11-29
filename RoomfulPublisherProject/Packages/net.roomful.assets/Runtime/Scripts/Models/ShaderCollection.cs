using System.Collections.Generic;
using UnityEngine;

namespace net.roomful.assets
{
    [CreateAssetMenu(fileName = "ShaderCollection", menuName = "Roomful/ShaderCollection")]
    public class ShaderCollection : ScriptableObject
    {
        [SerializeField] List<Shader> m_Shaders;
        [SerializeField] List<Shader> m_URPShaders;

        public IReadOnlyList<Shader> Shaders => m_Shaders;
        public IReadOnlyList<Shader> URPShaders => m_URPShaders;
    }
}
