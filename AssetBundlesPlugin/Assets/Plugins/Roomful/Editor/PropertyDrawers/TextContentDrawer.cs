using System;
using UnityEngine;
using UnityEditor;

using RF.AssetBundles.Serialisation;


namespace RF.AssetWizzard.Editor {

	[CustomPropertyDrawer(typeof(TextContent))]
	public class TextContentDrawer : PropertyDrawer {


		public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {

			var DataProvider = property.FindPropertyRelative ("DataProvider");
			if( DataProvider.enumValueIndex != 0) {
				return EditorGUI.GetPropertyHeight(DataProvider) * 2f + 2f;
			} else {
				return EditorGUI.GetPropertyHeight(DataProvider);
			}
		}


		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {


			Rect drawPosition = new Rect (position);
			drawPosition.height = EditorGUIUtility.singleLineHeight;

			EditorGUI.BeginProperty(position, label, property);


			// Draw label
			drawPosition = EditorGUI.PrefixLabel(drawPosition, GUIUtility.GetControlID(FocusType.Passive), label);


			var DataProvider = property.FindPropertyRelative ("DataProvider");
			EditorGUI.PropertyField(drawPosition, DataProvider, GUIContent.none);

			if( DataProvider.enumValueIndex != 0) {
				drawPosition.y += drawPosition.height + EditorGUIUtility.standardVerticalSpacing;

				var indexRect = new Rect(drawPosition.x, drawPosition.y, 30, drawPosition.height);
				var sourceRect = new Rect(drawPosition.x + 35, drawPosition.y, drawPosition.width - 35, position.height);


				EditorGUI.PropertyField(indexRect, property.FindPropertyRelative("ResourceIndex"), GUIContent.none);
				EditorGUI.PropertyField(sourceRect, property.FindPropertyRelative("ResourceContentSource"), GUIContent.none);
			}
				

			EditorGUI.EndProperty();

		}

	

	}

}