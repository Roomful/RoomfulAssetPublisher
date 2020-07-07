using UnityEngine;

public class VideoTexturePlugin : MonoBehaviour
{
    enum StreamMode
    {
        None,
        Presenter,
        SharedScreen,
        SharedScreenWithPresenter
    }

    [Header("Shared Screen")]
    public Transform SharedScreenContainer;
    public MeshRenderer SharedScreen;

    [Header("Presenter")]
    public Transform PresenterContainer;
    public MeshRenderer Presenter;

    [Header("Shared Screen With Presenter")]
    public Transform SharedScreenWithPresenterContainer;

    public Transform PresenterRendererContainer;
    public MeshRenderer PresenterRenderer;
    public Transform SharedScreenRendererContainer;
    public MeshRenderer SharedScreenRenderer;

    readonly Vector3 m_ZeroPos = new Vector3(0.0f, 0.01f, 0.001f);

    StreamMode m_StreamMode = StreamMode.None;
    bool m_Initialized;

    void Init(GameObject container)
    {
        if (Api == null)
        {
            if (Application.platform == RuntimePlatform.WebGLPlayer)
                Api = container.AddComponent<WebGlVideoTexturePlugin>();
            else
                Api = container.AddComponent<DummyVideoTexturePlugin>();
        }

        Api.Init();
        m_Initialized = true;
    }

    void Update()
    {
        if (!m_Initialized)
            Init(gameObject);

        if (Api.IsPresenterTextureReady
            && Api.IsShareScreenTextureReady
            && Api.ShareScreenTex != null
            && Api.ShareScreenTex.isReadable
            && Api.PresenterTex != null
            && Api.PresenterTex.isReadable
        )
        {
            PresenterRenderer.material.mainTexture = Api.PresenterTex;
            SharedScreenRenderer.material.mainTexture = Api.ShareScreenTex;
            m_StreamMode = StreamMode.SharedScreenWithPresenter;
        }
        else if (Api.IsShareScreenTextureReady && Api.ShareScreenTex != null && Api.ShareScreenTex.isReadable )
        {
            SharedScreen.material.mainTexture = Api.ShareScreenTex;
            m_StreamMode = StreamMode.SharedScreen;
        }
        else if (Api.IsPresenterTextureReady && Api.PresenterTex != null && Api.PresenterTex.isReadable)
        {
            Presenter.material.mainTexture = Api.PresenterTex;
            m_StreamMode = StreamMode.Presenter;
        }
        else
        {
            m_StreamMode = StreamMode.None;
        }

        ScaleTextures();
    }

    Vector3 GetScaleToFitContainer(Transform container, Texture2D tex)
    {
        var localScale = container.localScale;
        var oldRatio = localScale.x / localScale.y;
        var width = tex.width;
        var height = tex.height;
        var newRatio = (float)width / height;

        var newScale = Vector3.one;
        var diff = oldRatio / newRatio;
        if (newRatio < oldRatio)
            newScale.x = 1 / diff;
        else
            newScale.z = -diff;

        return newScale;
    }

    void ScaleTextures()
    {
        if (m_StreamMode == StreamMode.SharedScreenWithPresenter)
        {
            PresenterContainer.gameObject.SetActive(false);
            SharedScreenContainer.gameObject.SetActive(false);
            SharedScreenWithPresenterContainer.gameObject.SetActive(true);

            var t = SharedScreenRenderer.transform;
            t.localPosition = m_ZeroPos;
            t.localScale = GetScaleToFitContainer(SharedScreenRendererContainer, Api.ShareScreenTex);

            t = PresenterRenderer.transform;
            t.localPosition = m_ZeroPos;
            t.localScale = GetScaleToFitContainer(PresenterRendererContainer, Api.PresenterTex);
        }
        else if (m_StreamMode == StreamMode.SharedScreen)
        {
            PresenterContainer.gameObject.SetActive(false);
            SharedScreenContainer.gameObject.SetActive(true);
            SharedScreenWithPresenterContainer.gameObject.SetActive(false);

            var t = SharedScreen.transform;
            t.localPosition = m_ZeroPos;
            t.localScale = GetScaleToFitContainer(SharedScreenContainer, Api.ShareScreenTex);
        }
        else if (m_StreamMode == StreamMode.Presenter)
        {
            PresenterContainer.gameObject.SetActive(true);
            SharedScreenContainer.gameObject.SetActive(false);
            SharedScreenWithPresenterContainer.gameObject.SetActive(false);

            var t = Presenter.transform;
            t.localPosition = m_ZeroPos;
            t.localScale = GetScaleToFitContainer(PresenterContainer, Api.PresenterTex);
        }
        else
        {
            PresenterContainer.gameObject.SetActive(false);
            SharedScreenContainer.gameObject.SetActive(false);
            SharedScreenWithPresenterContainer.gameObject.SetActive(false);
        }
    }

    IVideoTexturePlugin Api { get; set; }
}
