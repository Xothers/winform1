using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class FormatTime:Time
    {
        public FormatTime()
            : base()
        {
        }
        public FormatTime(int h, int m, int s)
            : base(h, m, s)
        {
        }
        public void show()
        {
            if (hour < 12)
                base.show();
            else
            {
                Console.WriteLine("{0:00}:{1:00}:{2:00} PM", hour-12, minute, second);
            }
        }
    }
}
