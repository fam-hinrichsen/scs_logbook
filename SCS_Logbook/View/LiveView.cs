using SCSSdkClient;
using SCSSdkClient.Object;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace SCS_Logbook.View
{
    public partial class LiveView : Form
    {
        public delegate void UpdateDelegate(SCSTelemetry telemetry);
        bool isClosing;

        public LiveView()
        {
            InitializeComponent();
        }

        public void Update(SCSTelemetry telemetry)
        {
            if (InvokeRequired)
            {
                Invoke(new UpdateDelegate(Update), telemetry);
            }
            else
            {
                int limit = (int)telemetry.NavigationValues.SpeedLimit.Kph;
                int speed = (int)telemetry.TruckValues.CurrentValues.DashboardValues.Speed.Kph;
                SetSpeed(speed, limit);
            }
        }

        private void SetSpeed(int speed, int limit)
        {
            string speedText = speed.ToString();
            while(speedText.Length < 3)
            {
                speedText = "0" + speedText;
            }

            if(speed > limit && speed < limit + 5)
            {
                lbl_speed.ForeColor = Color.Yellow;
            }
            else if (speed > limit && speed < limit + 10)
            {
                lbl_speed.ForeColor = Color.Orange;
            }
            else if (speed > limit)
            {
                lbl_speed.ForeColor = Color.Red;
            }
            else
            {
                lbl_speed.ForeColor = Color.Black;
            }

            lbl_speed.Text = speedText;
        }

        private void LiveView_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isClosing)
            {
                return;
            }

            isClosing = true;
            Logbook.Instance.closeView(GetType());
        }
    }
}
