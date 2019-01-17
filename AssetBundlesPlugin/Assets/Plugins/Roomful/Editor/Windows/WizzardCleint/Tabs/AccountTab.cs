#if UNITY_2018_3_OR_NEWER

using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace RF.AssetWizzard.Editor
{
    public class AccountTab : BaseWizzardTab, IWizzardTab
    {
        public override string Name {
            get {
                return "Account";
            }
        }
    }
}


#endif