using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Moon.Network.Web {

	public interface IServerCommunicator  {

        void Send(IRequest request);

	}

}
