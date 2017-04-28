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

public class TagSwitchHandler  {

		private CustomTag _Tag;

		public TagSwitchHandler(CustomTag tag) {
			_Tag = tag;
		}


		public void Switch() {
			_Tag.Enabled = !_Tag.Enabled;
			LoggerWindow.Refresh ();
		}

}


}