using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace SA.Foundation.Tests
{
    public class SA_TestResultUI : MonoBehaviour
    {
        [SerializeField] private Text m_text;

        private void Awake() {
            SA_TestsManager.OnGroupTestStarted += GroupTestStartedHandler;
            SA_TestsManager.OnTestStarted += TestStartedHandler;
            SA_TestsManager.OnTestResult += TestResultHandler;
        }

        private void TestStartedHandler(string obj) {
            m_text.text += obj + "...";
        }

        private void GroupTestStartedHandler(string obj) {
            m_text.text += obj + " started tests\n";
        }

        private void TestResultHandler(SA_TestResult result) {
            var color = result.IsFailed ? "red" : "green";
            m_text.text += string.Format("<color=\"{0}\">{1}</color>\n", color, result.Message);
            Debug.Log(result.Message);
        }
    }
}
