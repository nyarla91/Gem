using UnityEngine;

namespace Extentions
{
    public static class StringExtentions
    {
        public static string SecondsToFormatTime(this int seconds, bool showHours)
        {
            string secondsLine = $"{seconds % 60}";
            if (seconds % 60 < 10)
                secondsLine = "0" + secondsLine;
            
            string minutesLine = $"{Mathf.FloorToInt(seconds / 60)}:";
            if (Mathf.FloorToInt(seconds / 60) < 10)
                minutesLine = "0" + minutesLine;

            string hoursLine = "";
            if (showHours)
            {
                hoursLine = $"{Mathf.FloorToInt(seconds / 360)}:";
                if (Mathf.FloorToInt(seconds / 360) < 10)
                    hoursLine = "0" + hoursLine;
            }
            return hoursLine + minutesLine + secondsLine;
        }
    }

}