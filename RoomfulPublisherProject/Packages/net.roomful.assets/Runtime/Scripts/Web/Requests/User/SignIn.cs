using System.Collections.Generic;

namespace net.roomful.assets {
	class Signin : BaseWebPackage
	{
		public override bool ShouldDisplayAnErrorPopup => false;

		public const string PackUrl = "/auth/signin";

		readonly string m_Mail;
		readonly string m_Password;

		public Signin(string mail, string password):base(PackUrl) {
			m_Mail = mail;
			m_Password = password;
		}

		public override bool AuthenticationRequired => false;

		public override Dictionary<string, object> GenerateData () {
			var originalJson =  new Dictionary<string, object>
			{
				{ "email", m_Mail },
				{ "password", m_Password }
			};

			return originalJson;
		}
	}
}