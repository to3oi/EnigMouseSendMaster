using System.Net;
using System.Text;
using UnityEasyNet;

namespace EnigMouseSendMaster
{
    /// <summary>
    /// ClientPCの情報
    /// </summary>
    public struct ClientPCInfo : IDisposable
    {
        /// <summary>
        /// trueで結果を待機中
        /// </summary>
        public bool WaitingForInput { get; set; } = false;
        public TCPSender TCPSender { get; set; } = null;
        public UDPSender ImageUDPSender { get; set; } = null;
        public string IP_Address { get; set; }

        public ClientPCInfo(string iP_Address)
        {
            IP_Address = iP_Address;


        }
        public void testUDPSetUp()
        {
            IPEndPoint Ep = new IPEndPoint(IPAddress.Parse(IP_Address), Form1.ImageSendPort);
            ImageUDPSender = new UDPSender(Ep);
            Form1.Instance.ClientPCInfos.Add(this);
            Form1.Instance.AddClientPCIPList(IP_Address);
        }
        public void CommunicationReceive((byte[] bytes, int readCount) tuple)
        {
            var s = Encoding.UTF8.GetString(tuple.bytes, 0, tuple.readCount);


            //接続の確立
            if (s == "connecting")
            {
                Console.WriteLine(s == "connecting");
                Form1.Instance.ClientPCInfos.Add(this);
                Form1.Instance.AddClientPCIPList(IP_Address);

                //if (ImageUDPSender != null) { return; }

                IPEndPoint Ep = new IPEndPoint(IPAddress.Parse(IP_Address), Form1.ImageSendPort);
                ImageUDPSender = new UDPSender(Ep);
                Console.WriteLine(s == "connecting");

            }
            //TODO:接続の解除
            if (s == "disConnecting")
            {
                Form1.Instance.ClientPCInfos.Remove(this);
            }
        }

        public void SendImage(byte[] bytes)
        {
            Console.WriteLine("Send");
            ImageUDPSender?.Send(bytes);
        }

        public void Dispose()
        {
            TCPSender?.Dispose();
            ImageUDPSender?.Dispose();

        }
    }
}
