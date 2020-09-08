using System.Collections.Generic;

namespace net.roomful.assets.Network.Request {
	public class Signin : BaseWebPackage  {

		public const string PackUrl = "/auth/signin";

		private readonly string Mail = string.Empty;
		private readonly string Password = string.Empty;

		public Signin(string mail, string password):base(PackUrl) {
			Mail = mail;
			Password = password;
		}

		public override bool AuthenticationRequired => false;

		public override Dictionary<string, object> GenerateData () {
			var OriginalJSON =  new Dictionary<string, object>();

			OriginalJSON.Add("email", Mail);
			OriginalJSON.Add("password", Password);

			return OriginalJSON;
		}
	}
}