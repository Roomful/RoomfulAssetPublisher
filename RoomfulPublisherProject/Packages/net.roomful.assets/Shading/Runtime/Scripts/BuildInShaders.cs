using System.Collections.Generic;
using UnityEngine;

namespace net.roomful.assets
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Build In Shaders Asset", fileName = "BuildInShaders")]
    public class BuildInShaders : ScriptableObject
    {
        [SerializeField] List<Shader> m_BuiltInShader;
    }
}


