using System;
using UnityEngine;

namespace Util
{
    public static class TimeUtil
    {
        public static bool IsTimeEnd(ref float timer, float delay)
        {
            if (timer > 0f)
            {
                timer -= Time.deltaTime;
                return false;
            }

            timer = delay;
            return true;    
        }
    }
}