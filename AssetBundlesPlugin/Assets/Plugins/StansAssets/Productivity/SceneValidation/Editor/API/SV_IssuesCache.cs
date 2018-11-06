using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace SA.Productivity.SceneValidator
{
    public class SV_IssuesCache 
    {


        private Dictionary<int, SV_ValidationState> m_issues = new Dictionary<int, SV_ValidationState>();
        private Dictionary<int, Dictionary<Component, Dictionary<SV_iValidationRule, SV_ValidationState>>> m_fullIssuesCache = new Dictionary<int, Dictionary<Component, Dictionary<SV_iValidationRule, SV_ValidationState>>>();



        private Dictionary<int, List<int>> m_parentsToIssuesMap = new Dictionary<int, List<int>>();
        private Dictionary<int, List<int>> m_issuesToParentsMap = new Dictionary<int, List<int>>();

       

        public SV_ValidationState GetGameobjectnValidationState(GameObject go) {
            return GetGameobjectnValidationState(go.GetInstanceID());
        }

        public SV_ValidationState GetGameobjectnValidationState(int instanceId) {
            if (m_issues.ContainsKey(instanceId)) {
                return m_issues[instanceId];
            } else {
                return SV_ValidationState.Ok;
            }
        }


        public SV_ValidationState GetChildrenValidationState(int parentInstanceId) {
            if (!m_parentsToIssuesMap.ContainsKey(parentInstanceId)) {
                return SV_ValidationState.Ok;
            }

            var childState = SV_ValidationState.Ok;
            List<int> childrenIssues = m_parentsToIssuesMap[parentInstanceId];
            foreach (var child in childrenIssues) {
                childState = GetGameobjectnValidationState(child);

                //Higher state, no point to further check
                if (childState == SV_ValidationState.Error) {
                    return SV_ValidationState.Error;
                }
            }

            return childState;
        }


        public void SetGameobjectIssues(GameObject go, Dictionary<Component, Dictionary<SV_iValidationRule, SV_ValidationState>> issues) {

            var state = SV_ValidationState.Ok;
            if (issues.Count != 0) {
                foreach(var pair in issues) {

                    if (state == SV_ValidationState.Error) {
                        break;
                    }

                    Dictionary<SV_iValidationRule, SV_ValidationState> rulesStates = pair.Value;
                    foreach(var statePair in rulesStates) {
                        if(state == SV_ValidationState.Error) {
                            break;
                        }
                        state = statePair.Value;
                    }
                }
            }


            int instanceId = go.GetInstanceID();
            if (state == SV_ValidationState.Ok) {
                if (m_issues.ContainsKey(instanceId)) {
                    RemoveIssueParents(instanceId);
                    m_issues.Remove(instanceId);
                    m_fullIssuesCache.Remove(instanceId);
                }
            } else {
                AddIssueParents(go);
                m_issues[instanceId] = state;
                m_fullIssuesCache[instanceId] = issues;
            }
        }


        public List<int> GetParentIssues(int parentInstanceId) {
            return m_parentsToIssuesMap[parentInstanceId];
        }

        public void Clear() {
            m_issues.Clear();
            m_fullIssuesCache.Clear();

            m_parentsToIssuesMap.Clear();
            m_issuesToParentsMap.Clear();
        }


        public void CleanUp() {
            var GameObjectInstancesWithIssues =  new List<int>(m_issues.Keys);
           
            foreach (int instanceId in GameObjectInstancesWithIssues) {
                var go = EditorUtility.InstanceIDToObject(instanceId);
                if (go == null) {
                    Remove(instanceId);
                }
            }
            
        }



        public Dictionary<int, Dictionary<Component, Dictionary<SV_iValidationRule, SV_ValidationState>>> FullIssuesCache {
            get {
                return m_fullIssuesCache;
            }
        }


        //--------------------------------------
        // Private Methods
        //--------------------------------------

        private void Remove(int instanceId) {
            m_issues.Remove(instanceId);
            m_fullIssuesCache.Remove(instanceId);
        }


        //--------------------------------------
        // Working With Issues Parents
        //--------------------------------------

        private void AddIssueParents(GameObject go) {

            var issueInstanceId = go.GetInstanceID();

            //First of all the object parent could have been changed,
            //so let's clean all the records for our guy
            RemoveIssueParents(issueInstanceId);


            var parents = GetParents(go);
            List<int> registredParents = new List<int>();
            ;
            foreach (var parent in parents) {
                var parentInstanceId = parent.GetInstanceID();
                registredParents.Add(parentInstanceId);

                List<int> issues;
                if (m_parentsToIssuesMap.ContainsKey(parentInstanceId)) {
                    issues = m_parentsToIssuesMap[parentInstanceId];
                } else {
                    issues = new List<int>();
                    m_parentsToIssuesMap[parentInstanceId] = issues;
                }
                if (!issues.Contains(issueInstanceId)) {
                    issues.Add(issueInstanceId);
                }
            }

            m_issuesToParentsMap[issueInstanceId] = registredParents;
        }

        private void RemoveIssueParents(GameObject go) {
            RemoveIssueParents(go.GetInstanceID());
        }

        private void RemoveIssueParents(int issueInstanceId) {
            if (!m_issuesToParentsMap.ContainsKey(issueInstanceId)) {
                return;
            }

            List<int> prents = m_issuesToParentsMap[issueInstanceId];
            foreach (int parentInstanceId in prents) {
                List<int> issues = m_parentsToIssuesMap[parentInstanceId];
                if (issues != null) {
                    issues.Remove(issueInstanceId);
                }
            }
            m_issuesToParentsMap.Remove(issueInstanceId);
        }


        private List<GameObject> GetParents(GameObject go) {
            var result = new List<GameObject>();
            var transform = go.transform.parent;
            while (transform != null) {

                result.Add(transform.gameObject);
                transform = transform.parent;
            }

            return result;
        }









    }
}