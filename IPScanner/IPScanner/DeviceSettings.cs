using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPScanner
{
    [Serializable()]
    public class DeviceSettings
    {
        public string MAC;
        public string Name;
        public string Notes;
        public Color DeviceColor;

        public DeviceSettings(string MAC)
        {
            this.MAC = MAC;
        }

    }
}
