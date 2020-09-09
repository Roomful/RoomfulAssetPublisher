using UnityEngine;

namespace net.roomful.assets
{
    public class PropBounds : AssetBounds
    {
        public override bool IsValidForBounds(Renderer renderer) {
            var isValid = base.IsValidForBounds(renderer);
            if (!isValid) {
                return false;
            }

            return true;
        }
    }
}