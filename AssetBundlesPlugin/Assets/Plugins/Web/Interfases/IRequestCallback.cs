using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Moon.Network.Web {

	public interface IRequestCallback  {

        void SetResponce(WebResponce reponce);
        
        IRequest Request { get; set; }
	}

}