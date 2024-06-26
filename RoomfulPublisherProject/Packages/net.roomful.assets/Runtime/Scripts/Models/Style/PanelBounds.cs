using UnityEngine;


namespace net.roomful.assets
{
    internal class PanelBounds : AssetBounds
    {
        protected override bool IsValidForBounds(Renderer renderer) {
            var isValid = base.IsValidForBounds(renderer);
            if (!isValid) {
                return false;
            }

            if (renderer.GetComponent<StylePanel>() != null) {
                return false; ;
            }

            return true;
        }
    }
}