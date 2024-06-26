using System;
using System.Collections.Generic;
using UnityEngine;

namespace net.roomful.api.styles
{
    public interface IStyleService
    {
        float RoomXBoundLeft { get; }
        
        float LeftRoomLimit { get; }
        float RightRoomLimit { get; }

        event Action RoomSizeUpdated;
        event Action<float, float> OnRoomLimitsChanged;

        event Action<IStylePanel> OnAboutToRemovePanel;

        IEnumerable<IStylePanel> GetActivePanels();
        IReadOnlyList<IStyle> Styles { get; }

        void LockPanelEdit(string panelId, string reason);

        /// <summary>
        /// Removes style from the room.
        /// </summary>
        /// <param name="styleTemplateId">The Style template id.</param>
        /// <param name="callback">Callback is fired when style remove animation is completed.</param>
        void RemoveStyle(string styleTemplateId, Action callback);

        void RegisterComponent(StyleComponent styleComponent);
        void AddComponentCreatedCallback<T>(Action<T> callback) where T : StyleComponent;
        
        
        IStylePanel GetRoomPanelById(string id);
    }
    
    public static class StyleServiceExtensions {
        public static bool TryGetPanelOutOfTransform(this IStyleService @this, Transform transform, out IStylePanel panel) {
            panel = null;
            
            var branch = transform;
            while (branch != null) {
                if (branch.GetComponent<IStylePanel>() != null) {
                    panel = branch.GetComponent<IStylePanel>();

                    return true;
                }

                branch = branch.transform.parent;
            }

            return false;
        }
    }
}
