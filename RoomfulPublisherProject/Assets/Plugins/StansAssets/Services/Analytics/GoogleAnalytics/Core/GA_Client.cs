////////////////////////////////////////////////////////////////////////////////
//
// @module Google Analytics Plugin
// @author Osipov Stanislav (Stan's Assets)
// @support support@stansassets.com
// @website https://stansassets.com
//
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Text;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

namespace SA.Analytics.Google {


	public class GA_Client  {

		private const string PROTOCOL_VERSION = "v=1";

#if UNITY_WEBGL
		private const string HTTP_URL = "https://ssl.google-analytics.com/collect";
#else
		private const string HTTP_URL = "http://www.google-analytics.com/collect";
#endif
		private const string HTTPS_URL = "https://ssl.google-analytics.com/collect";

		public string TrackingID;
		public string ClientID;
		public string AppName;
		public string AppVersion;



#if UNITY_IPHONE || UNITY_WP8 || UNITY_METRO
		private  Dictionary<string, string> Headers = new Dictionary<string, string>();
#endif

		private string DefaultHitData;
		private StringBuilder builder = new StringBuilder(256, 8192);
		private GA_HitType currentHitType;
		private string DataSendUrl;




		public GA_Client(string anonymousClientID)  {



			ClientID = EscapeString(anonymousClientID, true);

			AppName 		= EscapeString(GA_Settings.Instance.AppName, true);
			AppVersion 		= EscapeString(GA_Settings.Instance.AppVersion, true);


			if(GA_Settings.Instance.UseHTTPS) {
				DataSendUrl = HTTPS_URL;
			} else {
				DataSendUrl = HTTP_URL;
			}


			GenerateHeaders(GA_Settings.Instance.GetCurentProfile().TrackingID);
		}

		public void GenerateHeaders(string trackingId) {

			TrackingID = EscapeString(trackingId);
			builder.Length = 0;


			builder.Append(PROTOCOL_VERSION);
			builder.Append("&tid=");
			builder.Append(TrackingID);
			builder.Append("&cid=");
			builder.Append(ClientID);


			builder.Append("&an=");
			builder.Append(AppName);
			builder.Append("&av=");
			builder.Append(AppVersion);



			DefaultHitData = builder.ToString();
			builder.Length = 0;



#if UNITY_IPHONE


			if(Application.platform == RuntimePlatform.IPhonePlayer) {

				string model = SystemInfo.deviceModel;
				string opSys = SystemInfo.operatingSystem;
				string deviceType = "iPhone";
				string osVersion = "7.0.1";
				if (model.StartsWith("iPhone")) {
					deviceType = "iPhone";
					osVersion = opSys.Replace('.', '_');
				} else if (model.StartsWith("iPad")) {
					deviceType = "iPad";
					osVersion = opSys.Substring(opSys.IndexOf("OS")).Replace('.', '_');
				} else if (model.StartsWith("iPod")) {
					deviceType = "iPod";
					osVersion = opSys.Replace('.', '_');
				}


				builder.Append("Mozilla/5.0 (");
				builder.Append(deviceType);
				builder.Append("; CPU ");
				builder.Append(osVersion);
				builder.Append(" like Mac OS X)");

				string userAgent = builder.ToString();
				builder.Length = 0;
				//string Escape(string s)
				//{
					userAgent = userAgent.Replace("(", "&#40");
					userAgent = userAgent.Replace(")", "&#41");
				//}
				Headers.Add("User-Agent", userAgent);

			}

#endif


#if UNITY_WP8 || UNITY_METRO


			//"Mozilla/5.0 (compatible; MSIE 10.0; Windows Phone 8.0; Trident/6.0; IEMobile/10.0; ARM; Touch; NOKIA; Lumia 920)"
			// Mozilla/5.0 (compatible; MSIE 10.0; Windows Phone 8.0; Trident/6.0; IEMobile/10.0; ARM; Touch; NOKIA; Lumia 920)
			// Mozilla/5.0 (compatible; MSIE 10.0; Windows Phone 8.0; Trident/6.0; ARM; Touch; IEMobile/10.0; <Manufacturer>; <Device> [;<Operator>])


			string model = SystemInfo.deviceModel;
			string deviceType = "compatible";
			string osVersion = SystemInfo.operatingSystem; //"Windows Phone 8.0";


			builder.Append("Mozilla/5.0 (");
			builder.Append(deviceType);
			builder.Append("; MSIE 10.0; ");
			builder.Append(osVersion);
			builder.Append("; Trident/6.0; IEMobile/10.0; ARM; Touch; " + model + " )");

			string userAgent = builder.ToString();
			builder.Length = 0;



			Headers.Add("User-Agent", userAgent);

#endif
		}




