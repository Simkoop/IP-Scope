using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IPScanner
{
    class IP
    {
        public static IEnumerable<IPAddress> GetIpsForNetworkAdapters()
        {

            var nics = from i in NetworkInterface.GetAllNetworkInterfaces()
                       where i.OperationalStatus == OperationalStatus.Up
                       select new { name = i.Name, ip = GetIpFromUnicastAddresses(i) };

            return nics.Select(x => x.ip);
        }

        private static IPAddress GetIpFromUnicastAddresses(NetworkInterface i)
        {
            return (from ip in i.GetIPProperties().UnicastAddresses
                    where ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork
                    select ip.Address).SingleOrDefault();
        }


        public static List<string> GetLanIPs(string scanScope)
        {
            string[] splitList = scanScope.Split('.');
            if (splitList.Length < 3) return new List<string>();

            List<string> IPList = new List<string>();
            //First digit
            Point pnt1 = new Point(1, 255);
            if (splitList[0].Contains('-')) pnt1 = new Point(Convert.ToInt32(splitList[0].Split('-')[0]), Convert.ToInt32(splitList[0].Split('-')[1]));
            for (int i1 = pnt1.X; i1 <= pnt1.Y; i1++) { if (splitList[0] != "*" && !splitList[0].Contains('-')) i1 = Convert.ToInt32(splitList[0]);
                
                //Second digit
                Point pnt2 = new Point(1, 255);
                if (splitList[1].Contains('-')) pnt2 = new Point(Convert.ToInt32(splitList[1].Split('-')[1]), Convert.ToInt32(splitList[1].Split('-')[1]));
                for (int i2 = pnt2.X; i2 <= pnt2.Y; i2++) { if (splitList[1] != "*" && !splitList[1].Contains('-')) i2 = Convert.ToInt32(splitList[1]);

                    //Third digit
                    Point pnt3 = new Point(1, 255);
                    if (splitList[2].Contains('-')) pnt3 = new Point(Convert.ToInt32(splitList[2].Split('-')[0]), Convert.ToInt32(splitList[2].Split('-')[1]));
                    for (int i3 = pnt3.X; i3 <= pnt3.Y; i3++) { if (splitList[2] != "*" && !splitList[2].Contains('-')) i3 = Convert.ToInt32(splitList[2]);

                        //Fourth digit
                        Point pnt4 = new Point(1, 255);
                        if (splitList[3].Contains('-')) pnt4 = new Point(Convert.ToInt32(splitList[3].Split('-')[0]), Convert.ToInt32(splitList[3].Split('-')[1]));
                        for (int i4 = pnt4.X; i4 <= pnt4.Y; i4++) { if (splitList[3] != "*" && !splitList[3].Contains('-')) i4 = Convert.ToInt32(splitList[3]);

                            IPList.Add($"{i1}.{i2}.{i3}.{i4}");

                            if (splitList[3] != "*" && !splitList[3].Contains('-')) i4 = 255; }

                        if (splitList[2] != "*" && !splitList[2].Contains('-')) i3 = 255; }

                    if (splitList[1] != "*" && !splitList[1].Contains('-')) i2 = 255; }

                if (splitList[0] != "*" && !splitList[0].Contains('-')) i1 = 255; }


            

            return IPList;
        }

        [DllImport("iphlpapi.dll", ExactSpelling = true)]
        public static extern int SendARP(int destIp, int srcIP, byte[] macAddr, ref uint physicalAddrLen);
        public static string GetMac(string ipAddress)
        {
            IPAddress dst = IPAddress.Parse(ipAddress); // the destination IP address

            byte[] macAddr = new byte[6];
            uint macAddrLen = (uint)macAddr.Length;

            try
            {
                if (SendARP(BitConverter.ToInt32(dst.GetAddressBytes(), 0), 0, macAddr, ref macAddrLen) != 0)
                    throw new InvalidOperationException("SendARP failed.");
            }
            catch
            {

            }

            string[] str = new string[(int)macAddrLen];
            for (int i = 0; i < macAddrLen; i++)
                str[i] = macAddr[i].ToString("x2");

            Console.WriteLine(string.Join(":", str));
            return string.Join(":", str);
        }


        public delegate void ScannedIPCallback(Device device);
        public void ScanIP(string IP, ScannedIPCallback callback)
        {
            Ping ping = new Ping();
            PingReply reply;
            reply = ping.Send(IP, Globals.ScanTimeout);


            if (reply.Status == IPStatus.Success) {
                string hostName = "";
                try {
                    IPHostEntry hostEntry = Dns.GetHostEntry(IP);
                    hostName = hostEntry.HostName;
                }
                catch
                {

                }

                string mac = GetMac(IP);

                //MessageBox.Show(IP + Environment.NewLine + hostName + Environment.NewLine + mac);
                string PingTime = reply.RoundtripTime.ToString();
                Device newDev = new Device(IP, hostName, mac, PingTime);

                callback(newDev);
                return;
            }
            callback(Device.Null());
        }
    }
}
