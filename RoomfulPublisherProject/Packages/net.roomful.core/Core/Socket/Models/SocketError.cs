// Copyright Roomful 2013-2019. All rights reserved.

namespace net.roomful.api.socket
{
    public class SocketError
    {
        private readonly string m_message;

        public SocketError(int code, string message) {
            Code = code;
            m_message = message;
        }

        public void SetDebugInfo(string debug) {
            Debug = debug;
        }

        public override string ToString() {
            return Code + " / " + Message + " / " + Debug;
        }

        public int Code { get; }

        public string Debug { get; private set; } = string.Empty;

        public string Message => m_message + " " + Debug;

        public string FullMessage => $"Error: {Code} \n{Message}";
    }
}