		//--------------------------------------
		//  GENERAL PROTOCOL
		//--------------------------------------

		//When present, the IP address of the sender will be anonymized
		public void SetAnonymizeIP() {
			builder.Append("&aip=0");
		}

		public void SetQueueTime(int time) {
			builder.Append("&qt=");
			builder.Append(time);
		}


		//--------------------------------------
		//  Session
		//--------------------------------------

		public void StartSession() {
			builder.Append("&sc=start");
		}

		public void EndSession() {
			builder.Append("&sc=end");
		}

		public void IPOverride (string ip) {
			AppendData("uip", ip);
		}

		public void UserAgentOverride (string userAgent) {
			AppendData("ua", userAgent);
		}



		//--------------------------------------
		//  Traffic Sources
		//--------------------------------------


		public void SetDocumentReferrer(string url) {
			AppendData("dr", url, "Document Referrer", 2048);
		}


		public void SetCampaignName(string name) {
			AppendData("cn", name, "Campaign Name", 100);
		}

		public void SetCampaignSource(string source) {
			AppendData("cs", source, "Campaign Source", 100);
		}

		public void SetCampaignMedium(string medium) {
			AppendData("cm", medium, "Campaign Medium", 50);
		}

		public void AddCampaignKeyword(string keyword) {
			AppendData("ck", keyword, "Campaign Keyword", 500);
		}

		public void SetCampaignContent(string content) {
			AppendData("cc", content, "Campaign Content", 500);
		}


		public void SetCampaignID(string id) {
			AppendData("ci", id, "Campaign ID", 500);
		}

		public void SetGoogleAdWordsID(string id) {
			AppendData("gclid", id);
		}



		public void SetGoogleDisplayAdsID(string id) {
			AppendData("dclid", id);
		}



		//--------------------------------------
		//  SYSTEM INFO PROTOCOL
		//--------------------------------------

		public void SetScreenResolution(int width, int height) {
			builder.Append("&sr=");
			builder.Append(width);
			builder.Append('x');
			builder.Append(height);
		}

		public void SetViewportSize(int width, int height) {
			builder.Append("&vp=");
			builder.Append(width);
			builder.Append('x');
			builder.Append(height);
		}


		public void SetDocumentEncoding(string encoding) {
			AppendData("de", encoding);
		}


		public void SetScreenColors(string colorsBuffer) {
			AppendData("sd", colorsBuffer);
		}

		public void SetUserLanguage(string lang) {
			AppendData("ul", lang);
		}

		public void SetJavaEnablede(bool isEnabled) {
			string data = "0";
			if(isEnabled) {
				data = "1";
			}
			AppendData("je", data);
		}


		public void SetFlashVersion(string version) {
			AppendData("fl", version);
		}


		//--------------------------------------
		//  HIT PROTOCOL
		//--------------------------------------

		public void SetHitType(GA_HitType hit) {
			AppendData("t", hit.ToString().ToLower());
		}

		public void SetNoInteractionHit() {
			builder.Append("&ni=1");
		}


		//--------------------------------------
		//  CONTENT INFORMATION PROTOCOL
		//--------------------------------------

		public void SetDocumentlocationURL(string url) {
			AppendData("dl", url, "Document location URL", 2048);
		}


		public void SetDocumentHostName(string host) {
			AppendData("dh", host, "Document Host Name", 100);
		}

		public void SetDocumentPath(string path) {
			AppendData("dp", path, "Document Path", 2048);
		}

