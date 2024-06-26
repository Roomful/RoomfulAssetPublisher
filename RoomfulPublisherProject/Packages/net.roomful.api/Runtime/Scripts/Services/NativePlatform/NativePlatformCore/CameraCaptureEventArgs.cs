namespace net.roomful.api.native
{
    public abstract class CameraCaptureEventArgs
    {
        public string Data { get; }
        
        protected readonly JSONData m_jsonData;
        
        protected CameraCaptureEventArgs(string data) {
            Data = data;
            
            m_jsonData = new JSONData(Data);
        }
    }
    
    public class CameraCaptureRequestCompletedArgs : CameraCaptureEventArgs
    {
        public string DeviceId { get; }

        public CameraCaptureRequestCompletedArgs(string data) : base(data) {
            DeviceId = m_jsonData.GetValue<string>("captureDeviceId");
        }
    }
    
    public class CameraCaptureRequestFailedArgs : CameraCaptureEventArgs
    {
        public string Error { get; }

        public CameraCaptureRequestFailedArgs(string data) : base(data) {
            Error = data;
        }
    }
}