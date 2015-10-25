using Mono.Xml.Xsl;
using System;
using UnityEngine;

public class TimeUtility {
    public static TimeSpan getTimePassedSinceLastRoll() {
        //Store the current time when it starts
        DateTime currentDate = System.DateTime.Now;
        //Grab the old time from the player prefs as a long
        string lastRoll = PlayerPrefs.GetString(Const.LAST_ROLL);
        if (lastRoll == null || lastRoll.Equals("")) {
            return new TimeSpan(4, 0, 0);
        }
        long temp = Convert.ToInt64(lastRoll);
        //Convert the old time from binary to a DataTime variable
        DateTime targetDate = DateTime.FromBinary(temp);
        Debug.Log(targetDate.ToString());
        //Use the Subtract method and store the result as a timespan variable
        TimeSpan difference = targetDate.Subtract(currentDate);
        Debug.Log(difference.ToString() + " " + difference.Hours + " " + difference.Minutes);
        return difference;
    }

    public static void saveLastRollTime(){
        DateTime now = System.DateTime.Now;
        DateTime targetTime = now.AddHours(4);
        Debug.Log(targetTime.ToString());
        PlayerPrefs.SetString(Const.LAST_ROLL, targetTime.ToBinary().ToString());
    }
}
