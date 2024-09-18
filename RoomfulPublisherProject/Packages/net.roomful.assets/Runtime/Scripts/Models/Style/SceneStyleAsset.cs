using System;
using System.Collections.Generic;
using System.Linq;
using net.roomful.api;
using net.roomful.assets.serialization;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace net.roomful.assets
{
    class SceneStyleAsset : BaseStyleAsset
    {
        [SerializeField] string m_StyleId;
        [SerializeField] string m_Title;
        [SerializeField] Texture2D m_Thumbnail;
        [SerializeField] Vector3 m_HomePosition;
        [SerializeField] float m_Price;
        [SerializeField] int m_SortingScore;
        [SerializeField] List<string> m_Tags;
        
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
            var decals = GetComponentsInChildren<DecalProjector>();
            foreach (var decal in decals)
            {
                sceneStyle.AddMaterial(decal.material);
            }
            
            var reflectiveFloors = GetComponentsInChildren<SerializedReflectiveFloor>();
            sceneStyle.SetReflectiveFloors(reflectiveFloors.ToList());
        }
        
        public override GameObject Environment => null;

        public void Validate()
        {
            Debug.Log("Validate");
            Template.StyleType = StyleType.NonExtendable;
            Template.DoorsType = StyleDoorsType.None;

            if (!string.IsNullOrEmpty(m_StyleId) || !string.IsNullOrEmpty(m_Title))
            {
                Template.Id = m_StyleId;
                Template.Title = m_Title;
                gameObject.name = m_Title;
                
                Template.HomePosition = m_HomePosition;
                Template.Price = Convert.ToDecimal(m_Price);
                Template.Score = m_SortingScore;
                
                Template.Tags.Clear();
                Template.Tags.AddRange(m_Tags);
                
                Template.Icon.SetThumbnail(m_Thumbnail);
                Icon = Template.Icon.Thumbnail;
            }
            
        }
    }
}