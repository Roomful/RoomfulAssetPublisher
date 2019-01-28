#if UNITY_2018_3_OR_NEWER

using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace RF.AssetWizzard.Editor
{
    public class UserSettingsTab : BaseWizardTab, IWizzardTab
    {

        SettingsPanel m_oldSettings;

        public UserSettingsTab() : base() {


            var oldUIContainer = new IMGUIContainer(OnGUI);
            Add(oldUIContainer);

            m_oldSettings = new SettingsPanel(null);
        }

        private void OnGUI() {
            m_oldSettings.OnGUI();
        }


        public override string Name {
            get {
                return "Settings";
            }
        }
    }
}


#endif