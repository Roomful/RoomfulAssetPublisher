////////////////////////////////////////////////////////////////////////////////
//  
// @module Ultimate Logger
// @author Osipov Stanislav (Stan's Assets) 
// @support support@stansassets.com
// @website https://stansassets.com
//
////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


namespace SA.UltimateLogger {

	[System.Serializable]
	public class LogStackLine  {

		private string _RawData = string.Empty;
		private FilePointer _Pointer = null;
		private int _LineNumber = 0;



		//--------------------------------------
		// Initialisation
		//--------------------------------------


		public LogStackLine(string unityStackFrame) {
			_RawData = unityStackFrame;

			var match = System.Text.RegularExpressions.Regex.Matches(_RawData, @".*\(at (.*):(\d+)");
			if(match.Count > 0) {
				string name = match[0].Groups[1].Value;
				int line = Convert.ToInt32(match[0].Groups[2].Value);
				_Pointer = new FilePointer (name, line);

				if(LoggerWindow.Settings.IgnoredWrapperClasses.Contains(_Pointer.FileName)) {
					_Pointer = null;
				}
			} 


		}

		//--------------------------------------
		// Public Methods
		//--------------------------------------

		public void SetLineNumber(int number) {
			_LineNumber = number;
		}


		//--------------------------------------
		// Get / Set
		//--------------------------------------
			

		public string RawData {
			get {
				return _RawData;
			}
		}

		public int LineNumber {
			get {
				return _LineNumber;
			}
		}			

		public FilePointer Pointer {
			get {
				return _Pointer;
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
	}

}