		public void SetDocumentTitle(string title) {
			AppendData("dt", title, "Document Title", 1500);
		}

		public void SetScreenName(string name) {
			AppendData("cd", name, "Screen Name", 2048);
		}

		public void SetLinkID(string id) {
			AppendData("linkid", id);
		}


		//--------------------------------------
		//  APP TRACKING PROTOCOL
		//--------------------------------------

		public void SetApplicationName(string name) {
			AppendData("an", name, "Application Name", 100);
		}

		public void SetApplicationVersion(string version) {
			AppendData("av", version, "Application Version", 100);
		}

		public void SetApplicationInstallerID(string identifier) {
			AppendData("aiid", identifier, "Application Installer ID", 150);
		}



		//--------------------------------------
		//  EVENT TRACKING PROTOCOL
		//--------------------------------------


		public void SetEventCategory(string ec) {
			AppendData("ec", ec, "Event Category", 150, GA_HitType.EVENT);
		}

		public void SetEventAction(string ea) {
			AppendData("ea", ea, "Event Action", 500, GA_HitType.EVENT);
		}

		public void SetEventLabel(string el) {
			AppendData("el", el, "Event Label", 500, GA_HitType.EVENT);
		}

		public void SetEventValue(int val) {
			AppendData("ev", val.ToString(), "", 0, GA_HitType.EVENT);
		}


		//--------------------------------------
		//  E-COMMERCE PROTOCOL
		//--------------------------------------


		public void SetTransactionID(string ti) {
			AppendData("ti", ti, "Transaction ID", 500, GA_HitType.TRANSACTION, GA_HitType.ITEM);
		}

		public void SetTransactionAffiliation(string ta) {
			AppendData("ta", ta, "Transaction Affiliation", 500, GA_HitType.TRANSACTION);
		}

		public void SetTransactionRevenue(float tr) {
			AppendData("tr", FloatToCurrency(tr), "", 0, GA_HitType.TRANSACTION);
		}

		public void SetTransactionShipping(float ts) {
			AppendData("ts", FloatToCurrency(ts), "", 0, GA_HitType.TRANSACTION);
		}

		public void SetTransactionTax(float tt) {
			AppendData("tt", FloatToCurrency(tt), "", 0, GA_HitType.TRANSACTION);
		}

		public void SetTransactionCouponCode(string tcc) {
			AppendData("tcc", tcc);
		}

		public void SetItemName(string name) {
			AppendData("in", name, "Item Name", 500, GA_HitType.ITEM);
		}

		public void SetItemPrice(float ip) {
			AppendData("ip", FloatToCurrency(ip), "", 0, GA_HitType.ITEM);
		}

		public void SetItemQuantity(int iq) {
			AppendData("iq", iq.ToString(), "", 0, GA_HitType.ITEM);
		}

		public void SetItemCode(string ic) {
			AppendData("ic", ic, "Item Code", 500, GA_HitType.ITEM);
		}

		public void SetItemCategory(string iv) {
			AppendData("iv", iv, "Item Category", 500, GA_HitType.ITEM);
		}


		public void SetCurrencyCode(string cu) {
			AppendData("cu", cu, "Currency Code", 10, GA_HitType.ITEM, GA_HitType.TRANSACTION);
		}




		//--------------------------------------
		// Enhanced E-CommerceL
		//--------------------------------------

		public void SetProductSKU(int productIndex, string sku) {
			AppendData("pr" + productIndex.ToString() + "id", sku, "Product SKU", 500);
		}

		public void SetSetProductName(int productIndex, string name) {
			AppendData("pr" + productIndex.ToString() + "nm", name, "Product Name", 500);
		}

		public void SetProductBrand(int productIndex, string brand) {
			AppendData("pr" + productIndex.ToString() + "br", brand, "Product Brand ", 500);
		}

		public void SetProductCategory(int productIndex, string category) {
			AppendData("pr" + productIndex.ToString() + "ca", category, "Product Category ", 500);
		}

		public void SetProductVariant(int productIndex, string variant) {
			AppendData("pr" + productIndex.ToString() + "va", variant, "Product Variant ", 500);
		}

