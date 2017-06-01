using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RF.AssetWizzard.Network.Request {
	public class DownloadIcon : BaseWebPackage {

		private const string PackUrl = "/api/v0/resource/";
		private const RequestMethods PackMethodName = RequestMethods.GET;



		public DownloadIcon (Resource res) : base (PackUrl, PackMethodName) {
			AddToUrl (res.Id);
		}



		public override Dictionary<string, object> GenerateData () {
			Dictionary<string, object> OriginalJSON =  new Dictionary<string, object>();

			return OriginalJSON;
		}

		public override bool IsDataPack {
			get {
				return true;
			}
		}

		public override void Send() {
			_Url = "https://storage.googleapis.com/roomful-templates/assets/fc0xwxmxpnz80v/wc762k5xbs4772/2fhpcxhkzbfcrs?Expires=1496399029&GoogleAccessId=storage1%40serious-amulet-131413.iam.gserviceaccount.com&Signature=m%2FPe0A44SJvqvt91TzQrIwhQ8RqOBw%2FQS1JYvVANmJASoUG3WOX7uHHpx48WzPmMcmPNTzUlfEPCJ4njhA%2F%2ByqWHGaAEonZTk39PpXXCDE3JUWsluO5dkEbpo9JHYZDibK2UlrKQqpen4JMcPUrteVACTgbbZbmU8zyDNfIbn0Buj2Ceo3FAX8Xn%2FCg1AcE7IdXu9lGiSuUwZuYzHe8eRrBxwRxvb6vGfUfi4WRODUYJf9Yr6gaUAhzIu19uZmIXoeJijxxddCf4jsXOm8iztlWzt%2F4xBrg%2Fq0a%2Fgmgjk0Dc0hQjoOXPEWOaawcgal3vBFvkYKkCLyvaAfgcmhIxGA%3D%3D";
			//Debug.Log (AssetBundlesSettings.WEB_SERVER_URL + Url);
			base.Send ();
		}
	}
}