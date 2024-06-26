using UnityEngine;
using UnityEditor;

namespace net.roomful.assets.editor
{
    [CustomPropertyDrawer(typeof(FontData), true)]
    internal class FontDataDrawer : PropertyDrawer
    {
        private static class Styles
        {
            public static readonly GUIStyle alignmentButtonLeft;

            public static readonly GUIStyle alignmentButtonMid;

            public static readonly GUIStyle alignmentButtonRight;

            public static readonly GUIContent m_LeftAlignText;

            public static readonly GUIContent m_CenterAlignText;

            public static readonly GUIContent m_RightAlignText;

            public static readonly GUIContent m_TopAlignText;

            public static readonly GUIContent m_MiddleAlignText;

            public static readonly GUIContent m_BottomAlignText;

            public static readonly GUIContent m_LeftAlignTextActive;

            public static readonly GUIContent m_CenterAlignTextActive;

            public static readonly GUIContent m_RightAlignTextActive;

            public static readonly GUIContent m_TopAlignTextActive;

            public static readonly GUIContent m_MiddleAlignTextActive;

            public static readonly GUIContent m_BottomAlignTextActive;

            static Styles() {
                alignmentButtonLeft = new GUIStyle(EditorStyles.miniButtonLeft);
                alignmentButtonMid = new GUIStyle(EditorStyles.miniButtonMid);
                alignmentButtonRight = new GUIStyle(EditorStyles.miniButtonRight);
                m_LeftAlignText = EditorGUIUtility.IconContent("GUISystem/align_horizontally_left", "Left Align");
                m_CenterAlignText = EditorGUIUtility.IconContent("GUISystem/align_horizontally_center", "Center Align");
                m_RightAlignText = EditorGUIUtility.IconContent("GUISystem/align_horizontally_right", "Right Align");
                m_LeftAlignTextActive = EditorGUIUtility.IconContent("GUISystem/align_horizontally_left_active", "Left Align");
                m_CenterAlignTextActive = EditorGUIUtility.IconContent("GUISystem/align_horizontally_center_active", "Center Align");
                m_RightAlignTextActive = EditorGUIUtility.IconContent("GUISystem/align_horizontally_right_active", "Right Align");
                m_TopAlignText = EditorGUIUtility.IconContent("GUISystem/align_vertically_top", "Top Align");
                m_MiddleAlignText = EditorGUIUtility.IconContent("GUISystem/align_vertically_center", "Middle Align");
                m_BottomAlignText = EditorGUIUtility.IconContent("GUISystem/align_vertically_bottom", "Bottom Align");
                m_TopAlignTextActive = EditorGUIUtility.IconContent("GUISystem/align_vertically_top_active", "Top Align");
                m_MiddleAlignTextActive = EditorGUIUtility.IconContent("GUISystem/align_vertically_center_active", "Middle Align");
                m_BottomAlignTextActive = EditorGUIUtility.IconContent("GUISystem/align_vertically_bottom_active", "Bottom Align");
                FixAlignmentButtonStyles(alignmentButtonLeft, alignmentButtonMid, alignmentButtonRight);
            }

            private static void FixAlignmentButtonStyles(params GUIStyle[] styles) {
                for (var i = 0; i < styles.Length; i++) {
                    var gUIStyle = styles[i];
                    gUIStyle.padding.left = 2;
                    gUIStyle.padding.right = 2;
                }
            }
        }

        private enum VerticalTextAligment
        {
            Top,
            Middle,
            Bottom
        }

        private enum HorizontalTextAligment
        {
            Left,
            Center,
            Right
        }

        private const int kAlignmentButtonWidth = 20;

        private static readonly int s_TextAlignmentHash = "DoTextAligmentControl".GetHashCode();

        private SerializedProperty m_Font;

        private SerializedProperty m_FontSize;

        private SerializedProperty m_LineSpacing;
        private SerializedProperty m_LineCharacterLimit;

        private SerializedProperty m_FontStyle;

