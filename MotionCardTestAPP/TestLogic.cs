using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotionCardTestAPP
{
    public class TestLogic : LogicBase
    {
        public TestLogic(string name) : base(name)
        {

        }

        protected override int ActualMotionLogic(int step)
        {
            switch (CurrentStep)
            {
                case 1:
                    if (LGTool.Delay(3000))
                    {
                        GoToStep(2);
                    }
                    
                    break;
                case 2:
                    if (LGTool.Delay(2000))
                    {
                        GoToStep(3);
                    }

                    break;
                case 3:

                    if (LongTimeNoStop)
                    {
                        GoToStep(1);
                    }
                    if (LGTool.Delay(2000))
                    {
                        Stop();
                        Reset();
                    }
                    break;
            }
            return 0;
        }
    }
}
