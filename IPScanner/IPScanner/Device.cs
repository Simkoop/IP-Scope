using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPScanner
{
    public class Device
    {
        public string IPAddress;
        public string Hostname;
        public string MAC;
        public string PingTime;
        public string Notes;
        public Color UIColor;
        public bool HasSettingsLoaded = false;

        public Device(string IPAddress, string Hostname, string MAC, string PingTime = null)
        {
            this.IPAddress = IPAddress;
            this.Hostname = Hostname;
            this.MAC = MAC;
            this.PingTime = PingTime;

            LoadSettings();
        }

        public void LoadSettings()
        {
            foreach (DeviceSettings dev in Globals.DevSettingList)
            {
                if (dev.MAC != MAC) continue;

                Hostname = dev.Name;
                Notes = dev.Notes;
                UIColor = dev.DeviceColor;
                HasSettingsLoaded = true;
                return;
            }
        }

        public static Device Null()
        {
            return new Device(null, null, null);
        }
    }
}
