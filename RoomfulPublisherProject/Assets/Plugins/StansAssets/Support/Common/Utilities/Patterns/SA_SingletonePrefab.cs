////////////////////////////////////////////////////////////////////////////////
//  
// @module Assets Common Lib
// @author Osipov Stanislav (Stan's Assets) 
// @support support@stansassets.com
// @website https://stansassets.com
//
////////////////////////////////////////////////////////////////////////////////

using UnityEngine;



namespace SA.Common.Pattern
{
  
    public abstract class SingletonePrefab<T> : MonoBehaviour where T : MonoBehaviour
    {
        protected static T _Instance = null;

        public static T Instance {

            get {
                if (_Instance == null) {
                    _Instance = FindObjectOfType(typeof(T)) as T;
                    if (_Instance == null) {


                        GameObject prefab = Object.Instantiate(Resources.Load(typeof(T).Name)) as GameObject;
                        prefab.name = typeof(T).Name;
                        _Instance = prefab.GetComponent<T>();
                        DontDestroyOnLoad(prefab);
                    }
                }

                return _Instance;
            }
        }

        public static bool HasInstance {
            get {
                return _Instance != null;
            }
        }
    }
    
}