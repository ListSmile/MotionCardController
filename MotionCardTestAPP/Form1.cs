using MotionController.Core.Interface;
using MotionController.MotionController.ZMotion.Entities;

namespace MotionCardTestAPP
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        IMotionController motionController;
        private void button1_Click(object sender, EventArgs e)
        {
            motionController = new ZMotionController("ZMC", "1.0");
            ZMotionInput intput1 = new ZMotionInput("默认输入1",0, motionController.CardHandle);
            ZMotionOutput output1 = new ZMotionOutput("默认输出1", 0, motionController.CardHandle);
            ZMotionAxis axis1 = new ZMotionAxis(0, "默认轴1", motionController.CardHandle);


            var val = intput1.Value;
        }
    }
}
