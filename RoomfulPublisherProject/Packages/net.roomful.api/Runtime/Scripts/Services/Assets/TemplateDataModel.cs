using System.Collections.Generic;

namespace net.roomful.api.assets
{
    /// <summary>
    /// Template data model.
    /// </summary>
    public abstract class TemplateDataModel : ITemplate
    {
        /// <summary>
        /// Template id.
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Raw server data as JSON.
        /// </summary>
        public JSONData RawData { get; }

        protected TemplateDataModel() { }

        protected TemplateDataModel(JSONData rawData) {
            RawData = rawData;
            Id = rawData.GetValue<string>("id");
        }

        /// <summary>
        /// Converts template data to the dictionary key / value representation.
        /// </summary>
        /// <returns>New Dictionary instance with asset data inside.</returns>
        public virtual Dictionary<string, object> ToDictionary() {
            var data = new Dictionary<string, object>();
            data.Add("id", Id);
            return data;
        }
    }
}
