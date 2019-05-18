using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Time
    {
        protected int hour, minute, second;
        public Time(){}
        public Time(int h, int m, int s)
        {
            hour = h;
            minute = m;
            second = s;
        }
        public Time(Time t)
        {
            second = t.second;
            minute = t.minute;
            hour = t.hour;
        }
        public void show()
        {
            Console.WriteLine("{0:00}:{1:00}:{2:00}", hour, minute, second);
        }
        public Time Clone()
        {
            return new Time(hour, minute, second);
        }
        public override bool Equals(object obj)
        {
            Time t = (Time)obj;
            return hour == t.hour && minute == t.minute && second == t.second;
        }
        public static bool operator ==(Time t1, Time t2)
        {
            return t1.hour == t2.hour && t1.minute == t2.minute && t1.second == t2.second;
        }
        public static bool operator !=(Time t1, Time t2)
        {
            return !(t1 == t2);
        }
        public static Time operator ++(Time t)
        {
            int seconds;
            seconds = t.hour * 3600 + t.minute * 60 + t.second + 1;
            t.hour = seconds / 3600;
            t.minute = seconds / 60 % 60;
            t.second = seconds % 60;
            return new Time(t);
        }
        public static explicit operator int(Time t)
        {
            return t.hour * 3600 + t.minute * 60 + t.second;
        }
    }
}

