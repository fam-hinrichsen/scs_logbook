using log4net;
using SCS_Logbook.Extensions;
using SCSSdkClient.Object;
using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace SCS_Logbook.View
{
    public partial class LiveView : Form
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public delegate void UpdateDelegate();

        private readonly object telemetryDataLock = new object();
        private SCSTelemetry telemetryData;
        private bool isClosing;
        private bool doExit;
        private readonly Thread updateThread;

        public LiveView()
        {
            InitializeComponent();
            updateThread = new Thread(Updater);
            updateThread.Name = "LiveView_fieldUpdater";
            updateThread.Start();
        }

        public void Update(SCSTelemetry telemetry)
        {
            if (Monitor.TryEnter(telemetryDataLock))
            {
                telemetryData = telemetry;
                Monitor.Exit(telemetryDataLock);
            }
        }

        private void Updater()
        {
            while (!doExit)
            {
                UpdateFields();
                Thread.Sleep(100);
            }
        }

        private void UpdateFields()
        {
            if (InvokeRequired)
            {
                Invoke(new UpdateDelegate(UpdateFields));
            }
            else
            {
                try { 
                    Monitor.Enter(telemetryDataLock);
                    if(telemetryData != null)
                    {
                        int speed = (int)telemetryData.TruckValues.CurrentValues.DashboardValues.Speed.Kph;
                        int limitSpeed = (int)telemetryData.NavigationValues.SpeedLimit.Kph;

                        int rpm = (int)telemetryData.TruckValues.CurrentValues.DashboardValues.RPM;
                        int limitRpm = (int)telemetryData.TruckValues.ConstantsValues.MotorValues.EngineRpmMax;

                        SetSpeed(speed, limitSpeed);
                        SetRpm(rpm, limitRpm);
                    }
                }
                catch(Exception ex)
                {
                    log.Error("Could not update LiveView fields.", ex);
                }
                finally
                {
                    Monitor.Exit(telemetryDataLock);
                }
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

        private void SetRpm(int rpm, int limit)
        {
            string text = rpm.ToString();
            while (text.Length < 4)
            {
                text = "0" + text;
            }

            if (rpm >= limit * 0.95)
            {
                lbl_rpm.ForeColor = Color.Red;
                pb_rpm.SetState(2);
            }
            else if (rpm >= limit * 0.90)
            {
                lbl_rpm.ForeColor = Color.Orange;
                pb_rpm.SetState(2);
            }
            else if (rpm >= limit * 0.80)
            {
                lbl_rpm.ForeColor = Color.Yellow;
                pb_rpm.SetState(3);
            }
            else
            {
                lbl_rpm.ForeColor = Color.Black;
                pb_rpm.SetState(1);
            }

            pb_rpm.Maximum = limit;
            pb_rpm.Value = rpm;
            lbl_rpm.Text = text;
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
