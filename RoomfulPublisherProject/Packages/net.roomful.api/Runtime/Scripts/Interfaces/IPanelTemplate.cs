using System.Collections.Generic;
using net.roomful.api.props;

namespace net.roomful.api
 {
     public interface IPanelTemplate : IRoomContentTemplate {
         List<IPropTemplate> InitialProps { get; }
         string Name { get; }
         string StyleId { get; }
         string ParentPanelId { get; }
         void SetStyleId(string styleId);
         void SetParentId(string parentId);
     }
 }
