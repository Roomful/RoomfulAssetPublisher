using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RF.AssetWizzard {
	public class AssetTemplate {
		
		private string _Id = string.Empty;
		private string _AssetName = string.Empty;
		private string _Placing = string.Empty;
		private string _InvokeType = string.Empty;
		private string _Thumbnail = string.Empty;
		private float _MinScale = 0.5f;
		private float _MaxScale = 2f;

		public AssetTemplate() {

		}

		public string Id {
			get {
				return _Id;
			}
			set {
				_Id = value;
			}
		}

		public string AssetName {
			get {
				return _AssetName;
			}
			set {
				_AssetName = value;
			}
		}

		public string Placing {
			get {
				return _Placing;
			}
			set {
				_Placing = value;
			}
		}

		public string InvokeType {
			get {
				return _InvokeType;
			}
			set {
				_InvokeType = value;
			}
		}

		public string Thumbnail {
			get {
				return _Thumbnail;
			}
			set {
				_Thumbnail = value;
			}
		}

		public float MinScale {
			get {
				return _MinScale;
			}
			set {
				_MinScale = value;
			}
		}

		public float MaxScale {
			get {
				return _MaxScale;
			}
			set {
				_MaxScale = value;
			}
		}
	}
}