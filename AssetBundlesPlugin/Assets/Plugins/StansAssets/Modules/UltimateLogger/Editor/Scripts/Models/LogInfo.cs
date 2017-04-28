////////////////////////////////////////////////////////////////////////////////
//  
// @module Ultimate Logger
// @author Osipov Stanislav (Stan's Assets) 
// @support support@stansassets.com
// @website https://stansassets.com
//
////////////////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA.UltimateLogger {


	[System.Serializable]
	public class LogInfo {

		private string _LogString = string.Empty;
		private string _StackTrace  = string.Empty;
		private FilePointer _Pointer = null;
		private List<LogStackLine> _Stack = new List<LogStackLine> ();
		private UnityEngine.LogType _LogType = UnityEngine.LogType.Log;
		private string _TagName = string.Empty;
		private int _Mode = 0;
		private int _instanceID = 0;
		private int _LineNumber = 0;

		//--------------------------------------
		// Initialisation
		//--------------------------------------

		public LogInfo (LogInfo log) {

			_LogString = log.LogString;
			_instanceID = log.instanceID;
			_Stack = log.Stack;
			_TagName = log.TagName;
			_Pointer = log.Pointer;
			_LogType = log.LogType;
		}


		public LogInfo(string logString, int mode, int instanceId) {
			
			_Mode = mode;
			_instanceID = instanceId;

			if(HasMode(LogMode.AssetImportError) || HasMode(LogMode.Error) || HasMode(LogMode.GraphCompileError) || HasMode(LogMode.ScriptCompileError) || HasMode(LogMode.ScriptingError) || HasMode(LogMode.StickyError)) {
				_LogType = LogType.Error;
			} else if(HasMode(LogMode.AssetImportWarning) || HasMode(LogMode.ScriptCompileWarning) || HasMode(LogMode.ScriptingWarning)) {
				_LogType = LogType.Warning;
			} else {
				_LogType = LogType.Log;
			}



			//Parsing stack trase and log message
			bool stackFound = false;
			string[] lines = null;
			if (Application.platform == RuntimePlatform.OSXEditor) {
				lines = System.Text.RegularExpressions.Regex.Split (logString, System.Environment.NewLine);
			} else {
				lines = System.Text.RegularExpressions.Regex.Split (logString, "\n");
			}

			if(logString.Contains("UnityEngine.Debug:")) {
				foreach(string line in lines) {
					if(line.Contains("UnityEngine.Debug:")) {
						stackFound = true;
					}

					if(!stackFound) {
						_LogString += line;
					} else {
						AddStackLine (line);
					}
				}
			} else {
				foreach (string line in lines) {
					if(!stackFound) {
						_LogString = line;
						stackFound = true;
					} else {
						AddStackLine (line);
					}
				}
			}


				

			//In case this is an error or warning
			var match = System.Text.RegularExpressions.Regex.Matches(_LogString, @"Assets\/(.*)\((\d+),(\d+)\):");
			if(match.Count > 0) {
				string name = "Assets/" + match[0].Groups[1].Value;
				int line = System.Convert.ToInt32(match[0].Groups[2].Value);
				_Pointer = new FilePointer (name, line);
			} 

			//if log line has tag
			if(_LogType == LogType.Log) {
				match = System.Text.RegularExpressions.Regex.Matches(_LogString, @"\[(.*)\]: ");
				if (match.Count > 0) {
					_TagName = match [0].Groups [1].Value;
					int subValue = _TagName.Length + 4;
					_LogString = _LogString.Substring (subValue, _LogString.Length - subValue);
				}
			}

			if(_TagName.Equals(string.Empty)) {
				switch(_LogType) {
				case LogType.Error:
					_TagName = LoggerSettings.ERROR_TAG_NAME;
					break;
				case LogType.Warning:
					_TagName = LoggerSettings.WARNING_TAG_NAME;
					break;
				default:
					_TagName = LoggerSettings.MESSAGE_TAG_NAME;
					break;
					
				}
			}

				

		}


	




		//--------------------------------------
		// Public Methods
		//--------------------------------------

		public bool HasMode(LogMode modeToCheck) {
			return (_Mode & (int)modeToCheck) != 0;
		}

		public void SetLineNumber(int line) {
			_LineNumber = line;
		}
			

		//--------------------------------------
		// Get / Set
		//--------------------------------------

		public string LogString {
			get {
				return _LogString;
			}

			set {
				_LogString = value;
			}
		}

		public string StackTrace {
			get {
				return _StackTrace;
			}
		}

		public UnityEngine.LogType LogType {
			get {
				return _LogType;
			}
		}

		public List<LogStackLine> Stack {
			get {
				return _Stack;
			}
		}

		public FilePointer Pointer {
			get {
				return _Pointer;
			}
		}

		public string TagName {
			get {
				return _TagName;
			}
		}
			

		public int instanceID {
			get {
				return _instanceID;
			}
		}

		public bool HasValidFilePointer {
			get {
				if(Pointer == null) {
					return false;
				}

				return Pointer.CanBeOpened;
			}
		}

		public int LineNumber {
			get {
				return _LineNumber;
			}
		}
			
		//--------------------------------------
		// Private Methods
		//--------------------------------------



		private void AddStackLine(string line) {
			if(line.Length > 1) {
				var stackLine = new LogStackLine(line);
				stackLine.SetLineNumber (Stack.Count);
				Stack.Add(stackLine);
			}
		}

	}
}
