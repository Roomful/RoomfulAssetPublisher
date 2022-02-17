using System;
using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace net.roomful.api
{
    [RequireComponent(typeof(TMP_InputField))]
    public class TMPInputFieldHotkeysSupport : MonoBehaviour, ISelectHandler, IDeselectHandler
    {
        [SerializeField] private TMP_InputField m_inputField;
        [SerializeField] private bool m_refocusInputOnEnter = false;
        [SerializeField] private bool m_lockRoomfulInputWhenActive = true;

        private Coroutine longPressCoroutine = null;
        private string selectedText = string.Empty;
        private RectTransform rectTransform;
        private readonly Stack<string> m_valuesHistory = new Stack<string>();

        private void Awake() {
            if (m_inputField == null) {
                m_inputField = GetComponent<TMP_InputField>();
                if (m_inputField == null) {
                    Debug.LogWarning("WebglHotkeysTMProHandler is missing TMP_InputField component");
                    enabled = false;
                    Destroy(this);
                    return;
                }
            }

            if (m_inputField.textViewport == null) {
                m_inputField.textViewport = m_inputField.textComponent.GetComponent<RectTransform>();
            }
            
            m_inputField.onSelect.AddListener(e => {
                SetInputLock(true);
            });

            m_inputField.onDeselect.AddListener(e => {
                if (!m_refocusInputOnEnter) {
                    SetInputLock(false);
                }
            });

            m_inputField.onEndEdit.AddListener(e => {
                if (!m_refocusInputOnEnter) {
                    SetInputLock(false);
                }
            });

            m_inputField.onSubmit.AddListener(e => {
                var isMobile = Roomful.Platform == RoomfulPlatform.Mobile;
                if (m_refocusInputOnEnter && !isMobile) {
                    m_inputField.ActivateInputField();
                    m_inputField.Select();
                }
                else {
                    SetInputLock(false);
                }
            });
            rectTransform = GetComponent<RectTransform>();
            m_inputField.shouldHideMobileInput = true;
            if (Roomful.Platform == RoomfulPlatform.Mobile) {
                m_inputField.lineType = TMP_InputField.LineType.SingleLine;
            }
        }

        public void OnSelect(UnityEngine.EventSystems.BaseEventData eventData) {
            if (eventData == null || eventData.selectedObject != gameObject) {
                return;
            }
            m_valuesHistory.Push(m_inputField.text);
            m_inputField.onValueChanged.AddListener(ValueChangeHandler);

            if (m_inputField.lineType == TMP_InputField.LineType.MultiLineNewline) {
                m_inputField.textComponent.overflowMode = TextOverflowModes.Overflow;
            }
            
        }

        public void OnDeselect(UnityEngine.EventSystems.BaseEventData eventData) {
            if (eventData == null || eventData.selectedObject != gameObject) {
                return;
            }

            m_valuesHistory.Clear();
            m_inputField.onValueChanged.RemoveListener(ValueChangeHandler);
        }

        private void SetInputLock(bool locked) {
            if(Roomful.Input != null && m_lockRoomfulInputWhenActive)
                Roomful.Input.SetKeyBoardInputLock(locked);
        }

        private void ValueChangeHandler(string newText) {
            m_valuesHistory.Push(newText);
        }

        private void Update() {
            if (m_inputField.isFocused) {
                if (Roomful.Platform == RoomfulPlatform.Mobile) {
                    TouchRecognizer();
                }
                if (IsControlHold()) {
                    if (Input.GetKeyDown(KeyCode.Z)) {
                        UndoLastAction();
                    }

                    if (Input.GetKeyDown(KeyCode.V)) {
                        PasteTextFromClipboard();
                    }

                    if (Input.GetKeyDown(KeyCode.C)) {
                        CopyTextToClipboard();
                    }

                    if (Input.GetKeyDown(KeyCode.X)) {
                        CutTextToClipboard();
                    }
                }
            }
        }

        private IEnumerator LongPressCoroutine() {
            yield return new WaitForSeconds(0.6f);
            if (longPressCoroutine != null) {
                m_inputField.interactable = false;
                // In order to fix problem with chat resize so we can get real position of our input.
                yield return new WaitForEndOfFrame();
                yield return new WaitForEndOfFrame();
                yield return new WaitForEndOfFrame();
                selectedText = GetSelectedText();
                if (string.IsNullOrEmpty(selectedText)) {
                    selectedText = m_inputField.text;
                }
                var rect = StansAssets.Foundation.RectTransformUtility.RectTransformToScreenSpace(rectTransform);
                var menu = new CopyPasteMenu(selectedText, rect.center.x, rect.center.y);
                menu.Show(CopyPasteMenuHandle);
                longPressCoroutine = null;
            }
        }

        private void TouchRecognizer() {
            if (Input.touchCount > 0) {
                if (longPressCoroutine == null) {
                    longPressCoroutine = StartCoroutine(LongPressCoroutine());
                }
                if (Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetTouch(0).phase == TouchPhase.Canceled) {
                    longPressCoroutine = null;
                }
            } else {
                longPressCoroutine = null;
            }
        }
        
        
        private void CopyPasteMenuHandle(CopyPasteMenuResult result) {
            switch (result.Item) {
                case CopyPasteMenuItem.Copy:
                    GUIUtility.systemCopyBuffer = selectedText;
                    break;
                case CopyPasteMenuItem.Cut:
                    GUIUtility.systemCopyBuffer = selectedText;
                    selectedText = string.Empty;
                    m_inputField.text = selectedText;
                    break;
                case CopyPasteMenuItem.Paste:
                    var text = GUIUtility.systemCopyBuffer;
                    if (!string.IsNullOrEmpty(text)) {
                        PasteText(text);
                    }
                    break;
                case CopyPasteMenuItem.Dismiss:
                    m_inputField.interactable = true;
                    m_inputField.ActivateInputField();
                    m_inputField.Select();
                    break;
            }
#if UNITY_ANDROID
            if (result.Item != CopyPasteMenuItem.Dismiss) {
                m_inputField.interactable = true;
                m_inputField.ActivateInputField();
                m_inputField.Select();
            }
#endif                
        }

        private void CutTextToClipboard() {
            var selected = GUIUtility.systemCopyBuffer;
            if (selected.Length == 0) {
                return;
            }
            Roomful.Native.CopyToClipboard(selected);
            GUIUtility.systemCopyBuffer = string.Empty;
        }

        private void CopyTextToClipboard() {
            var selectionStart = Math.Min(m_inputField.selectionAnchorPosition, m_inputField.selectionFocusPosition);
            var selectionFinish = Math.Max(m_inputField.selectionAnchorPosition, m_inputField.selectionFocusPosition);
            var selected = GetSelectedText();
            if (selected.Length == 0) {
                Console.WriteLine("Copy text: selected.Length == 0");
                return;
            }

            Roomful.Native.CopyToClipboard(selected);
            GUIUtility.systemCopyBuffer = string.Empty;
        }

        private string GetSelectedText() {
            var selectionStart = Math.Min(m_inputField.selectionAnchorPosition, m_inputField.selectionFocusPosition);
            var selectionFinish = Math.Max(m_inputField.selectionAnchorPosition, m_inputField.selectionFocusPosition);
            return m_inputField.text.Substring(selectionStart, selectionFinish - selectionStart);
        }

        private void PasteTextFromClipboard() {
            Roomful.Native.GetFromClipboard(textToPaste => {
                if (this && m_inputField.isFocused) {
                    PasteText(textToPaste);
                }
            });
        }

        private void PasteText(string textToPaste) {
            var selectionStart = Math.Min(m_inputField.selectionAnchorPosition, m_inputField.selectionFocusPosition);
            var selectionFinish = Math.Max(m_inputField.selectionAnchorPosition, m_inputField.selectionFocusPosition);
            m_inputField.text = m_inputField.text.Substring(0, selectionStart) + textToPaste + m_inputField.text.Substring(selectionFinish, m_inputField.text.Length - selectionFinish);
            m_inputField.selectionAnchorPosition = selectionStart + textToPaste.Length;
            m_inputField.selectionFocusPosition = m_inputField.selectionAnchorPosition;
            
        }

        private bool IsControlHold() {
            if (Application.isEditor) {
                return Input.GetKey(KeyCode.RightAlt) || Input.GetKey(KeyCode.LeftAlt);
            }

            if (Roomful.Platform == RoomfulPlatform.Web) {
                if (Roomful.Native.GetWebglPlatform() == WebglPlatform.MacOS) {
                    return Input.GetKey(KeyCode.RightCommand) || Input.GetKey(KeyCode.LeftCommand);
                }

                return Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.LeftControl);
            }
            
            return false;
        }

        private void UndoLastAction() {
            if (m_valuesHistory.Count == 0) {
                return;
            }

            m_inputField.onValueChanged.RemoveListener(ValueChangeHandler);
            m_inputField.text = m_valuesHistory.Pop();
            m_inputField.onValueChanged.AddListener(ValueChangeHandler);
        }
    }
}
