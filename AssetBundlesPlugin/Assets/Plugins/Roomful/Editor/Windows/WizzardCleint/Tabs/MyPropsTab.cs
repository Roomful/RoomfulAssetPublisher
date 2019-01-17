#if UNITY_2018_3_OR_NEWER

using UnityEngine;
using System.Collections;



namespace RF.AssetWizzard.Editor
{
    public class MyPropsTab : BaseWizzardTab, IWizzardTab
    {
        public override string Name {
            get {
                return "My Props";
            }
        }
    }
}

#endif
