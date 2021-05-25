using System.Collections.Generic;

// Copyright Roomful 2013-2020. All rights reserved.

namespace net.roomful.api {

	public class PermissionTemplate : IPermissionsTemplate {

		private bool m_permissionComment = true;
		private bool m_permissionContribute;
		private bool m_permissionEdit;
		private bool m_permissionManage;
        private bool m_permissionView = true;

        public PermissionTemplate(bool allStates) {
			m_permissionComment = allStates;
			m_permissionContribute = allStates;
			m_permissionEdit = allStates;
			m_permissionManage = allStates;
            m_permissionView = allStates;
        }

		public PermissionTemplate(JSONData permissionsData) {
            if (permissionsData.HasValue("comment")) {
				m_permissionComment = permissionsData.GetValue<bool>("comment");
			}
			if (permissionsData.HasValue("contribute")) {
				m_permissionContribute = permissionsData.GetValue<bool>("contribute");
			}
			if (permissionsData.HasValue("edit")) {
				m_permissionEdit = permissionsData.GetValue<bool>("edit");
			}
			if (permissionsData.HasValue("manage")) {
				m_permissionManage = permissionsData.GetValue<bool>("manage");
			}
            if (permissionsData.HasValue("view")) {
                m_permissionView = permissionsData.GetValue<bool>("view");
            }
        }

		public Dictionary<string, object> ToDictionary() {
			return new Dictionary<string, object> {
				{ "comment", m_permissionComment },
				{ "contribute", m_permissionContribute },
				{ "edit", m_permissionEdit },
				{ "manage", m_permissionManage },
				{ "view", m_permissionView }
			};
		}

		public void ResetDefault() {
			m_permissionComment = true;
			m_permissionContribute = false;
			m_permissionEdit = false;
			m_permissionManage = false;
            m_permissionView = true;
        }

		public bool PermissionComment {
			get => m_permissionComment;
			set => m_permissionComment = value;
		}

		public bool PermissionContribute {
			get => m_permissionContribute;
			set => m_permissionContribute = value;
		}

		public bool PermissionEdit {
			get => m_permissionEdit;
			set => m_permissionEdit = value;
		}

		public bool PermissionManage {
			get => m_permissionManage;
			set => m_permissionManage = value;
		}

        public bool PermissionView {
	        get => m_permissionView;
	        set => m_permissionView = value;
        }

		public override string ToString() {
			return "View: " + m_permissionView + ", Comments: " + m_permissionComment + ", Contribute: " + m_permissionContribute + ", Edit: " + m_permissionEdit +
					", Manage: " + m_permissionManage;
		}

		public Permission Permission {
			get {
				if (PermissionManage) {
					return Permission.Own;
				}

				if (PermissionEdit) {
					return Permission.Edit;
				}

				if (PermissionContribute) {
					return Permission.Contribute;
				}

				if (PermissionComment) {
					return Permission.Comment;
				}

				return Permission.View;
			}
            set {
                switch (value) {
                    case Permission.Comment:
                        PermissionComment = true;
                        PermissionContribute = false;
                        PermissionEdit = false;
                        PermissionManage = false;
                        break;
                    case Permission.Contribute:
                        PermissionComment = false;
                        PermissionContribute = true;
                        PermissionEdit = false;
                        PermissionManage = false;
                        break;
                    case Permission.Edit:
                        PermissionComment = false;
                        PermissionContribute = false;
                        PermissionEdit = true;
                        PermissionManage = false;
                        break;
                    case Permission.Own:
                        PermissionComment = false;
                        PermissionContribute = false;
                        PermissionEdit = false;
                        PermissionManage = true;
                        break;
                    default:
                        PermissionComment = false;
                        PermissionContribute = false;
                        PermissionEdit = false;
                        PermissionManage = false;
                        PermissionView = true;
                        break;
                }
            }
		}
	}
}
