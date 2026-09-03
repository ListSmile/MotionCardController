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
            motionController = new ZMotionController_PCIE("ZMC_PCIE", "");
            ZMotionInput intput1 = new ZMotionInput(motionController, "Input1", 0);
            ZMotionOutput output1 = new ZMotionOutput(motionController, "Output1", 0);
            ZMotionAxis axis1 = new ZMotionAxis(motionController, 0, "Axis1");

            motionController.LoadConfig("config.json");


            var axisreal = motionController.GetAxis(0);

        }
        TestLogic t1;
        private void button2_Click(object sender, EventArgs e)
        {
             t1 = new TestLogic("≤‚ ‘¬ﬂº≠");
            LogicScheduler.Instance.Add(t1);
            LogicScheduler.Instance.Start();
            t1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label1.Text = t1?.CurrentStep.ToString();
        }
    }
}