		public void SetProductPrice(int productIndex, float prise) {
			AppendData("pr" + productIndex.ToString() + "pr", FloatToCurrency(prise));
		}

		public void SetProductQuantity(int productIndex, int quantit) {
			AppendData("pr" + productIndex.ToString() + "qt", quantit.ToString());
		}



		public void SetProductCouponCode(int productIndex, string couponCode) {
			AppendData("pr" + productIndex.ToString() + "cc", couponCode, "Product Coupon Code", 500);
		}

		public void SetProductPosition(int productIndex, int pos) {
			AppendData("pr" + productIndex.ToString() + "ps", pos.ToString());
		}

		public void SetProductCustomDimension(int productIndex, int index, string val) {
			AppendData("pr" + productIndex.ToString() + "cd" + index.ToString(), val);
		}

		public void SetProductCustomMetric(int productIndex, int index, int metric) {
			AppendData("pr" + productIndex.ToString() + "cm" + index.ToString(), metric.ToString());
		}

		public void SetProductAction(string pa) {
			AppendData("pa", pa);
		}

		public void SetProductActionList(string val) {
			AppendData("pal", val);
		}


		public void SetCheckoutStep(int cos) {
			AppendData("cos", cos.ToString());
		}

		public void SetCheckoutStepOption(string col) {
			AppendData("col", col);
		}

		/*
		public void ProductImpressionListName(int productIndex, string val) {
			AppendData("il" + productIndex.ToString() + "nm", val);
		}

		public void ProductImpressionListName(int productIndex, string val) {
			AppendData("il" + productIndex.ToString() + "nm", val);
		}
		*/


		//--------------------------------------
		// SOCIAL INTERACTIONS PROTOCOL
		//--------------------------------------

		public void SetSocialNetwork(string sn) {
			AppendData("sn", sn, "Social Network", 50, GA_HitType.SOCIAL);
		}

		public void SetSocialAction(string action) {
			AppendData("sa", action, "Social Action", 50, GA_HitType.SOCIAL);
		}

		public void SetSocialActionTarget(string target) {
			AppendData("st", target, "Social Action Target", 2048, GA_HitType.SOCIAL);
		}


		//--------------------------------------
		// TIMING PROTOCOL
		//--------------------------------------

		public void SetUserTimingCategory(string category) {
			AppendData("utc", category, "User Timing Category", 150, GA_HitType.TIMING);
		}

		public void SetUserTimingVariableName(string name) {
			AppendData("utv", name, "User Timing Variable Name", 500, GA_HitType.TIMING);
		}

		public void SetUserTimingTime(int time) {
			AppendData("utt", time.ToString(), "User Timing Time", 0, GA_HitType.TIMING);
		}

		public void SetUserTimingLabel(string label) {
			AppendData("utl", label, "User Timing Label", 500, GA_HitType.TIMING);
		}

		public void SetPageLoadTime(int time) {
			AppendData("plt", time.ToString(), "Page Load Time", 0, GA_HitType.TIMING);
		}

		public void SetDNSTime(int time) {
			AppendData("dns", time.ToString(), "DNS Time", 0, GA_HitType.TIMING);
		}

		public void SetPageDownloadTime(int time) {
			AppendData("pdt", time.ToString(), "", 0, GA_HitType.TIMING);
		}

		public void SetRedirectResponseTime(int time) {
			AppendData("rrt", time.ToString(), "", 0, GA_HitType.TIMING);
		}

		public void SetTCPConnectTime(int time) {
			AppendData("tcp", time.ToString(), "", 0, GA_HitType.TIMING);
		}


		public void SetServerResponseTime(int time) {
			AppendData("srt", time.ToString(), "", 0, GA_HitType.TIMING);
		}

		//--------------------------------------
		// Promotion
		//--------------------------------------

		public void SetPromotionID(int index, string id) {
			AppendData("promo" + index.ToString() + "id", id);
		}

		public void SetPromotionName(int index, string nm) {
			AppendData("promo" + index.ToString() + "nm",  nm);
		}

