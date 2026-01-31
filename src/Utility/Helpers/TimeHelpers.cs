using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace OutwardBasicChatCommands.Utility.Helpers
{
    public static class TimeHelpers
    {
        public static void SetTime(int hours, int minutes)
        {
            // Normalize minutes
            minutes = Mathf.Clamp(minutes, 0, 59);
            hours = Mathf.Clamp(hours, 0, 23);

            float hourFloat = hours + (minutes / 60f);

            float delta = hourFloat - TOD_Sky.Instance.Cycle.Hour;

            TOD_Sky.Instance.Cycle.Hour = hourFloat;
            EnvironmentConditions.Instance.m_timeJump = delta;
            EnvironmentConditions.Instance.m_dayReset = true;
        }

        public static void SetMinutes(int minutes)
        {
            minutes = Mathf.Clamp(minutes, 0, 59);

            float currentHour = TOD_Sky.Instance.Cycle.Hour;
            int hourPart = Mathf.FloorToInt(currentHour);

            float newHour = hourPart + (minutes / 60f);
            float delta = newHour - currentHour;

            TOD_Sky.Instance.Cycle.Hour = newHour;
            EnvironmentConditions.Instance.m_timeJump = delta;
            EnvironmentConditions.Instance.m_dayReset = true;
        }
    }
}
