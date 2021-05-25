using System.Collections.Generic;

namespace net.roomful.api
{
    public abstract class TemplateDataModel
    {
        public string Id { get; set; } = string.Empty;
        public JSONData RawData { get; }

        protected TemplateDataModel() { }

        protected TemplateDataModel(JSONData rawData) {
            RawData = rawData;
            Id = rawData.GetValue<string>("id");
        }

        public virtual Dictionary<string, object> ToDictionary() {
            var data = new Dictionary<string, object>();
            data.Add("id", Id);
            return data;
        }
    }
}