		public void SetPromotionCreative(int index, string cr) {
			AppendData("promo" + index.ToString() + "cr", cr);
		}

		public void SetPromotionPosition(int index, string ps) {
			AppendData("promo" + index.ToString() + "ps", ps);
		}

		public void SetPromotionAction(string promoa) {
			AppendData("promoa", promoa);
		}



		//--------------------------------------
		// EXCEPTIONS PROTOCOL
		//--------------------------------------

		public void SetExceptionDescription(string description) {
			AppendData("exd", description, "Exception Description", 150, GA_HitType.EXCEPTION);
		}

		public void SetIsFatalException(bool isFatal) {
			string data = "0";
			if(isFatal) {
				data = "1";
			}
			AppendData("exf", data, "", 0, GA_HitType.EXCEPTION);
		}


		public void SetNonInteractionFlag() {
			AppendData("ni", "1");
		}


		//--------------------------------------
		// Custom Dimensions / Metrics PROTOCOL
		//--------------------------------------


		public void SetCustomDimension(int index, string value) {
			AppendData("cd" + index.ToString(), value, "Custom Dimension", 150);
		}

		public void SetCustomMetric(int index, int value) {
			AppendData("cm" + index.ToString(), value.ToString());
		}


		//--------------------------------------
		// CONTENT EXPERIMENTS PROTOCOL
		//--------------------------------------

		public void SetExperimentID(string id) {
			AppendData("xid", id, "Experiment ID", 40);
		}

		public void SetExperimentVariant(string variant) {
			AppendData("xvar", variant);
		}


		//--------------------------------------
		//  METHODS API
		//--------------------------------------





		public void SendPageHit(string host, string page, string title, string description = "", string linkId = "") {
			CreateHit(GA_HitType.PAGEVIEW);
			SetDocumentHostName(host);
			SetDocumentPath(page);
			SetDocumentTitle(title);

			if(!description.Equals(string.Empty)) {
				SetScreenName(description);
			}

			if(!linkId.Equals(string.Empty)) {
				SetLinkID(linkId);
			}

			Send();
		}


		public void SendEventHit(string category, string action, string label = "", int val = -1, bool trackLevelName = false) {
			CreateHit(GA_HitType.EVENT);
			SetEventCategory(category);
			SetEventAction(action);


			if(trackLevelName) {
				SetScreenName(GA_Settings.Instance.LevelPrefix + GA_Manager.LoadedLevelName + GA_Settings.Instance.LevelPostfix);
			}



			if(!label.Equals(string.Empty)) {
				SetEventLabel(label);
			}

			if(val != -1) {
				SetEventValue(val);
			}

			Send();
		}


		public void SendTransactionHit(string id, string affiliation = "", string currencyCode = "",  float revenue = 0f, float shipping = 0f, float tax = 0f) {
			CreateHit(GA_HitType.TRANSACTION);


			SetTransactionID(id);

			if(!string.IsNullOrEmpty(affiliation)) {
				SetTransactionAffiliation(affiliation);
			}

			if(!string.IsNullOrEmpty(currencyCode)) {
				SetCurrencyCode(currencyCode);
			}

			if(revenue != 0f) {
				SetTransactionRevenue(revenue);
			}

			if(shipping != 0f) {
				SetTransactionShipping(shipping);
			}

			if(tax != 0f) {
				SetTransactionTax(tax);
			}


			Send();
		}


		public void SendItemHit(string transactionId, string name, string SKU, float price, int quantity = 1, string category = "", string currencyCode = "") {
			CreateHit(GA_HitType.ITEM);

			SetTransactionID(transactionId);
			SetItemName(name);
			SetItemCode(SKU);
			SetItemPrice(price);
			SetItemQuantity(quantity);

			if( !string.IsNullOrEmpty(category) ) {
				SetItemCategory(category);
			}

			if(!string.IsNullOrEmpty(currencyCode)) {
				SetCurrencyCode(currencyCode);
			}


			Send();
		}


