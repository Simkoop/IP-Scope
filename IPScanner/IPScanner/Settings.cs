using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IPScanner
{
    public partial class Settings : UserControl
    {
        Color UIColor;
        public Settings(Color UIColor)
        {
            InitializeComponent();
            this.UIColor = UIColor;

            tmr_ThemeCheck.Start();
            ApplyTheme();
            SetValues(Globals.ScanThreads, Globals.ScanTimeout, Globals.FastScan);
        }

        public void ApplyTheme()
        {
            lbl_IP.ForeColor = UIColor;
            lbl_RestartProgram.ForeColor = UIColor;
            pnl_BackTop.BackColor = UIColor;
        }

        public void SetValues(int ScanThreads, int ScanTimeout, bool FastScan)
        {
            nud_ScanThreads.Value = ScanThreads;
            nud_ScanTimeout.Value = ScanTimeout;
            cb_FastScan.Checked = FastScan;
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            Globals.ScanThreads = trb_ScanThreads.Value;
            Globals.ScanTimeout = trb_ScanTimeout.Value;
            Globals.FastScan = cb_FastScan.Checked;
            Globals.SaveSettings();
        }

        bool FromScroll = false;
        private void trb_ScanThreads_Scroll(object sender, EventArgs e)
        {
            FromScroll = true;
            nud_ScanThreads.Value = trb_ScanThreads.Value;
        }

        private void trb_ScanTimeout_Scroll(object sender, EventArgs e)
        {
            FromScroll = true;
            nud_ScanTimeout.Value = trb_ScanTimeout.Value;
        }

        private void nud_ScanThreads_ValueChanged(object sender, EventArgs e)
        {
            if (FromScroll)
            {
                FromScroll = false;
                return;
            }

            trb_ScanThreads.Value = (int)nud_ScanThreads.Value;
        }

        private void nud_ScanTimeout_ValueChanged(object sender, EventArgs e)
        {
            if (FromScroll)
            {
                FromScroll = false;
                return;
            }

            trb_ScanTimeout.Value = (int)nud_ScanTimeout.Value;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Process.Start("ms-settings:colors");
        }

        private void tmr_ThemeCheck_Tick(object sender, EventArgs e)
        {
            if (ThemeInfo.GetThemeColor() != ThemeInfo.GetUpdatedThemeColor())
            {
                lbl_RestartProgram.Show();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Globals.ScanThreads = Globals.defScanThreads;
            Globals.ScanTimeout = Globals.defScanTimeout;
            Globals.FastScan = Globals.defFastScan;
            Globals.SaveSettings();
            SetValues(Globals.ScanThreads, Globals.ScanTimeout, Globals.FastScan);
        }

        private void btn_Clear_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(
                "Are you sure you want to reset saved device info?" + Environment.NewLine +  
                "(This action can not be undone)", 
                "Warning", 
                MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Globals.DevSettingList.Clear();
                Globals.SaveDeviceSettings(Globals.DevSettingList);

                btn_Clear.Text = "Cleared";
                btn_Clear.Enabled = false;
            }
        }
    }
}
