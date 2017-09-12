
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Moon.Network.Web
{


	public static class MainThreadDispatcher {

		public static void Init() {
			#if !UNITY_EDITOR
			MainThreadDispatcherPlaymode.Instance.Init ();
			#endif
		}

		public static void Enqueue(Action action) {

			#if UNITY_EDITOR
			MainThreadDispatcherEdior.Enqueue (action);
			//MainThreadDispatcherPlaymode.Instance.Enqueue (action);
			#else
			MainThreadDispatcherPlaymode.Instance.Enqueue (action);
			#endif
		
		}
	}



	#if UNITY_EDITOR

	public static class MainThreadDispatcherEdior {

		private static readonly Queue<Action> _executionQueue = new Queue<Action>();

		static MainThreadDispatcherEdior() {
			EditorApplication.update += Update;
		}
			

		private static void Update () {
			lock (_executionQueue) {
				while (_executionQueue.Count > 0) {
					_executionQueue.Dequeue().Invoke();
				}
			}
		}

		public static void Enqueue(Action action) {
			_executionQueue.Enqueue (action);
		}
	}

	#endif

	//For Play Mode
	public class MainThreadDispatcherPlaymode : MonoBehaviour  {

        private static readonly Queue<Action> _executionQueue = new Queue<Action>();


		private static MainThreadDispatcherPlaymode s_instance = null;
		public static MainThreadDispatcherPlaymode Instance {
			get {
				if (s_instance == null) {
					s_instance = GameObject.FindObjectOfType<MainThreadDispatcherPlaymode> ();
					if (s_instance == null) {
						s_instance = new GameObject ("MainThreadDispatcherPlaymode").AddComponent<MainThreadDispatcherPlaymode> ();
					}
				}
				return s_instance;
			}
		}

		void Awake() {
			DontDestroyOnLoad(gameObject);
		}

		public void Init() {
			
		}



        public void Update() {
            lock (_executionQueue) {
                while (_executionQueue.Count > 0) {
                    _executionQueue.Dequeue().Invoke();
                }
            }
        }
			

        public void Enqueue(Action action) {
			_executionQueue.Enqueue (action);
        }



    }


}