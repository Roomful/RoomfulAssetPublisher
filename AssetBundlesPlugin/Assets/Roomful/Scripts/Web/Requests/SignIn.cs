using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace RF.AssetWizzard.Network.Request {
	public class Signin : BaseWebPackage  {

		public const string PackUrl = "/auth/signin";

		private string Mail = string.Empty;
		private string Password = string.Empty;

		public Signin(string mail, string password):base(PackUrl) {
			Mail = mail;
			Password = password;
		}

		public override bool AuthenticationRequired {
			get {
				return false;
			}
		}

		public override Dictionary<string, object> GenerateData () {
			Dictionary<string, object> OriginalJSON =  new Dictionary<string, object>();

			OriginalJSON.Add("email", Mail);
			OriginalJSON.Add("password", Password);

			return OriginalJSON;
		}
	}
}