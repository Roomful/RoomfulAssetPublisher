﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Rotorz.ReorderableList;


namespace RF.AssetWizzard.Editor
{
    public class StyleWizzard : AssetWizzard<StyleAsset>
    {

        public override void Create() {
            WindowManager.ShowCreateNewStyle();
        }

        public override void Download() {
            BundleService.Download<StyleTemplate>(Asset.Template);
        }

        public override void Upload() {
            BundleService.Upload<StyleAsset>(Asset);
        }


        public override void OnGUI(bool GUIState) {

            

            GUILayout.BeginHorizontal();


            GUILayout.BeginVertical(GUILayout.Width(370));
            {
                DrawTitleFiled(GUIState);
            }GUILayout.EndVertical();


            GUILayout.BeginVertical(GUILayout.Width(100));
            {
                Asset.Icon = (Texture2D)EditorGUILayout.ObjectField(Asset.Icon, typeof(Texture2D), false, new GUILayoutOption[] { GUILayout.Width(70), GUILayout.Height(70) });

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