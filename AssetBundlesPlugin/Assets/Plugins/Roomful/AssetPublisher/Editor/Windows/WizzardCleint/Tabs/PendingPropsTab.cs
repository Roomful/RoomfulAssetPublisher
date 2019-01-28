#if UNITY_2018_3_OR_NEWER

using UnityEngine;

namespace RF.AssetWizzard.Editor
{
    public class PendingPropsTab : BaseWizardTab, IWizzardTab
    {
        public override string Name {
            get {
                return "Pending Props";
            }
        }
    }
}


#endif