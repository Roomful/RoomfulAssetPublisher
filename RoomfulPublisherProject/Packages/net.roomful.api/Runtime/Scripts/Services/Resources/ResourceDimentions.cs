using System.Collections.Generic;

namespace net.roomful.api
{
	public class ResourceDimentions  {

		private int m_width = 1;
		private int m_height = 1;

		public ResourceDimentions() {}

		public ResourceDimentions(JSONData info) {
			if(info.HasValue("width")) {
				m_width = info.GetValue<int>("width");
				if (m_width < 1) {
					m_width = 1;
				}
			}
			if(info.HasValue("height")) {
				m_height = info.GetValue<int>("height");
				if (m_height < 1) { m_height = 1; }
			}
		}

		public Dictionary<string, object> ToDictionary() {
			var data = new Dictionary<string, object> {
				{"width", m_width},
				{"height", m_height}
			};
			return data;
		}

		public void SetSize(int w, int h) {
			m_width = w;
			m_height = h;
		}

		public int Width => m_width;

		public int Height => m_height;
	}
}