        private SerializedProperty m_HorizontalOverflow;

        private SerializedProperty m_VerticalOverflow;

        private SerializedProperty m_Alignment;

        private float m_FontFieldfHeight = 0f;

        private float m_FontStyleHeight = 0f;

        private float m_FontSizeHeight = 0f;

        private float m_LineSpacingHeight = 0f;
        private float m_LineCharacterLimitHeight = 0f;
        private float m_HorizontalOverflowHeight = 0f;
        private float m_VerticalOverflowHeight = 0f;

        protected void Init(SerializedProperty property) {
            m_Font = property.FindPropertyRelative("m_Font");
            m_FontSize = property.FindPropertyRelative("m_FontSize");
            m_LineSpacing = property.FindPropertyRelative("m_LineSpacing");
            m_LineCharacterLimit = property.FindPropertyRelative("m_LineCharacterLimit");

            m_FontStyle = property.FindPropertyRelative("m_FontStyle");

            m_HorizontalOverflow = property.FindPropertyRelative("m_HorizontalOverflow");
            m_VerticalOverflow = property.FindPropertyRelative("m_VerticalOverflow");
            m_Alignment = property.FindPropertyRelative("m_Alignment");
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            Init(property);
            m_FontFieldfHeight = EditorGUI.GetPropertyHeight(m_Font);
            m_FontStyleHeight = EditorGUI.GetPropertyHeight(m_FontStyle);
            m_FontSizeHeight = EditorGUI.GetPropertyHeight(m_FontSize);
            m_LineSpacingHeight = EditorGUI.GetPropertyHeight(m_LineSpacing);
            m_LineCharacterLimitHeight = EditorGUI.GetPropertyHeight(m_LineCharacterLimit);

            m_HorizontalOverflowHeight = EditorGUI.GetPropertyHeight(m_HorizontalOverflow);
            m_VerticalOverflowHeight = EditorGUI.GetPropertyHeight(m_VerticalOverflow);
            var num = m_FontFieldfHeight + m_FontStyleHeight + m_FontSizeHeight + m_LineSpacingHeight + m_LineCharacterLimitHeight + m_HorizontalOverflowHeight + m_VerticalOverflowHeight + EditorGUIUtility.singleLineHeight * 3f + EditorGUIUtility.standardVerticalSpacing * 10f;
            return num;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            Init(property);
            var position2 = position;
            position2.height = EditorGUIUtility.singleLineHeight;
            EditorGUI.LabelField(position2, "Character", EditorStyles.boldLabel);
            position2.y += position2.height + EditorGUIUtility.standardVerticalSpacing;
            EditorGUI.indentLevel++;
            var font = m_Font.objectReferenceValue as Font;
            position2.height = m_FontFieldfHeight;
            EditorGUI.BeginChangeCheck();
            EditorGUI.PropertyField(position2, m_Font);
            if (EditorGUI.EndChangeCheck()) {
                font = (m_Font.objectReferenceValue as Font);
                if (font != null && !font.dynamic) {
                    m_FontSize.intValue = font.fontSize;
                }
            }

            position2.y += position2.height + EditorGUIUtility.standardVerticalSpacing;
            position2.height = m_FontStyleHeight;
            using (new EditorGUI.DisabledScope(!m_Font.hasMultipleDifferentValues && font != null && !font.dynamic)) {
                EditorGUI.PropertyField(position2, m_FontStyle);
            }

            position2.y += position2.height + EditorGUIUtility.standardVerticalSpacing;
            position2.height = m_FontSizeHeight;
            EditorGUI.PropertyField(position2, m_FontSize);
            position2.y += position2.height + EditorGUIUtility.standardVerticalSpacing;
            position2.height = m_LineSpacingHeight;
            EditorGUI.PropertyField(position2, m_LineSpacing);

            position2.y += position2.height + EditorGUIUtility.standardVerticalSpacing;
            position2.height = m_LineSpacingHeight;
            EditorGUI.PropertyField(position2, m_LineCharacterLimit);

            EditorGUI.indentLevel--;
            position2.y += position2.height + EditorGUIUtility.standardVerticalSpacing;
            position2.height = EditorGUIUtility.singleLineHeight;
            EditorGUI.LabelField(position2, "Paragraph", EditorStyles.boldLabel);
            position2.y += position2.height + EditorGUIUtility.standardVerticalSpacing;
            EditorGUI.indentLevel++;
            position2.height = EditorGUIUtility.singleLineHeight;
            DoTextAligmentControl(position2, m_Alignment);
            position2.y += position2.height + EditorGUIUtility.standardVerticalSpacing;
            position2.height = m_HorizontalOverflowHeight;

            EditorGUI.PropertyField(position2, m_HorizontalOverflow);
            position2.y += position2.height + EditorGUIUtility.standardVerticalSpacing;
            position2.height = m_VerticalOverflowHeight;
            EditorGUI.PropertyField(position2, m_VerticalOverflow);
            position2.y += position2.height + EditorGUIUtility.standardVerticalSpacing;
            position2.y += position2.height + EditorGUIUtility.standardVerticalSpacing;

            EditorGUI.indentLevel--;
        }

