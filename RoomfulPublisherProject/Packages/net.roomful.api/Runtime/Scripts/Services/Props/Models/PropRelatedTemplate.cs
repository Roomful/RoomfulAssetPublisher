using System;
using System.Collections.Generic;
using net.roomful.api.assets;

namespace net.roomful.api
{
    public abstract class PropRelatedTemplate : TemplateDataModel
    {
        public DateTime Created { get; } = DateTime.MinValue;
        public DateTime Updated { get; } = DateTime.MinValue;
        
        protected PropRelatedTemplate() { }
        
        public PropRelatedTemplate(JSONData metaData):base(metaData) {
            Created = metaData.GetValue<DateTime>("created");
            Updated = metaData.GetValue<DateTime>("updated");
        }

        public override Dictionary<string, object> ToDictionary() {
            var data = base.ToDictionary();
            data.Add("created", RoomfulTime.DateTimeToRfc3339(Created));
            data.Add("updated", RoomfulTime.DateTimeToRfc3339(Updated));

            return data;
        }
    }
}