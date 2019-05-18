using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class AlarmEventArgs:EventArgs
    {
        public Time aTime;
        //public Time ATime;
        public AlarmEventArgs()
            : base()
        {
            aTime = new Time();
        }
        public  AlarmEventArgs(Time t)
            : base()
        {
            aTime=new Time(t);
        }
    }
}
