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
            motionController = new ZMotionController_PCIE("ZMC_PCIE","");
            ZMotionInput intput1 = new ZMotionInput(motionController,"Input1",0);
            ZMotionOutput output1 = new ZMotionOutput(motionController,"Output1", 0);
            ZMotionAxis axis1 = new ZMotionAxis(motionController,0, "Axis1");

            motionController.LoadConfig("config.json");
            var val = intput1.Value;
        }
    }
}
