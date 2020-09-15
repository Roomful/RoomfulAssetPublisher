using System;
using System.Collections.Generic;
using UnityEngine;

namespace net.roomful.assets.serialization
{
    [Serializable]
    internal class DummyVideoTexturePlugin : MonoBehaviour, IVideoTexturePlugin
    {
        private readonly List<Texture2D> m_textures = new List<Texture2D>();

        private int m_presenterTextureIndex;
        private int m_shareScreenTextureIndex;

        private float m_timeSinceLastUpdate;
        private System.Random m_random;

        private const float SWITCH_TIME = 0.5f;

        public void Init() {
            m_textures.Add(CreateTexture(Color.red));
            m_textures.Add(CreateTexture(Color.green));
            m_textures.Add(CreateTexture(Color.blue));
            m_textures.Add(CreateTexture(Color.yellow));
            m_textures.Add(CreateTexture(Color.gray));
            m_textures.Add(CreateTexture(Color.magenta));
            m_textures.Add(CreateTexture(Color.cyan));

            m_random = new System.Random();
            m_presenterTextureIndex = m_random.Next(0, m_textures.Count - 1);
            m_shareScreenTextureIndex = m_random.Next(0, m_textures.Count - 1);
        }

        void Update() {
            m_timeSinceLastUpdate += Time.deltaTime;
            if (m_timeSinceLastUpdate >= SWITCH_TIME) {
                m_presenterTextureIndex = m_random.Next(0, m_textures.Count - 1);
                m_shareScreenTextureIndex = m_random.Next(0, m_textures.Count - 1);
                m_timeSinceLastUpdate = 0.0f;
            }
        }

        Texture2D CreateTexture(Color color) {
            var tex = new Texture2D(2, 1, TextureFormat.RGBA32, false) { wrapMode = TextureWrapMode.Clamp };
            tex.SetPixel(0, 0, color);
            tex.SetPixel(1, 0, color);
            tex.Apply();
            return tex;
        }

        public Texture2D PresenterTex => m_textures[m_presenterTextureIndex];
        public Texture2D ShareScreenTex => m_textures[m_shareScreenTextureIndex];

        public bool IsPresenterTextureReady => true;
        public bool IsShareScreenTextureReady => true;
    }
}