////////////////////////////////////////////////////////////////////////////////
//
// @module Assets Common Lib
// @author Osipov Stanislav (Stan's Assets)
// @support support@stansassets.com
// @website https://stansassets.com
//
////////////////////////////////////////////////////////////////////////////////


using System;
using System.Globalization;
using System.Text;
using System.Security.Cryptography;
using SA.Common.Models;

#if NETFX_CORE
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
#endif

namespace SA.Common.Util {

	public static class General {


#pragma warning disable CS1692

		//--------------------------------------
		// Time
		//--------------------------------------


		/// <summary>
		/// Current UTC timestamp format
		/// </summary>
		public static Int32 CurrentTimeStamp {
			get {
				return (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
			}
		}



		//--------------------------------------
        // Sequrity
        //--------------------------------------


#if NETFX_CORE

        public static string HMAC(string key, string data)
        {
            byte[] secretKey = Encoding.ASCII.GetBytes(key);
            var objMacProv = MacAlgorithmProvider.OpenAlgorithm(MacAlgorithmNames.HmacSha256);
            var hash = objMacProv.CreateHash(secretKey.AsBuffer());
            hash.Append(CryptographicBuffer.ConvertStringToBinary(data, BinaryStringEncoding.Utf8));
            return CryptographicBuffer.EncodeToBase64String(hash.GetValueAndReset());
        }
#else
        /// <summary>
        /// HMAC SHA256 hex key
        /// </summary>

        public static string HMAC(string key, string data)
        {
            var keyByte = ASCIIEncoding.UTF8.GetBytes(key);
            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                hmacsha256.ComputeHash(ASCIIEncoding.UTF8.GetBytes(data));

                byte[] buff = hmacsha256.Hash;
                string sbinary = "";

                for (int i = 0; i < buff.Length; i++)
                    sbinary += buff[i].ToString("X2"); /* hex format */
                return sbinary.ToLower();

            }
        }
#endif
	}

}
