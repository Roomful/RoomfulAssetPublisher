using UnityEngine;
using UnityEditor;

namespace net.roomful.assets.Editor
{
    internal class EnvironmentWizzard : AssetWizzard<EnvironmentAsset>
    {
        protected override void Create() {
            WindowManager.ShowCreateNewEnvironment();
        }

        protected override void Download() {
            BundleService.Download(Asset.Template);
        }

        protected override void Upload() {
            BundleService.Upload(Asset);
        }


        public override void OnGUI(bool GUIState) {

            

            GUILayout.BeginHorizontal();


            GUILayout.BeginVertical(GUILayout.Width(370));
            {
                DrawTitleFiled(GUIState);
            }GUILayout.EndVertical();


            GUILayout.BeginVertical(GUILayout.Width(100));
            {
                Asset.Icon = (Texture2D)EditorGUILayout.ObjectField(Asset.Icon, typeof(Texture2D), false, GUILayout.Width(70), GUILayout.Height(70));

                if (Asset.Icon == null) {
                    DrawPreloaderAt(new Rect(525, 65, 32, 32));
                }

            }GUILayout.EndVertical();


            GUILayout.EndHorizontal();


         
            DrawTags();
            DrawControlButtons();

            GUI.enabled = true;
        }



    }
}