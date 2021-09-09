using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SA.Productivity.GoogleSheets
{
    public class GD_LabelLocalizer : MonoBehaviour
    {
        /// <summary>
        /// Localization token
        /// </summary>
        [SerializeField] private string m_token = "token";

        /// <summary>
        /// Localization section
        /// </summary>
        [SerializeField] private GD_LangSection m_section = default;
        [SerializeField] private GD_TextType m_textType = default;
        [SerializeField] private string m_prefix = default;
        [SerializeField] private string m_suffix = default;

        private Text m_localizableLabel;
        private TMP_Text m_localizableLabelTmp;

        void Start() {
            m_localizableLabelTmp = GetComponent<TMP_Text>();
            if (m_localizableLabelTmp == null) {
                m_localizableLabelTmp = GetComponentInChildren<TMP_Text>();
                if (m_localizableLabelTmp == null) {
                    m_localizableLabel = GetComponent<Text>();
                    if (m_localizableLabel == null) {
                        m_localizableLabel = GetComponentInChildren<Text>();
                    }
                }
            }
            UpdateLabel();
            GD_Localization.OnLanguageChanged += UpdateLabel;
        }

        void OnDestroy() {
            GD_Localization.OnLanguageChanged -= UpdateLabel;
        }

        private void UpdateLabel() {
            if (m_localizableLabelTmp != null) {
                m_localizableLabelTmp.text = GetLocalizedString();
                return;
            }
            if (m_localizableLabelTmp != null) {
                m_localizableLabelTmp.text = GetLocalizedString();
                return;
            }
        }

        private string GetLocalizedString() {
            return string.Format("{0}{1}{2}", m_prefix, GD_Localization.GetLocalizedString(m_token, m_textType, m_section), m_suffix);
        }
    }
}
