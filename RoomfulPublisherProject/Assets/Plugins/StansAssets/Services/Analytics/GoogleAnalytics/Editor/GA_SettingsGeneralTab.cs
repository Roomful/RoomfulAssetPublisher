using UnityEngine;
using UnityEditor;
using System.Collections;

using StansAssets.Plugins.Editor;


namespace SA.Analytics.Google
{

    public class GA_SettingsGeneralTab : IMGUILayoutElement
    {


        private static GUIContent acountNameLabel = new GUIContent("Account Name [?]:", "Name of Google Analytics Account");
        
        private static GUIContent testingModeLabel = new GUIContent("Testing [?]:", "This account will be used if testing mode enabled");

        private static GUIContent appTrackingIdLabel = new GUIContent("Tracking Id [?]:", "The ID that distinguishes to which Google Analytics property to send data.");
        
        private static GUIContent newLevelTrackingLabel = new GUIContent("Levels [?]:", "Screen Hit will be sent automaticaly when new level is loaded");
        private static GUIContent exTrackingLabel = new GUIContent("Exceptions [?]:", "Application exceptions reports will be sent automatically ");
        private static GUIContent systemInfoTrackingLabel = new GUIContent("SystemInfo [?]:", "System info will be automatically submitted on first launch ");
        private static GUIContent quitTrackingLabel = new GUIContent("App Quit [?]:", "Automatically track app quit.");
        private static GUIContent pauseTrackingLabel = new GUIContent("App [?]:", "Automatically track when app goes background and start / end session on this event ");
        
        
        private static GUIContent levelPostfix = new GUIContent("Level Postfix [?]:", "Postfix for loaded scene name ");
        private static GUIContent levelPrefix = new GUIContent("Level Prefix [?]:", "Prefix for loaded scene name");
        
        
        private static GUIContent UseHttpsLabel = new GUIContent("Use HTTPS [?]", "Enable data send  over SSL");
        private static GUIContent StringEscape  = new GUIContent("String Escape [?]", "Enable All strings  escaping using WWW.EscapeURL, Escaping of strings is safer, but adds additional RAM consummation");
        private static GUIContent EditorAnalytics  = new GUIContent("Send From Editor [?]", "Enable sending analytics while you testing your game in Unity Editor");
        
        
        private static GUIContent EnableCachingLabel  = new GUIContent("Enable Caching [?]", "When Internet Connection is not available event hits will be saved and sanded when connection is recovered.");
        private static GUIContent EnableQueueTimeLabel  = new GUIContent("Enable Queue Time [?]", "Queue Time to report time when hit occurred. But values greater than four hours may lead to hits not being processed by Google.");
        


        public override void OnGUI() {
            #if UNITY_WEBPLAYER
            EditorGUILayout.HelpBox("Sending analytics in Editor is disabled because you using Web Player Mode. To find out more about analytics usgae in web player, please click the link bellow", MessageType.Warning);
            EditorGUILayout.BeginHorizontal();
            
            EditorGUILayout.Space();
            if(GUILayout.Button("Using With Web Player",  GUILayout.Width(150))) {
                Application.OpenURL("http://goo.gl/5lbHLd");
            }
            
            EditorGUILayout.EndHorizontal();
            #endif


            using (new IMGUIBlockWithIndent(new GUIContent("Accounts"))) {

                int indentLevel = EditorGUI.indentLevel;
                EditorGUI.indentLevel = 0;
                using (new IMGUIBeginHorizontal()) {
                    GUILayout.Space(15);

                    using (new IMGUIBeginVertical()) {
                        Accounts();
                    }
                }

                EditorGUI.indentLevel = indentLevel;
            }


            using (new IMGUIBlockWithIndent(new GUIContent("TestingMode"))) {
                TestingMode();
            }

            using (new IMGUIBlockWithIndent(new GUIContent("Advanced Tracking"))) {
                AdvancedTracking();
            }

            using (new IMGUIBlockWithIndent(new GUIContent("Auto Tracking"))) {
                AutoTracking();
            }

            using (new IMGUIBlockWithIndent(new GUIContent("Actions"))) {
                ButtonsGUI();
            }

        }


        private static void ButtonsGUI() {
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.Space();
            if (GUILayout.Button("Refresh Client Id", GUILayout.Width(120))) {
                PlayerPrefs.DeleteKey(GA_Manager.GOOGLE_ANALYTICS_CLIENTID_PREF_KEY);
            }


            GUI.enabled = true;

            Color c = GUI.color;
            string text = "";
            if (GA_Settings.Instance.IsDisabled) {
                text = "Enable Analytics";
                GUI.color = Color.green;
            } else {
                text = "Disable Analytics";
                GUI.color = Color.red;
            }


            if (GUILayout.Button(text, GUILayout.Width(120))) {
                GA_Settings.Instance.IsDisabled = !GA_Settings.Instance.IsDisabled;
            }

            GUI.color = c;


            EditorGUILayout.EndHorizontal();
        }

