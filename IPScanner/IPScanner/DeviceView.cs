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
    public partial class DeviceView : UserControl
    {
        Device device;
        public Color UIColor;
        public DeviceView(Device device, Color UIColor)
        {
            InitializeComponent();
            this.device = device;
            this.UIColor = UIColor;

            if(device.HasSettingsLoaded)
                this.UIColor = device.UIColor;

            UpdateUI();
        }
        public void UpdateUI()
        {
            lbl_IP.ForeColor = UIColor;
            pnl_BackTop.BackColor = UIColor;

            lbl_IP.Text = device.IPAddress;
            lbl_MAC.Text = device.MAC;
            tb_Name.Text = device.Hostname;
            lbl_Ping.Text = $"Ping: {device.PingTime}ms";
            tb_Notes.Text = device.Notes;
        }

        private void btn_ChangeColor_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                UIColor = colorDialog1.Color;
                UpdateUI();
            }
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            foreach(DeviceSettings dev in Globals.DevSettingList)
            {
                if (dev.MAC != lbl_MAC.Text) continue;

                dev.Name = tb_Name.Text;
                dev.Notes = tb_Notes.Text;
                dev.DeviceColor = UIColor;
                device.LoadSettings();
                Globals.ActiveUIDevice.InvertedUI();
                Globals.Form1.SetMainBackColor(dev.DeviceColor);
                Globals.SaveDeviceSettings(Globals.DevSettingList);
                return;
            }

            DeviceSettings devAdd = new DeviceSettings(lbl_MAC.Text);
            devAdd.Name = tb_Name.Text;
            devAdd.Notes = tb_Notes.Text;
            devAdd.DeviceColor = UIColor;
            device.LoadSettings();
            Globals.DevSettingList.Add(devAdd);
            Globals.ActiveUIDevice.InvertedUI();
            Globals.Form1.SetMainBackColor(devAdd.DeviceColor);
            Globals.SaveDeviceSettings(Globals.DevSettingList);
        }
    }
}
