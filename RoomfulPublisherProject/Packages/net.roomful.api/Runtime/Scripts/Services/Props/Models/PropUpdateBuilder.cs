using System;
using System.Collections.Generic;
using StansAssets.Foundation.Patterns;
using UnityEngine;

namespace net.roomful.api.props
{
    /// <summary>
    /// Use it to build prop update request
    /// </summary>
    public class PropUpdateBuilder : IDisposable
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public bool? IsAllowTextChat { get; set; }
        public bool? ParticipatesInNextPrevWalk { get; set; }
        public PropInfoDisplayType? PropInfoDisplayType { get; set; }

        //TODO should be internal
        public float? Scale { get; set; }

        //TODO should be internal
        public Vector3? Rotation { get; set; }

        //TODO should be internal
        public Vector3? LocalPosition { get; set; }

        //TODO should be internal
        public string StyleId { get; set; }

        //TODO should be internal
        public string PanelId { get; set; }

        //TODO should be internal
        public string ParentPropId { get; set; }

        public Dictionary<string, string> Skins { get; set; }
        public PropUpdateBuilderVariantColor VariantColorUpdate { get; set; }
        public IPropAssetTemplate Asset { get; set; }

        public bool LockAppearanceUpdate { get; set; } = false;
        public bool LockServerSync { get; set; } = false;

        public IReadOnlyDictionary<string, object> UpdatedParams => m_updatedParams;
        public IEnumerable<string> RemovedParams => m_removedParams;
        public IEnumerable<string> AddedPropTypes => m_addedPropTypes;
        public IEnumerable<string> RemovedPropTypes => m_removedPropTypes;
        public List<IPropEventsActionsModel> EventsActions => m_eventActions;

        private readonly List<string> m_removedParams;
        private readonly List<string> m_addedPropTypes;
        private readonly List<string> m_removedPropTypes;

        private List<IPropEventsActionsModel> m_eventActions;

        private Dictionary<string, object> m_updatedParams;

        public PropUpdateBuilder() {
            m_addedPropTypes = ListPool<string>.Get();
            m_removedParams = ListPool<string>.Get();
            m_removedPropTypes = ListPool<string>.Get();
        }

        public void Dispose() {
            ListPool<string>.Release(m_addedPropTypes);
            ListPool<string>.Release(m_removedParams);
            ListPool<string>.Release(m_removedPropTypes);
        }

        public void AddPropType(string propType) {
            m_addedPropTypes.Add(propType);
        }

        public void RemovePropType(string propType) {
            m_removedPropTypes.Add(propType);
        }

        public void SetParam(string paramKey, object paramValue) {
            if (m_updatedParams == null) {
                m_updatedParams = new Dictionary<string, object>();
            }

            m_updatedParams[paramKey] = paramValue;
        }

        public void RemoveParam(string paramKey) {
            m_removedParams.Add(paramKey);
        }

        public void SetActions(List<IPropEventsActionsModel> eventActions) {
            m_eventActions = eventActions;
        }

        public void SetAsset(IPropAssetTemplate assetTemplate) {
            Asset = assetTemplate;
        }

        public void SetSkins(Dictionary<string, string> skins) {
            Skins = skins;
        }

        public void SetSVariantColor(string variantId, Color color) {
            VariantColorUpdate = new PropUpdateBuilderVariantColor {
                VariantId = variantId,
                Color = color
            };
        }

        public bool ShouldUpdateAppearance {
            get {
                if (LockAppearanceUpdate)
                    return false;

                return Skins != null || Asset != null;
            }
        }
    }
}
