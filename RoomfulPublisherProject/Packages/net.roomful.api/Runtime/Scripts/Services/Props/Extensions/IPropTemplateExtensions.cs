using System.Collections.Generic;
using System.Linq;

namespace net.roomful.api.props
{
    // ReSharper disable once InconsistentNaming
    public static class IPropTemplateExtensions
    {
        public static IResource FindInContent(this IPropTemplate template, string resId) {
            foreach (var resource in template.Content) {
                if (resource.Id.Equals(resId)) {
                    return resource;
                }
            }

            return null;
        }

        public static bool HasContent(this IPropTemplate template) {
            return template.Content.Count > 0;
        }

        public static string GetParamAsString(this IPropTemplate template, string paramKey) {
            var param = template.GetParam(paramKey);
            return param != null ? param.ToString() : string.Empty;
        }

        public static bool ContainsParam(this IPropTemplate template, string paramKey) {
            return template.GetParam(paramKey) != null;
        }

        public static List<IPropEventsActionsModel> FindAllActionsForTrigger(this IPropTemplate template, PropEventTrigger trigger) {

            var eventsActions = new List<IPropEventsActionsModel>();
            foreach (var action in template.PropEventsActions) {
                if(action.EnumEventName == trigger)
                    eventsActions.Add(action);
            }
            return eventsActions;
        }

        public static IEnumerable<IPropEventsActionsModel> GetActions(this IPropTemplate template, PropEventTrigger trigger) {
            return template.PropEventsActions
                .Where(item => item.EnumEventName == trigger);
        }

        public static bool HasNoAction(this IPropTemplate template) {
            var firstOnClickAction = template.GetActions(PropEventTrigger.OnClick).FirstOrDefault();
            return firstOnClickAction != null && firstOnClickAction.EnumActionName == PropAction.DoNothing;
        }

    }
}
