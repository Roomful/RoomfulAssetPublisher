
using System;
#if UNITY_WEBGL
using System.Runtime.InteropServices;
#endif
using UnityEngine;

namespace net.roomful.assets.serialization {
    [Serializable]
    public class WebGlVideoTexturePlugin : MonoBehaviour, IVideoTexturePlugin {
#if UNITY_WEBGL
        [DllImport("__Internal")]
        static extern void WebGlVideoChatVideoTextureSetup();

        [DllImport("__Internal")]
        static extern void WebGlVideoChatVideoTextureUpdate(string identity, int texture);

        [DllImport("__Internal")]
        static extern int WebGlVideoChatVideoTextureWidth(string identity);

        [DllImport("__Internal")]
        static extern int WebGlVideoChatVideoTextureHeight(string identity);

        [DllImport("__Internal")]
        static extern bool WebGlVideoChatVideoTextureIsReady(string identity);
#endif

        Texture2D m_VideoTextureCurrentPresenter;
        Texture2D m_VideoTextureCurrentShareScreen;

        const string k_PresenterIdentity = "presenter";
        const string k_ShareScreenIdentity = "share-screen";

        public Texture2D PresenterTex => m_VideoTextureCurrentPresenter;
        public Texture2D ShareScreenTex => m_VideoTextureCurrentShareScreen;

        public bool IsPresenterTextureReady {
            get {
#if UNITY_WEBGL
                return m_VideoTextureCurrentPresenter != null && m_VideoTextureCurrentPresenter.isReadable &&
                       IsWebGlVideoChatTextureReady(k_PresenterIdentity);
#else
            return false;
#endif
            }
        }

        public bool IsShareScreenTextureReady {
            get {
#if UNITY_WEBGL
                return m_VideoTextureCurrentShareScreen != null && m_VideoTextureCurrentShareScreen.isReadable &&
                       IsWebGlVideoChatTextureReady(k_ShareScreenIdentity);
#else
            return false;
#endif
            }
        }

        public void Init() {
#if UNITY_WEBGL
            // Init WebGL for Plugin
            WebGlVideoChatVideoTextureSetup();
#endif

            // Presenter
            m_VideoTextureCurrentPresenter =
                new Texture2D(1, 1, TextureFormat.RGBA32, false) {wrapMode = TextureWrapMode.Clamp};
            m_VideoTextureCurrentPresenter.SetPixel(0, 0, Color.black);
            m_VideoTextureCurrentPresenter.Apply();

            // Share screen
            m_VideoTextureCurrentShareScreen =
                new Texture2D(1, 1, TextureFormat.RGBA32, false) {wrapMode = TextureWrapMode.Clamp};
            m_VideoTextureCurrentShareScreen.SetPixel(0, 0, Color.black);
            m_VideoTextureCurrentShareScreen.Apply();
        }

        bool IsWebGlVideoChatTextureReady(string identity) {
#if UNITY_WEBGL
            return WebGlVideoChatVideoTextureIsReady(identity);
#else
        return false;
#endif
        }

        public void Update() {
#if UNITY_WEBGL
            // Presenter
            if (IsPresenterTextureReady) {
                var width = WebGlVideoChatVideoTextureWidth(k_PresenterIdentity);
                var height = WebGlVideoChatVideoTextureHeight(k_PresenterIdentity);

                if (width != m_VideoTextureCurrentPresenter.width || height != m_VideoTextureCurrentPresenter.height) {
                    m_VideoTextureCurrentPresenter.Resize(width, height, TextureFormat.RGBA32, false);
                    m_VideoTextureCurrentPresenter.Apply();
                }

                WebGlVideoChatVideoTextureUpdate(k_PresenterIdentity,
                    m_VideoTextureCurrentPresenter.GetNativeTexturePtr().ToInt32());
            }

            // Share screen
            if (IsShareScreenTextureReady) {
                var width = WebGlVideoChatVideoTextureWidth(k_ShareScreenIdentity);
                var height = WebGlVideoChatVideoTextureHeight(k_ShareScreenIdentity);

                if (width != m_VideoTextureCurrentShareScreen.width ||
                    height != m_VideoTextureCurrentShareScreen.height) {
                    m_VideoTextureCurrentShareScreen.Resize(width, height, TextureFormat.RGBA32, false);
                    m_VideoTextureCurrentShareScreen.Apply();
                }

                WebGlVideoChatVideoTextureUpdate(k_ShareScreenIdentity,
                    m_VideoTextureCurrentShareScreen.GetNativeTexturePtr().ToInt32());
            }
#endif
        }
    }
}