using UnityEngine.EventSystems;

namespace net.roomful.assets.serialization
{
    public class ButtonTrigger : UnityEngine.UI.Button
    {
        public Button Target { get; private set; }

        public void BindTo(Button target) {
            Target = target;
        }
        
        public override void OnPointerDown(PointerEventData eventData) {
            base.OnPointerDown(eventData);

            if (Target != null) {
                Target.HandlePointerDown(new PointerDownArgs {
                    Button = (InputButton)(int)eventData.button
                });
            }
        }

        public override void OnPointerUp(PointerEventData eventData) {
            base.OnPointerUp(eventData);

            if (Target != null) {
                Target.HandlePointerUp(new PointerUpArgs {
                    Button = (InputButton)(int)eventData.button
                });
            }
        }
    }
}