        private void DoTextAligmentControl(Rect position, SerializedProperty alignment) {
            var label = new GUIContent("Alignment");
            var controlID = GUIUtility.GetControlID(s_TextAlignmentHash, FocusType.Keyboard, position);
            EditorGUIUtility.SetIconSize(new Vector2(15f, 15f));
            EditorGUI.BeginProperty(position, label, alignment);
            var rect = EditorGUI.PrefixLabel(position, controlID, label);
            var num = 60f;
            var num2 = Mathf.Clamp(rect.width - num * 2f, 2f, 10f);
            var position2 = new Rect(rect.x, rect.y, num, rect.height);
            var position3 = new Rect(position2.xMax + num2, rect.y, num, rect.height);
            DoHorizontalAligmentControl(position2, alignment);
            DoVerticalAligmentControl(position3, alignment);
            EditorGUI.EndProperty();
            EditorGUIUtility.SetIconSize(Vector2.zero);
        }

        private static void DoHorizontalAligmentControl(Rect position, SerializedProperty alignment) {
            var intValue = (TextAnchor) alignment.intValue;
            var horizontalAlignment = GetHorizontalAlignment(intValue);
            var flag = horizontalAlignment == HorizontalTextAligment.Left;
            var flag2 = horizontalAlignment == HorizontalTextAligment.Center;
            var flag3 = horizontalAlignment == HorizontalTextAligment.Right;
            if (alignment.hasMultipleDifferentValues) {
                var targetObjects = alignment.serializedObject.targetObjects;
                for (var i = 0; i < targetObjects.Length; i++) {
                    var @object = targetObjects[i];
                    var text = @object as RoomfulText;
                    horizontalAlignment = GetHorizontalAlignment(text.FontData.Alignment);
                    flag = (flag || horizontalAlignment == HorizontalTextAligment.Left);
                    flag2 = (flag2 || horizontalAlignment == HorizontalTextAligment.Center);
                    flag3 = (flag3 || horizontalAlignment == HorizontalTextAligment.Right);
                }
            }

            position.width = 20f;
            EditorGUI.BeginChangeCheck();
            EditorToggle(position, flag, (!flag) ? Styles.m_LeftAlignText : Styles.m_LeftAlignTextActive, Styles.alignmentButtonLeft);
            if (EditorGUI.EndChangeCheck()) {
                SetHorizontalAlignment(alignment, HorizontalTextAligment.Left);
            }

            position.x += position.width;
            EditorGUI.BeginChangeCheck();
            EditorToggle(position, flag2, (!flag2) ? Styles.m_CenterAlignText : Styles.m_CenterAlignTextActive, Styles.alignmentButtonMid);
            if (EditorGUI.EndChangeCheck()) {
                SetHorizontalAlignment(alignment, HorizontalTextAligment.Center);
            }

            position.x += position.width;
            EditorGUI.BeginChangeCheck();
            EditorToggle(position, flag3, (!flag3) ? Styles.m_RightAlignText : Styles.m_RightAlignTextActive, Styles.alignmentButtonRight);
            if (EditorGUI.EndChangeCheck()) {
                SetHorizontalAlignment(alignment, HorizontalTextAligment.Right);
            }
        }

