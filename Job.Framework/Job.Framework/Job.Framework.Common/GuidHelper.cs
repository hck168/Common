using System;
using System.Threading;

namespace Job.Framework.Common
{
    public static class GuidHelper
    {
        private static readonly DateTime combBaseDate = new DateTime(1900, 1, 1);
        private static int lastDays;
        private static int lastTenthMilliseconds; 

        public static Guid NewGuid()
        {
            var now = DateTime.Now;
            var days = new TimeSpan(now.Ticks - combBaseDate.Ticks).Days;
            var guidArray = Guid.NewGuid().ToByteArray();
            var tenthMilliseconds = (Int32)(now.TimeOfDay.TotalMilliseconds * 10D);

            if (days > lastDays)
            {
                Interlocked.CompareExchange(ref lastDays, days, lastDays);
                Interlocked.CompareExchange(ref lastTenthMilliseconds, tenthMilliseconds, lastTenthMilliseconds);
            }
            else
            {
                if (tenthMilliseconds > lastTenthMilliseconds)
                {
                    Interlocked.CompareExchange(ref lastTenthMilliseconds, tenthMilliseconds, lastTenthMilliseconds);
                }
                else
                {
                    if (lastTenthMilliseconds < Int32.MaxValue) { Interlocked.Increment(ref lastTenthMilliseconds); }
                    tenthMilliseconds = lastTenthMilliseconds;
                }
            }

            var daysArray = BitConverter.GetBytes(days);
            var msecsArray = BitConverter.GetBytes(tenthMilliseconds);

            Array.Reverse(daysArray);
            Array.Reverse(msecsArray);

            Array.Copy(daysArray, daysArray.Length - 2, guidArray, guidArray.Length - 6, 2);
            Array.Copy(msecsArray, 0, guidArray, guidArray.Length - 4, 4);

            return new Guid(guidArray);
        }
    }
}