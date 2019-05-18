using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    delegate void AlarmEventHandler(object sender, AlarmEventArgs e);
    class Clock
    {
        private Time curT, alarmT;
        public event AlarmEventHandler alarm;
        public Clock(Time t1, Time t2)
        {
            curT=new Time(t1);
            alarmT=new Time(t2);
        }
        public Clock()
        {
            curT=new Time();
            alarmT = new Time();
        }
        private void onAlarm()
        {
            if(alarm!=null)
                alarm(this, new AlarmEventArgs(alarmT));
        }
        public void run()
        {
            while(true)
            {
                curT.show();
                if (curT == alarmT) onAlarm();
                System.Threading.Thread.Sleep(1000);
                curT++;
            }
        }
    }
}
