using UnityEngine;
using UnityEditor;

namespace net.roomful.assets.editor
{
    internal static class WindowManager
    {
        public static void ShowWizard() {
            Wizzard.minSize = new Vector2(700f, 450);
            Wizzard.maxSize = new Vector2(Wizzard.maxSize.x, Wizzard.maxSize.y);
            Wizzard.position = new Rect(new Vector2(100f, 100f), Wizzard.minSize);
            Wizzard.Show();
        }

        public static void ShowCreateNewProp() {
            EditorWindow window = EditorWindow.GetWindow<CreateAssetWindow>(true, "New Prop");
            ShowModal(window);
        }

        public static void ShowCreateNewStyle() {
            EditorWindow window = EditorWindow.GetWindow<CreateStyleWindow>(true, "New Style");
            ShowModal(window);
        }

        public static void ShowCreateNewEnvironment() {
            EditorWindow window = EditorWindow.GetWindow<CreateEnvironmentWindow>(true, "New Environment");
            ShowModal(window);
        }

        private static void ShowModal(EditorWindow window) {
            window.minSize = new Vector2(375f, 135f);
            window.maxSize = new Vector2(window.minSize.x, window.maxSize.y);
            window.position = new Rect(new Vector2(Wizzard.position.x + 100f, Wizzard.position.y + 100f), window.minSize);
            window.Focus();

            window.Show();
        }

        public static WizardWindow Wizzard => EditorWindow.GetWindow<WizardWindow>(false, "Roomful - Asset Publisher");
    }
}