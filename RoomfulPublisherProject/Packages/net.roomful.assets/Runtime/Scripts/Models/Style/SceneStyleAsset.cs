using net.roomful.api;
using UnityEngine;

namespace net.roomful.assets
{
    class SceneStyleAsset : BaseStyleAsset
    {
        [SerializeField] string m_StyleId;
        [SerializeField] string m_Title;
        [SerializeField] Texture2D m_Thumbnail;
        
        [ContextMenu("PrepareForUpload")]
        public override void PrepareForUpload()
        {
            var sceneStyle = GetComponent<SceneStyle>();
            if (sceneStyle == null)
            {
                sceneStyle = gameObject.AddComponent<SceneStyle>();
            }
            
            sceneStyle.Clear();
            var renderers = GetComponentsInChildren<Renderer>();
            foreach (var rnd in renderers)
            {
                foreach (var material in rnd.sharedMaterials)
                {
                    sceneStyle.AddMaterial(material);
                }
            }
            
            var graphics = transform.Find("Graphics");
            sceneStyle.SetGraphics(graphics);
        }
        
        public void Validate()
        {
            Debug.Log("Validate");
            if (Template.StyleType != StyleType.NonExtendable)
            {
                Template.StyleType = StyleType.NonExtendable;
            }

            if (!string.IsNullOrEmpty(m_StyleId))
            {
                Template.Id = m_StyleId;
            }
            
            if (!string.IsNullOrEmpty(m_Title))
            {
                Template.Title = m_Title;
                gameObject.name = m_Title;
            }

            if (m_Thumbnail != null)
            {
                Template.Icon.SetThumbnail(m_Thumbnail);
            }
        }
    }
}