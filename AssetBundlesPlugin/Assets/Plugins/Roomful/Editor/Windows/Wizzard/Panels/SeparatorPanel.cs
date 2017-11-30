using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace RF.AssetWizzard.Editor
{
    public class SeparatorPanel : Panel
    {
        public SeparatorPanel(EditorWindow window) : base(window) { }

        public override void OnGUI() {

        }


        public override bool CanBeSelected {
            get {
                return false;
            }
        }
    }
}