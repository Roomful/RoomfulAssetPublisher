using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SA.Productivity.SceneValidator
{
    public class SV_UserSettings 
    {

        public const string PLAYER_PREFS_KEY = "SCENE_VALIDATION_USER_SETTINGS";


        //API
        public bool ValidationEnabled = true;
        public bool AllowIssueIgnore = true;


        //Inspector UI settnings
        public bool DisplayIssuesCount = true;
        public bool DisplayIssueRelatedComponent = true;
        public bool DisplayRuleClassName = true;


        //Hierarchy UI
        public SV_IconAlligment HierarchyIconsAlligment = SV_IconAlligment.Left;

        //Scene View UI
        public bool DisplaySceneViewWindow = true;



    }
}