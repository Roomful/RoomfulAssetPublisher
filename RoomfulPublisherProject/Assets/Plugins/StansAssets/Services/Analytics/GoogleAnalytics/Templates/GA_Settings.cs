////////////////////////////////////////////////////////////////////////////////
//
// @module Google Analytics Plugin
// @author Osipov Stanislav (Stan's Assets)
// @support support@stansassets.com
// @website https://stansassets.com
//
////////////////////////////////////////////////////////////////////////////////



using UnityEngine;
using System.IO;
using System.Collections.Generic;

using SA.Foundation.Patterns;


namespace SA.Analytics.Google {

    public class GA_Settings : SA_ScriptableSingleton<GA_Settings> {

		public static string VERSION_NUMBER = "2018.1.0b";


		[SerializeField]
		public List<GA_Profile> accounts =  new List<GA_Profile>();

		[SerializeField]
		public List<GA_PlatfromBound> platfromBounds =  new List<GA_PlatfromBound>();



		public bool showAdditionalParams = false;
		public bool showAdvancedParams = false;
		public bool showCParams = false;

		public bool showAccounts = true;
		public bool showPlatfroms = false;
		public bool showTestingMode = false;





		public bool UseHTTPS = false;
		public bool StringEscaping = true;
		public bool EditorAnalytics = true;
		public bool IsDisabled = false;


		public bool IsTestingModeEnabled = false;
		public int TestingModeAccIndex = 0;


		public bool IsRequetsCachingEnabled= true;
		public bool IsQueueTimeEnabled = true;


		public bool AutoLevelTracking = true;
		public string LevelPrefix = "Level_";
		public string LevelPostfix = "";


		public bool AutoAppQuitTracking = true;
		public bool AutoExceptionTracking = true;
		public bool AutoAppResumeTracking = true;
		public bool SubmitSystemInfoOnFirstLaunch = true;




        //--------------------------------------
        //  Get / Set
        //--------------------------------------


        public string AppName  {
            get {
                return Application.productName;
            }
        }

        public string AppVersion  {
            get {
                return Application.version;
            }
        }


        //--------------------------------------
        //  Public Methods
        //--------------------------------------



		public void AddProfile(GA_Profile p) {
			accounts.Add(p);
		}

		public void RemoveProfile(GA_Profile p) {
			accounts.Remove(p);
		}

		public void SetProfileIndexForPlatfrom(RuntimePlatform platfrom, int index, bool IsTesting) {
			foreach(GA_PlatfromBound pb in platfromBounds) {
				if(pb.platfrom.Equals(platfrom)) {

					if(IsTesting) {
						pb.profileIndexTestMode = index;
					} else {
						pb.profileIndex = index;
					}

					return;
				}
			}

            GA_PlatfromBound bound =  new GA_PlatfromBound();
			bound.platfrom = platfrom;
			bound.profileIndex = 0;
			bound.profileIndexTestMode = 0;
			if(IsTesting) {
				bound.profileIndexTestMode = index;
			} else {
				bound.profileIndex = index;
			}

			platfromBounds.Add(bound);

		}

		public int GetProfileIndexForPlatfrom(RuntimePlatform platfrom, bool IsTestMode) {
			foreach(GA_PlatfromBound pb in platfromBounds) {
				if(pb.platfrom.Equals(platfrom)) {
					int index =  pb.profileIndex;
					if(IsTestMode) {
						index = pb.profileIndexTestMode;
					}

					if(index < accounts.Count) {
						return index;
					} else {
						return 0;
					}
				}
			}

			return 0;
		}

		public string[] GetProfileNames() {
			List<string> names =  new List<string>();
			foreach(GA_Profile p in accounts) {
				names.Add(p.Name);
			}

			return names.ToArray();
		}

		public int GetProfileIndex(GA_Profile p ) {
			int index = 0;
			string[] names = GetProfileNames();

			foreach(string name in names) {
				if(name.Equals(p.Name)) {
					return index;
				}

				index++;
			}

			return 0;

		}




		public GA_Profile GetCurentProfile() {
			return GetPrfileForPlatfrom(Application.platform, IsTestingModeEnabled);
		}

		public GA_Profile GetPrfileForPlatfrom(RuntimePlatform platfrom, bool IsTestMode) {

			if (accounts.Count == 0) {
				return new GA_Profile();
			}



			return accounts[GetProfileIndexForPlatfrom(platfrom, IsTestMode)];

		}

	    protected override string BasePath {
		    get { return string.Empty; }
	    }

	    public override string PluginName { get; }
	    public override string DocumentationURL { get; }
	    public override string SettingsUIMenuItem { get; }
    }
}
