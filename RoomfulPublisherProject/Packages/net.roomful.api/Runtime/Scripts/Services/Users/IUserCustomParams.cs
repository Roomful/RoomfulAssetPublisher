using System.Collections.Generic;

namespace net.roomful.api{

    public interface IUserCustomParams {

        void Update(Dictionary<string, object> @params);
        void Set(string paramKeyKey, object paramValue);
        void Delete(string paramKeyKey);
        bool Contains(string paramKeyKey);
        T Get<T>(string paramKeyKey);
    }
}