		public void SendSocialHit(string action, string network, string target) {
			CreateHit(GA_HitType.SOCIAL);
			SetSocialAction(action);
			SetSocialNetwork(network);
			SetSocialActionTarget(target);

			Send();
		}

		public void SendExceptionHit(string description, bool IsFatal = false) {
			CreateHit(GA_HitType.EXCEPTION);
			SetExceptionDescription(description);

			if(IsFatal) {
				SetIsFatalException(IsFatal);
			}

			Send();
		}


		public void SendUserTimingHit(string category, string variable, int time, string label) {

			CreateHit(GA_HitType.TIMING);
			SetUserTimingCategory(category);
			SetUserTimingVariableName(variable);
			SetUserTimingTime(time);
			SetUserTimingLabel(label);

			Send();

		}

		public void SendScreenHit(string screenName) {
			CreateHit(GA_HitType.APPVIEW);
			SetScreenName(screenName);
			Send();
		}


		public void CreateHit(GA_HitType hit) {
			currentHitType = hit;

			builder.Length = 0;
			builder.Append(DefaultHitData);
			SetHitType(hit);
		}

		public void Send() {

			if(GA_Settings.Instance.IsDisabled) {
				return;
			}

#if UNITY_EDITOR

			if(!GA_Settings.Instance.EditorAnalytics) {
				return;
			}

#endif

			//anti Cache
			builder.Append("&z=");
			builder.Append(UnityEngine.Random.Range(0, Int32.MaxValue) ^ 3424);


			string stringData = builder.ToString();
			builder.Length = 0;

            GA_Manager.Send(stringData);

		}


		public UnityWebRequest GenerateWWW(byte[] data) {
			var www = new UnityWebRequest(DataSendUrl, "POST");
			var uploadHandler = (UploadHandler) new UploadHandlerRaw(data);
			uploadHandler.contentType = "application/x-www-form-urlencoded";
			www.uploadHandler = uploadHandler;
			www.downloadHandler = new DownloadHandlerBuffer();
#if UNITY_IPHONE || UNITY_WP8 || UNITY_METRO
			foreach (var kv in Headers) {
				www.SetRequestHeader(kv.Key, kv.Value);
			}
#endif
			return www;
		}





		//--------------------------------------
		//  Get / Set
		//--------------------------------------

		public string AnalyticsHost {
			get {
				return DataSendUrl;
			}

		}

		//--------------------------------------
		//  PRIVATE METHODS
		//--------------------------------------


		public void AppendData(string protocolToken, string val) {
			AppendData(protocolToken, val, "", 0);
		}



		public void AppendData(string protocolToken, string val, string action, int maxSize, params GA_HitType[] supportedHitTypes) {


			if(supportedHitTypes.Length > 0) {

				bool isActionSupprted = false;

				foreach(GA_HitType h in supportedHitTypes) {
					if(h == currentHitType) {
						isActionSupprted = true;
					}
				}

				if(!isActionSupprted) {
                    Debug.LogWarning("Google Analytics: " + action + " not supported for hit type  " + currentHitType.ToString());
					return;
				}

			}

			string data = EscapeString(val);
			builder.Append("&");
			builder.Append(protocolToken);
			builder.Append("=");
			builder.Append(data);

			if(maxSize > 0) {
				CheckDataLength(action, data, maxSize);
			}
		}




		private string FloatToCurrency(float val) {
			return val.ToString("n2");
		}

		private void CheckDataLength(string action, string data, int maxLength) {

			if(string.IsNullOrEmpty(data)) {
                Debug.LogWarning("Google Analytics: " + action + " data is null");
				return;
			}

			if(data.Length > maxLength) {
                Debug.LogWarning("Google Analytics: " + action + " is too long, max size " + maxLength + " bytes (" + data + ")");
			}
		}



		private string EscapeString(string original) {
			return EscapeString(original, false);
		}

		private string EscapeString(string original, bool forced) {
			if(forced) {
                return UnityWebRequest.EscapeURL(original);
            } else {
				if(GA_Settings.Instance.StringEscaping) {
                    return UnityWebRequest.EscapeURL(original);
                } else {
					return original;
				}
			}
		}

	}
}