        private static void AutoTracking() {
          
            GA_Settings.Instance.AutoExceptionTracking = EditorGUILayout.Toggle(exTrackingLabel, GA_Settings.Instance.AutoExceptionTracking);
            GA_Settings.Instance.SubmitSystemInfoOnFirstLaunch = EditorGUILayout.Toggle(systemInfoTrackingLabel, GA_Settings.Instance.SubmitSystemInfoOnFirstLaunch);
            GA_Settings.Instance.AutoAppResumeTracking = EditorGUILayout.Toggle(pauseTrackingLabel, GA_Settings.Instance.AutoAppResumeTracking);
            GA_Settings.Instance.AutoAppQuitTracking = EditorGUILayout.Toggle(quitTrackingLabel, GA_Settings.Instance.AutoAppQuitTracking);



            GA_Settings.Instance.AutoLevelTracking = EditorGUILayout.Toggle(newLevelTrackingLabel, GA_Settings.Instance.AutoLevelTracking);

            if (GA_Settings.Instance.AutoLevelTracking) {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(levelPrefix);
                EditorGUILayout.LabelField(levelPostfix);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                GA_Settings.Instance.LevelPrefix = EditorGUILayout.TextField(GA_Settings.Instance.LevelPrefix);
                GA_Settings.Instance.LevelPostfix = EditorGUILayout.TextField(GA_Settings.Instance.LevelPostfix);
                EditorGUILayout.EndHorizontal();
            }

            
            EditorGUILayout.Space();
        }

        private static void AdvancedTracking() {
            GA_Settings.Instance.UseHTTPS = EditorGUILayout.Toggle(UseHttpsLabel, GA_Settings.Instance.UseHTTPS);
            GA_Settings.Instance.StringEscaping = EditorGUILayout.Toggle(StringEscape, GA_Settings.Instance.StringEscaping);
            GA_Settings.Instance.EditorAnalytics = EditorGUILayout.Toggle(EditorAnalytics, GA_Settings.Instance.EditorAnalytics);


           

            GA_Settings.Instance.IsRequetsCachingEnabled = EditorGUILayout.Toggle(EnableCachingLabel, GA_Settings.Instance.IsRequetsCachingEnabled);
            GA_Settings.Instance.IsQueueTimeEnabled = EditorGUILayout.Toggle(EnableQueueTimeLabel, GA_Settings.Instance.IsQueueTimeEnabled);

        }


        private void TestingMode() {
            if (GA_Settings.Instance.accounts.Count == 0) {
                EditorGUILayout.HelpBox("Setup at least one Google Analytics Profile", MessageType.Error);
            } else {
                EditorGUILayout.HelpBox("If Testing mode is enabled, testing account will be used on all platfroms. You will also get build warning if building with testing mode enabled. Make sure you will disable it with the production build", MessageType.Info);

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(testingModeLabel);
                GA_Settings.Instance.IsTestingModeEnabled = EditorGUILayout.Toggle(GA_Settings.Instance.IsTestingModeEnabled);
                EditorGUILayout.EndHorizontal();

            }
        }

        private void Accounts() {

            foreach (GA_Profile profile in GA_Settings.Instance.accounts) {

              
                EditorGUILayout.BeginVertical(GUI.skin.box);
                profile.IsOpen = EditorGUILayout.Foldout(profile.IsOpen, profile.Name);
                if (profile.IsOpen) {
   
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(acountNameLabel);
                    profile.Name = EditorGUILayout.TextField(profile.Name);
                    EditorGUILayout.EndHorizontal();



                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(appTrackingIdLabel);
                    profile.TrackingID = EditorGUILayout.TextField(profile.TrackingID);
                    EditorGUILayout.EndHorizontal();



                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.Space();

                    if (GUILayout.Button("Set For All Platfroms", GUILayout.Width(150))) {
                        int options = EditorUtility.DisplayDialogComplex(
                            "Setting Account",
                            "Setting " + profile.Name + " for all platfroms",
                            "Set As Testing",
                            "Cancel",
                            "Set As Production"
                            );

                        switch (options) {
                            case 0:

                                foreach (RuntimePlatform platfrom in (RuntimePlatform[])System.Enum.GetValues(typeof(RuntimePlatform))) {
                                    GA_Settings.Instance.SetProfileIndexForPlatfrom(platfrom, GA_Settings.Instance.GetProfileIndex(profile), true);
                                }

                                break;
                            case 2:
                                foreach (RuntimePlatform platfrom in (RuntimePlatform[])System.Enum.GetValues(typeof(RuntimePlatform))) {
                                    GA_Settings.Instance.SetProfileIndexForPlatfrom(platfrom, GA_Settings.Instance.GetProfileIndex(profile), false);
                                }
                                break;


                        }

                    }


                    if (GUILayout.Button("Remove", GUILayout.Width(80))) {
                        GA_Settings.Instance.RemoveProfile(profile);
                        return;
                    }
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.Space();

                }

                EditorGUILayout.EndVertical();

            }


          
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.Space();
            if (GUILayout.Button("Add New Account", GUILayout.Width(120))) {
                GA_Settings.Instance.AddProfile(new GA_Profile());
            }

            EditorGUILayout.EndHorizontal();
        }
       
    }
}