        private static void DoVerticalAligmentControl(Rect position, SerializedProperty alignment) {
            var intValue = (TextAnchor) alignment.intValue;
            var verticalAlignment = GetVerticalAlignment(intValue);
            var flag = verticalAlignment == VerticalTextAligment.Top;
            var flag2 = verticalAlignment == VerticalTextAligment.Middle;
            var flag3 = verticalAlignment == VerticalTextAligment.Bottom;
            if (alignment.hasMultipleDifferentValues) {
                var targetObjects = alignment.serializedObject.targetObjects;
                for (var i = 0; i < targetObjects.Length; i++) {
                    var @object = targetObjects[i];
                    var text = @object as RoomfulText;
                    var alignment2 = text.FontData.Alignment;
                    verticalAlignment = GetVerticalAlignment(alignment2);
                    flag = (flag || verticalAlignment == VerticalTextAligment.Top);
                    flag2 = (flag2 || verticalAlignment == VerticalTextAligment.Middle);
                    flag3 = (flag3 || verticalAlignment == VerticalTextAligment.Bottom);
                }
            }

            position.width = 20f;
            EditorGUI.BeginChangeCheck();
            EditorToggle(position, flag, (!flag) ? Styles.m_TopAlignText : Styles.m_TopAlignTextActive, Styles.alignmentButtonLeft);
            if (EditorGUI.EndChangeCheck()) {
                SetVerticalAlignment(alignment, VerticalTextAligment.Top);
            }

            position.x += position.width;
            EditorGUI.BeginChangeCheck();
            EditorToggle(position, flag2, (!flag2) ? Styles.m_MiddleAlignText : Styles.m_MiddleAlignTextActive, Styles.alignmentButtonMid);
            if (EditorGUI.EndChangeCheck()) {
                SetVerticalAlignment(alignment, VerticalTextAligment.Middle);
            }

            position.x += position.width;
            EditorGUI.BeginChangeCheck();
            EditorToggle(position, flag3, (!flag3) ? Styles.m_BottomAlignText : Styles.m_BottomAlignTextActive, Styles.alignmentButtonRight);
            if (EditorGUI.EndChangeCheck()) {
                SetVerticalAlignment(alignment, VerticalTextAligment.Bottom);
            }
        }

        private static bool EditorToggle(Rect position, bool value, GUIContent content, GUIStyle style) {
            var hashCode = "AlignToggle".GetHashCode();
            var controlID = GUIUtility.GetControlID(hashCode, FocusType.Keyboard, position);
            var current = Event.current;
            if (GUIUtility.keyboardControl == controlID && current.type == EventType.KeyDown && (current.keyCode == KeyCode.Space || current.keyCode == KeyCode.Return || current.keyCode == KeyCode.KeypadEnter)) {
                value = !value;
                current.Use();
                GUI.changed = true;
            }

            if (current.type == EventType.KeyDown && Event.current.button == 0 && position.Contains(Event.current.mousePosition)) {
                GUIUtility.keyboardControl = controlID;
                EditorGUIUtility.editingTextField = false;
                HandleUtility.Repaint();
            }

            return GUI.Toggle(position, controlID, value, content, style);
        }

