////////////////////////////////////////////////////////////////////////////////
//  
// @module Assets Common Lib
// @author Osipov Stanislav (Stan's Assets) 
// @support support@stansassets.com
// @website https://stansassets.com
//
////////////////////////////////////////////////////////////////////////////////

namespace SA.Common.Data {

    public class Converter  {

        //--------------------------------------
        // Constants
        //--------------------------------------
		
        public const char DATA_SPLITTER = '|';

        public const string ARRAY_SPLITTER = "%%%";
        public const string DATA_EOF = "endofline";

        public static string SerializeArray(string[] array, string splitter = ARRAY_SPLITTER) {

            if(array == null) {
                return string.Empty;
            }

            if(array.Length == 0) {
                return string.Empty;
            }

            string serializedArray = "";
            int len = array.Length;
            for(int i = 0; i < len; i++) {
                if(i != 0) {
                    serializedArray += splitter;
                }
						
                serializedArray += array[i];
            }
					
            return serializedArray;
        }
    }

}