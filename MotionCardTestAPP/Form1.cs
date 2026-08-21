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
            motionController = new ZMotionController_PCIE("ZMC_PCIE");
            ZMotionInput intput1 = new ZMotionInput("Input1",0, motionController.CardHandle);
            ZMotionOutput output1 = new ZMotionOutput("Output1", 0, motionController.CardHandle);
            ZMotionAxis axis1 = new ZMotionAxis(0, "Axis1", motionController.CardHandle);


            var val = intput1.Value;
        }
    }
}
