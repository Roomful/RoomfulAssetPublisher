using UnityEngine;
using UnityEditor;
using System;

namespace RF.AssetWizzard.Editor
{
	public class CreatePropVariant : EditorWindow
	{
		public Action<string> OnCreateClickEvent;
		string m_name = "prop variant";
		private bool isFocused;
		void OnGUI()
		{

			GUIContent headerContent = new GUIContent("Enter variant name");
			EditorGUI.LabelField(new Rect(100, 10, 300, 40), headerContent);
			GUI.SetNextControlName(m_name);
			m_name = EditorGUI.TextField(new Rect(50, 40, 200, 16), m_name);
			if (!isFocused)
			{
				EditorGUI.FocusTextInControl(m_name);
				isFocused = true;
			}
			GUILayout.Space(80f);
			GUILayout.BeginHorizontal();
			{
				GUILayout.Space(25);
				bool cancel = GUILayout.Button("Cancel", EditorStyles.miniButton, new GUILayoutOption[] { GUILayout.Width(100) });
				if (cancel)
				{
					Dismiss();
				}
				GUILayout.Space(50);
				bool create = GUILayout.Button("Create", EditorStyles.miniButton, new GUILayoutOption[] { GUILayout.Width(100) });
				if (create)
				{
					if (OnCreateClickEvent != null)
						OnCreateClickEvent(m_name);

					Dismiss();
				}
			}
			GUILayout.EndHorizontal();

		}

		private void Dismiss()
		{
			OnCreateClickEvent = null;
			this.Close();
		}
	}
}
