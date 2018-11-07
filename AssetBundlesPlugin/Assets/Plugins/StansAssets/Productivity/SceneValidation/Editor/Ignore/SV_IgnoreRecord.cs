using System;
using System.Collections.Generic;
using UnityEngine;


namespace SA.Productivity.SceneValidator
{

    [Serializable]
    public class SV_IgnoreRecord
    {

        [SerializeField] int m_fileId;
        [SerializeField] string m_typeName;



        public SV_IgnoreRecord(int fileId, string typeName) {
            m_fileId = fileId;
            m_typeName = typeName;
        }



        public int FileId {
            get {
                return m_fileId;
            }
        }



        public string TypeName {
            get {
                return m_typeName;
            }
        }
    }
}