        private static HorizontalTextAligment GetHorizontalAlignment(TextAnchor ta) {
            HorizontalTextAligment result;
            switch (ta) {
                case TextAnchor.UpperLeft:
                case TextAnchor.MiddleLeft:
                case TextAnchor.LowerLeft:
                    result = HorizontalTextAligment.Left;
                    break;
                case TextAnchor.UpperCenter:
                case TextAnchor.MiddleCenter:
                case TextAnchor.LowerCenter:
                    result = HorizontalTextAligment.Center;
                    break;
                case TextAnchor.UpperRight:
                case TextAnchor.MiddleRight:
                case TextAnchor.LowerRight:
                    result = HorizontalTextAligment.Right;
                    break;
                default:
                    result = HorizontalTextAligment.Left;
                    break;
            }

            return result;
        }

        private static VerticalTextAligment GetVerticalAlignment(TextAnchor ta) {
            VerticalTextAligment result;
            switch (ta) {
                case TextAnchor.UpperLeft:
                case TextAnchor.UpperCenter:
                case TextAnchor.UpperRight:
                    result = VerticalTextAligment.Top;
                    break;
                case TextAnchor.MiddleLeft:
                case TextAnchor.MiddleCenter:
                case TextAnchor.MiddleRight:
                    result = VerticalTextAligment.Middle;
                    break;
                case TextAnchor.LowerLeft:
                case TextAnchor.LowerCenter:
                case TextAnchor.LowerRight:
                    result = VerticalTextAligment.Bottom;
                    break;
                default:
                    result = VerticalTextAligment.Top;
                    break;
            }

            return result;
        }

        private static void SetHorizontalAlignment(SerializedProperty alignment, HorizontalTextAligment horizontalAlignment) {
            var targetObjects = alignment.serializedObject.targetObjects;
            for (var i = 0; i < targetObjects.Length; i++) {
                var @object = targetObjects[i];

                var text = @object as RoomfulText;
                var verticalAlignment = GetVerticalAlignment(text.FontData.Alignment);
                Undo.RecordObject(text, "Horizontal Alignment");
                text.FontData.Alignment = GetAnchor(verticalAlignment, horizontalAlignment);
                EditorUtility.SetDirty(@object);
            }
        }

        private static void SetVerticalAlignment(SerializedProperty alignment, VerticalTextAligment verticalAlignment) {
            var targetObjects = alignment.serializedObject.targetObjects;
            for (var i = 0; i < targetObjects.Length; i++) {
                var @object = targetObjects[i];
                var text = @object as RoomfulText;
                var horizontalAlignment = GetHorizontalAlignment(text.FontData.Alignment);
                Undo.RecordObject(text, "Vertical Alignment");
                text.FontData.Alignment = GetAnchor(verticalAlignment, horizontalAlignment);
                EditorUtility.SetDirty(@object);
            }
        }

        private static TextAnchor GetAnchor(VerticalTextAligment verticalTextAligment, HorizontalTextAligment horizontalTextAligment) {
            TextAnchor result;
            if (horizontalTextAligment != HorizontalTextAligment.Left) {
                if (horizontalTextAligment != HorizontalTextAligment.Center) {
                    if (verticalTextAligment != VerticalTextAligment.Bottom) {
                        if (verticalTextAligment != VerticalTextAligment.Middle) {
                            result = TextAnchor.UpperRight;
                        }
                        else {
                            result = TextAnchor.MiddleRight;
                        }
                    }
                    else {
                        result = TextAnchor.LowerRight;
                    }
                }
                else if (verticalTextAligment != VerticalTextAligment.Bottom) {
                    if (verticalTextAligment != VerticalTextAligment.Middle) {
                        result = TextAnchor.UpperCenter;
                    }
                    else {
                        result = TextAnchor.MiddleCenter;
                    }
                }
                else {
                    result = TextAnchor.LowerCenter;
                }
            }
            else if (verticalTextAligment != VerticalTextAligment.Bottom) {
                if (verticalTextAligment != VerticalTextAligment.Middle) {
                    result = TextAnchor.UpperLeft;
                }
                else {
                    result = TextAnchor.MiddleLeft;
                }
            }
            else {
                result = TextAnchor.LowerLeft;
            }

            return result;
        }
    }
}