using UnityEngine;

namespace net.roomful.api.styles
{
    public class StyleComponent : MonoBehaviour
    {
        void Start()
        {
            if(Roomful.StyleService != null)
                Roomful.StyleService.RegisterComponent(this);
        }
    }
}
