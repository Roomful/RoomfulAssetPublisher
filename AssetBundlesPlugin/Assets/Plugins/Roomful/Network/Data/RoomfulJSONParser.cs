using System;
using UnityEngine;

using Moon.Network.Web;

public class RoomfulJSONParser : JSONDataWriter, IDataWriter
{


    public override object GetValue(object filedValue, Type type) {

        object value = GetDefaultValue(type);

        if (type == typeof(DateTime)) {

            string dateString = Convert.ToString(filedValue);
            DateTime date;

            bool parsed = SA.Common.Util.General.TryParseRfc3339(dateString, out date);
            if (!parsed) {
                Debug.LogWarning("Date Parsing failed: " + dateString);
            }

            value = System.Convert.ChangeType(date, type);

        } else {
            value = System.Convert.ChangeType(filedValue, type);
        }

        return value;
    }

}
