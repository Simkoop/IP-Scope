using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.DirectoryServices;
using System.Threading;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Diagnostics;
using System.Configuration;

namespace IPScanner
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            Globals.LoadSettings();
            Globals.DevSettingList = Globals.LoadDeviceSettings();
            Globals.Form1 = this;

            IEnumerable<IPAddress> ips = IP.GetIpsForNetworkAdapters();

            foreach (IPAddress ip in ips)
            {
                if (ip.ToString() == "127.0.0.1") continue;
                string[] ipSplit = ip.ToString().Split('.');
                if (ipSplit.Length < 4) continue;
                comboBox1.Items.Add($"{ipSplit[0]}.{ipSplit[1]}.{ipSplit[2]}.*");
                comboBox1.Items.Add($"{ipSplit[0]}.{ipSplit[1]}.*.*");
            }

            Color clr = ThemeInfo.GetThemeColor();
            pnl_BackTop.BackColor = clr;
            pnl_BackSide.BackColor = clr;
            pnl_BackMain.BackColor = clr;
            pnl_BackSideTop.BackColor = clr;
            pnl_BackSideMiddle.BackColor = clr;
            pnl_ScanProgress.BackColor = clr;
            pnl_BackDeviceFound.BackColor = clr;
            pnl_BackScroller.BackColor = clr;
            pnl_BackDrag.BackColor = clr;
            lbl_Title.ForeColor = clr;
            lbl_DevicesFound.ForeColor = clr;


            //MessageBox.Show(IP.GetMac("192.168.1.3"));
            string name = Dns.GetHostEntry("").HostName.ToString();
        }

        int toscanIPcount = 1;
        int scannedIPcount = 1;
        int threadCount = 0;

        private void button1_Click(object sender, EventArgs e)
        {
            btn_Scan.Text = "Scanning..";
            btn_Scan.Enabled = false;
            for (int i = 0; i < pnl_Devices.Controls.Count; i++)
            {
                pnl_Devices.Controls[i].Dispose();
            }
            pnl_Devices.Controls.Clear();
            scrollerLoc = 0;
            pb_Scroller.Refresh();

            Thread T = new Thread(StartScanThread);
            T.Start(comboBox1.Text);
            
        }

        private void StartScanThread(object text)
        {
            List<string> IPList = IP.GetLanIPs((string)text);
            toscanIPcount = IPList.Count;
            scannedIPcount = 0;
            foreach (string ip in IPList)
            {
                Thread T = new Thread(ScanThread);
                T.Start(ip);
                threadCount++;
                while (threadCount > Globals.ScanThreads)
                {
                    //Keep thread alive in release builds, y needed tho????
                    SendBackData("");
                    Thread.Sleep(100);
                }
            }
        }

        private void ScanThread(object ipString)
        {
            string ip = (string)ipString;
            IP ipobj = new IP();
            ipobj.ScanIP(ip, ScannedItemCallback);
        }

        public void SendBackData(string text)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(SendBackData), text);
                return;
            }

        }

        public void ScannedItemCallback(Device device)
        {

            if (InvokeRequired)
            {
                Invoke(new Action<Device>(ScannedItemCallback), device);
                return;
            }            


            threadCount--;
            scannedIPcount++;
            UpdateProgressBar();
            

            if (scannedIPcount >= toscanIPcount)
            {
                btn_Scan.Text = "Scan";
                btn_Scan.Enabled = true;
            }


            if (device.IPAddress == null) return;


            UIDevice uidev = new UIDevice(device, ThemeInfo.GetThemeColor());
            pnl_Devices.Controls.Add(uidev);
            uidev.Dock = DockStyle.Top;
            pnl_Devices.Height = pnl_Devices.Controls.Count * 60;
            pb_Scroller.Refresh();
        }

        public void UpdateProgressBar()
        {
            double a = (pnl_ProgressBack.Width - 8.000000) / (double)toscanIPcount;
            double b = a * scannedIPcount;
            int res = Convert.ToInt32(b);
            pnl_ScanProgress.Width = res;
        }

        bool dragging = false;
        Point dragStartPoint; 
        private void WindowDragStart(object sender, MouseEventArgs e)
        {
            dragStartPoint = e.Location;
            dragging = true;
        }

        private void WindowDragEnd(object sender, MouseEventArgs e)
        {
            dragging = false;
        }

        private void WindowDragMove(object sender, MouseEventArgs e)
        {
            if (!dragging) return;
            
            this.Location = new Point(MousePosition.X - dragStartPoint.X - pnl_BackTop.Left - pnl_Top.Left, MousePosition.Y - dragStartPoint.Y - pnl_BackTop.Top - pnl_Top.Top);
        }

        private void pb_Drag_MouseMove(object sender, MouseEventArgs e)
        {
            if (!dragging) return;

            this.Location = new Point(MousePosition.X - dragStartPoint.X - pnl_BackTop.Left - pnl_BackDrag.Left - pnl_Drag.Left - pb_Drag.Left, MousePosition.Y - dragStartPoint.Y - pnl_BackTop.Top - pnl_BackDrag.Top - pnl_Drag.Top - pb_Drag.Top);
        }

        int oldScrollerLoc = 0;
        bool scrolling = false;
        Point scrollStartPoint;
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            oldScrollerLoc = scrollerLoc;
            scrollStartPoint = e.Location;
            scrolling = true;
            pb_Scroller.Refresh();
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            scrolling = false;
            pb_Scroller.Refresh();
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (scrolling)
            {
                scrollerLoc = oldScrollerLoc - (scrollStartPoint.Y - e.Location.Y);

                if (scrollerLoc > scrollerMax)
                    scrollerLoc = scrollerMax;
                else if (scrollerLoc < 0)
                    scrollerLoc = 0;

            }
            ScrollerMouseLoc = e.Location;
            pb_Scroller.Refresh();
        }
        private void pb_Scroller_MouseLeave(object sender, EventArgs e)
        {
            ScrollerMouseLoc = new Point(0,0);
            pb_Scroller.Refresh();
        }

        Point ScrollerMouseLoc;
        int scrollerLoc = 0;
        int scrollerMax = 0;
        private void pb_Scroller_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(Color.FromArgb(32,32,32));

            int screenspaceHeight = pnl_PnlDevices.Height;
            int panelHeight = pnl_Devices.Height;

            int width = pb_Scroller.Width;
            int height = pb_Scroller.Height;

            double a = 1.0000 * (screenspaceHeight - 8) / (panelHeight);

            Color clr_normal = ThemeInfo.GetThemeColor();
            Color clr_Hover = Color.FromArgb(
                Math.Min(255, clr_normal.R + 35),
                Math.Min(255, clr_normal.G + 35),
                Math.Min(255, clr_normal.B + 35));
            Color clr_Click = Color.FromArgb(
                Math.Min(255, clr_normal.R + 55),
                Math.Min(255, clr_normal.G + 55),
                Math.Min(255, clr_normal.B + 55));
            SolidBrush brush = new SolidBrush(clr_normal);


            if (a >= 1)
            {
                if (ScrollerMouseLoc.X > 4 && ScrollerMouseLoc.X < width - 4 && ScrollerMouseLoc.Y > 4 && ScrollerMouseLoc.Y < height - 4)
                {
                    if (scrolling)
                        brush = new SolidBrush(clr_Click);
                    else
                        brush = new SolidBrush(clr_Hover);
                }
                g.FillRectangle(brush, new Rectangle(new Point(4, 4), new Size(width - 8, height - 8)));
            }
            else
            {
                int ScrollBarHeight = Convert.ToInt32((height) * a);

                if (ScrollerMouseLoc.X > 4 && ScrollerMouseLoc.X < width - 4 && ScrollerMouseLoc.Y > scrollerLoc && ScrollerMouseLoc.Y < ScrollBarHeight + scrollerLoc)
                {
                    if (scrolling)
                        brush = new SolidBrush(clr_Click);
                    else
                        brush = new SolidBrush(clr_Hover);
                }
                if (scrolling)
                    brush = new SolidBrush(clr_Click);

                pnl_Devices.Top = Convert.ToInt32(-scrollerLoc / a);
                scrollerMax = pnl_BackScroller.Height - 8 - ScrollBarHeight;
                g.FillRectangle(brush, new Rectangle(new Point(4, 4 + scrollerLoc), new Size(width - 8, ScrollBarHeight)));
            }

        }


        bool IsCollapsed = false;
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (IsCollapsed)
            {
                this.Height = 450;
                this.Width = 800;
                this.TopMost = false;
                pb_Collapse.BackgroundImage = IPScanner.Properties.Resources.Collapse;
                pnl_ProgressBack.Parent = pnl_BackSideMiddle;
                pnl_ProgressBack.BringToFront();
                pnl_BackDrag.Width = 0;
            }
            else
            {
                this.Height = 40;
                this.Width = 800;
                this.TopMost = true;
                pb_Collapse.BackgroundImage = IPScanner.Properties.Resources.Expand;
                pnl_ProgressBack.Parent = pnl_Top;
                pnl_ProgressBack.BringToFront();
                pnl_BackDrag.Width = 50;
            }

            UpdateProgressBar();
            IsCollapsed = !IsCollapsed;
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            Globals.SaveSettings();
            Globals.SaveDeviceSettings(Globals.DevSettingList);
            Globals.Closing = true;
            this.Close();
        }

        private void btn_Minimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        public void ViewDevice(Device device)
        {
            //Clear previous control
            pnl_Help.Parent = pnl_BackMain;
            pnl_Help.Visible = false;
            for (int i = 0; i < pnl_Main.Controls.Count; i++)
            {
                pnl_Main.Controls[i].Dispose();
            }
            pnl_Main.Controls.Clear();
            Globals.SettingsOpen = false;

            DeviceView devView = new DeviceView(device, ThemeInfo.GetThemeColor());
            devView.Parent = pnl_Main;
            devView.Dock = DockStyle.Fill;
            pnl_BackMain.BackColor = devView.UIColor;
        }

        public void SetMainBackColor(Color clr)
        {
            pnl_BackMain.BackColor = clr;
        }

        private void pb_Settings_Click(object sender, EventArgs e)
        {
            //Clear previous control
            pnl_Help.Parent = pnl_BackMain;
            pnl_Help.Visible = false;
            for (int i = 0; i < pnl_Main.Controls.Count; i++)
            {
                pnl_Main.Controls[i].Dispose();
            }
            pnl_Main.Controls.Clear();

            if (Globals.SettingsOpen)
            {
                Globals.SettingsOpen = false;
                return;
            }

            Globals.SettingsOpen = true;
            Settings setting = new Settings(ThemeInfo.GetThemeColor());
            setting.Parent = pnl_Main;
            setting.Dock = DockStyle.Fill;
            pnl_BackMain.BackColor = ThemeInfo.GetThemeColor();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Globals.SaveDeviceSettings(Globals.DevSettingList);
            Globals.SaveSettings();
            Globals.Closing = true;
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < pnl_Main.Controls.Count; i++)
            {
                pnl_Main.Controls[i].Dispose();
            }
            pnl_Main.Controls.Clear();
            Globals.SettingsOpen = false;

            pnl_Help.Visible = true;
            pnl_Help.Parent = pnl_Main;
        }
    }

    public static class Globals
    {
        public static bool Closing = false;

        public static int defScanThreads = 100;
        public static int defScanTimeout = 4000;
        public static bool defFastScan = false;
        public static int ScanThreads = defScanThreads;
        public static int ScanTimeout = defScanTimeout;
        public static bool FastScan = defFastScan;
        public static bool SettingsOpen = false;

        public static Form1 Form1;
        public static UIDevice ActiveUIDevice = null;
        public static List<DeviceSettings> DevSettingList;

        public static void SaveDeviceSettings(List<DeviceSettings> deviceSettingsList)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(ms, deviceSettingsList);
                ms.Position = 0;
                byte[] buffer = new byte[(int)ms.Length];
                ms.Read(buffer, 0, buffer.Length);
                Properties.Settings.Default.DeviceSettings = Convert.ToBase64String(buffer);
                Properties.Settings.Default.Save();
            }
        }

        public static List<DeviceSettings> LoadDeviceSettings()
        {
            using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(Properties.Settings.Default.DeviceSettings)))
            {
                if (ms.Length == 0)
                {
                    List<DeviceSettings> devs = new List<DeviceSettings>();
                    devs.Add(new DeviceSettings("00:00:00:00:00"));
                    return devs;
                }
                BinaryFormatter bf = new BinaryFormatter();
                return (List<DeviceSettings>)bf.Deserialize(ms);
            }
        }

        public static void SaveSettings()
        {
            Properties.Settings.Default.ScanThreads = ScanThreads;
            Properties.Settings.Default.ScanTimeout = ScanTimeout;
            Properties.Settings.Default.FastScan = FastScan;
        }
        public static void LoadSettings()
        {
            if (Properties.Settings.Default.ScanThreads == 0) return;
            ScanThreads = Properties.Settings.Default.ScanThreads;
            ScanTimeout = Properties.Settings.Default.ScanTimeout;
            FastScan = Properties.Settings.Default.FastScan;
        }

    }
}
