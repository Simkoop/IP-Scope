using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IPScanner
{
    public partial class UIDevice : UserControl
    {
        public Color UIdark = Color.FromArgb(32,32,32);
        public Color UIhover = Color.FromArgb(42,42,42);
        public Color UIColor;
        public Color UIColorBackup;
        public Device device;
        public UIDevice(Device device, Color UIColor)
        {
            InitializeComponent();

            this.device = device;
            this.UIColor = UIColor;
            this.UIColorBackup = UIColor;

            if (device.HasSettingsLoaded)
            {
                this.UIColor = device.UIColor;
                this.UIColorBackup = device.UIColor;
            }


            NormalUI();
        }

        public void InvertedUI()
        {
            if (device.HasSettingsLoaded)
            {
                UIColor = device.UIColor;
                UIColorBackup = device.UIColor;
            }

            UIdark = Color.FromArgb(52, 52, 52);
            UIhover = Color.FromArgb(52, 52, 52);

            UIColor = Color.FromArgb(
                Math.Min(255, UIColorBackup.R + 35),
                Math.Min(255, UIColorBackup.G + 35),
                Math.Min(255, UIColorBackup.B + 35));

            UpdateUI();

        }

        public void NormalUI()
        {
            if (device.HasSettingsLoaded)
            {
                UIColor = device.UIColor;
                UIColorBackup = device.UIColor;
            }

            UIColor = UIColorBackup;
            UIdark = Color.FromArgb(32, 32, 32);
            UIhover = Color.FromArgb(42, 42, 42);

            UpdateUI();
        }

        public void UpdateUI()
        {           
            this.BackColor = UIColor;
            lbl_IP.ForeColor = UIColor;

            lbl_IP.Text = device.IPAddress;
            lbl_MAC.Text = device.MAC;
            lbl_Name.Text = device.Hostname;
            if (device.PingTime != null) lbl_Ping.Text = $"{device.PingTime}ms";

            if (device.MAC == "") lbl_MAC.Text = "-";
            if (device.Hostname == "") lbl_Name.Text = "-";

            pnl_Body.BackColor = UIdark;
        }


        private void HoverEnter(object sender, EventArgs e)
        {
            pnl_Body.BackColor = UIhover;
        }

        private void HoverLeave(object sender, EventArgs e)
        {
            pnl_Body.BackColor = UIdark;
        }

        private void Clicked(object sender, EventArgs e)
        {
            Globals.ActiveUIDevice = this;
            InvertedUI();
            tmr_Uninvert.Start();
            Globals.Form1.ViewDevice(device);
        }

        private void tmr_Uninvert_Tick(object sender, EventArgs e)
        {
            if (Globals.ActiveUIDevice != this)
            {
                NormalUI();
                tmr_Uninvert.Stop();
            }
        }
    }
}
