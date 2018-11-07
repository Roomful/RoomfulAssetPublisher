using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace SA.Productivity.SceneValidator
{

    [Serializable]
    public class SV_IgnoreList 
    {

        [SerializeField] List<SV_IgnoreRecord> m_records = new List<SV_IgnoreRecord>();
        private Dictionary<int, List<SV_IgnoreRecord>> m_recodsCache = new Dictionary<int, List<SV_IgnoreRecord>>();


        public void Cache() {
            m_recodsCache = new Dictionary<int, List<SV_IgnoreRecord>>();
            foreach(var record in m_records) {
                CacheRecord(record);
            }
        }

        public bool IsRuleIgnored(int fileId, SV_iValidationRule rule) {
            if(m_recodsCache.ContainsKey(fileId)) {
                List<SV_IgnoreRecord> records  = m_recodsCache[fileId];
                foreach(var record in records) {
                    if(record.TypeName.Equals(rule.GetType().Name)) {
                        return true;
                    }
                }
            } 

            return false;
        }

        public void AddRecord(SV_IgnoreRecord record) {
            m_records.Add(record);
            CacheRecord(record);
        }


        private void CacheRecord(SV_IgnoreRecord record) {
            List<SV_IgnoreRecord> records;
            if (m_recodsCache.ContainsKey(record.FileId)) {
                records = m_recodsCache[record.FileId];
            } else {
                records = new List<SV_IgnoreRecord>();
                m_recodsCache.Add(record.FileId, records);
            }

            records.Add(record);
        }



    }
}