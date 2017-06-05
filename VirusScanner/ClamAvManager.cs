using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace VirusScanner
{
    class ClamAvManager
    {
        private string _host;
        private int _port;
        private string nameVirus;
        private bool isVirus = false;
        public ClamAvManager(string host, int port)
        {
            _host = host;
            _port = port;
        }

        public string NameVirus { get => nameVirus; set => nameVirus = value; }
        public bool IsVirus { get => isVirus; set => isVirus = value; }

        public string Execute(string command)
        {
            string result;
            using (TcpClient client = new TcpClient(_host, _port))
            {
                using (NetworkStream stream = client.GetStream())
                {
                    byte[] data = Encoding.ASCII.GetBytes(command);
                    stream.Write(data, 0, data.Length);
                    StreamReader sr = new StreamReader(stream);
                    result = sr.ReadToEnd();
                }
            }
            return result;
            
        }

        public string GetVer()
        {
            try
            {
                return Execute("VERSION");
            }
            catch (Exception)
            {
                return "Chưa kích hoạt ClamAV";
            }
        }

        public void Scan(string path)
        {
            try
            {
                string result = Execute("SCAN " + path);
                if (result.Contains("FOUND"))
                {
                    IsVirus = true;
                    NameVirus = result.Replace(@"\\?\" + path + ":", "").Replace("FOUND", "");
                }
                else IsVirus = false;
            }
            catch (Exception)
            {
            }
        }
    }
}
