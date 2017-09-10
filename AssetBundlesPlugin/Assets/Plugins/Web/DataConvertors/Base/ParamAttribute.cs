using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Moon.Network.Web {
	
	public class ParamAttribute : System.Attribute {


		private string m_name = string.Empty;


        public ParamAttribute() {

        }

        public ParamAttribute(string name) {
			m_name = name;
		}


		public string Name {
			get {
				return m_name;
			}
		}


	}
}


