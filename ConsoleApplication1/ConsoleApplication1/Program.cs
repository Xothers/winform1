using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    public delegate double myDelegate(double x);
    class Program
    {
        static void Main(string[] args)
        {
            Time t1 = new Time(11,2,59);
            myDelegate myd=new myDelegate(fun);
            FormatTime t2 = new FormatTime(14,2,3);
            Clock myClock = new Clock(new Time(0,0,0),new Time(0,0,10));
            myClock.alarm += new AlarmEventHandler(myClock_Alarm);
            myClock.run();
            t1.show();
            t2.show();
            Console.ReadKey();
        }
        static double fun(double x)
        {
            return x+1;
        }
        static double integral(double a, double b, myDelegate p)
        {
            return 0;
        }
        static private void myClock_Alarm(object sender, AlarmEventArgs e)
        {
            Console.WriteLine("Alarm!!!");
            Console.WriteLine("Alarm time =");
            e.aTime.show();
        }
    }
}