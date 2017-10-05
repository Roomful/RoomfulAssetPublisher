////////////////////////////////////////////////////////////////////////////////
//  
// @module Assets Common Lib
// @author Osipov Stanislav (Stan's Assets) 
// @support support@stansassets.com
// @website https://stansassets.com
//
////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;

namespace SA.Common.Pattern {

	public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour {

		private static T s_instance = null;
		private static bool s_applicationIsQuitting = false;


		public static T Instance {
			get {
				if(s_applicationIsQuitting) {
					//Debug.Log(typeof(T) + " [Mog.Singleton] is already destroyed. Returning null. Please check HasInstance first before accessing instance in destructor.");
					return null;
				}

				if (s_instance == null) {
					s_instance = GameObject.FindObjectOfType(typeof(T)) as T;
					if (s_instance == null) {
						s_instance = new GameObject ().AddComponent<T> ();
						s_instance.gameObject.name = s_instance.GetType ().FullName;
					}
				}
				return s_instance;
			}
		}


        public virtual void Awake() {
            CompleteInitialization();
        }

        public virtual void OnEnable() {
            CompleteInitialization();
        }



 


        public static bool HasInstance {
			get {
				return !IsDestroyed;
			}
		}

		public static bool IsDestroyed {
			get {
				if(s_instance == null) {
					return true;
				} else {
					return false;
				}
			}
		}


        private void CompleteInitialization() {

            if (s_instance) {
                if (s_instance != this) {
                    Destroy(gameObject);
                    return;
                }
            } 
            s_instance = this as T;
            gameObject.transform.parent = SingletonsParent;
        }

        private static Transform s_singletonsParent = null;
        private static Transform SingletonsParent {
            get {
                if(s_singletonsParent == null) {
                    var go = new GameObject("Singletons");
                    s_singletonsParent = go.transform;
                    GameObject.DontDestroyOnLoad(go);
                }

                return s_singletonsParent;
            }
        }

        /// <summary>
        /// When Unity quits, it destroys objects in a random order.
        /// In principle, a Singleton is only destroyed when application quits.
        /// If any script calls Instance after it have been destroyed, 
        ///   it will create a buggy ghost object that will stay on the Editor scene
        ///   even after stopping playing the Application. Really bad!
        /// So, this was made to be sure we're not creating that buggy ghost object.
        /// </summary>
        protected virtual void OnDestroy () {
			s_instance = null;
			s_applicationIsQuitting = true;
			//Debug.Log(typeof(T) + " [Mog.Singleton] instance destroyed with the OnDestroy event");
		}
		
		protected virtual void OnApplicationQuit () {
			s_instance = null;
			s_applicationIsQuitting = true;
			//Debug.Log(typeof(T) + " [Mog.Singleton] instance destroyed with the OnApplicationQuit event");
		}

	}

}
