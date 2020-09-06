using System;
using System.Collections.Generic;
using UnityEngine;

namespace net.roomful.assets.serialization {
    [Serializable]
    public class DummyVideoTexturePlugin : MonoBehaviour, IVideoTexturePlugin {
        readonly List<Texture2D> m_Textures = new List<Texture2D>();

        int m_PresenterTextureIndex;
        int m_ShareScreenTextureIndex;

        float m_TimeSinceLastUpdate;
        System.Random m_Random;

        const float k_SwitchTime = 0.5f;

        public void Init() {
            m_Textures.Add(CreateTexture(Color.red));
            m_Textures.Add(CreateTexture(Color.green));
            m_Textures.Add(CreateTexture(Color.blue));
            m_Textures.Add(CreateTexture(Color.yellow));
            m_Textures.Add(CreateTexture(Color.gray));
            m_Textures.Add(CreateTexture(Color.magenta));
            m_Textures.Add(CreateTexture(Color.cyan));

            m_Random = new System.Random();
            m_PresenterTextureIndex = m_Random.Next(0, m_Textures.Count - 1);
            m_ShareScreenTextureIndex = m_Random.Next(0, m_Textures.Count - 1);
        }

        void Update() {
            m_TimeSinceLastUpdate += Time.deltaTime;
            if (m_TimeSinceLastUpdate >= k_SwitchTime) {
                m_PresenterTextureIndex = m_Random.Next(0, m_Textures.Count - 1);
                m_ShareScreenTextureIndex = m_Random.Next(0, m_Textures.Count - 1);
                m_TimeSinceLastUpdate = 0.0f;
            }
        }

        Texture2D CreateTexture(Color color) {
            var tex = new Texture2D(2, 1, TextureFormat.RGBA32, false) {wrapMode = TextureWrapMode.Clamp};
            tex.SetPixel(0, 0, color);
            tex.SetPixel(1, 0, color);
            tex.Apply();
            return tex;
        }

        public Texture2D PresenterTex => m_Textures[m_PresenterTextureIndex];
        public Texture2D ShareScreenTex => m_Textures[m_ShareScreenTextureIndex];

        public bool IsPresenterTextureReady => true;
        public bool IsShareScreenTextureReady => true;
    }
}