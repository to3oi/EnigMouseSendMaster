using System.Text;
using UnityEasyNet;

namespace EnigMouseSendMaster
{
    /// <summary>
    /// ClientPCの情報
    /// </summary>
    public struct ClientPCInfo
    {
        public bool WaitingForInput { get; set; } = false;
        public UDPSender UDPSender { get; set; } = null;
        public string IP_Address { get; set; }

        public ClientPCInfo(string iP_Address)
        {
            IP_Address = iP_Address;
        }
        public void CommunicationReceive((byte[] bytes, int readCount) tuple)
        {
            var s = Encoding.UTF8.GetString(tuple.bytes, 0, tuple.readCount);
            
            //接続の確立
            if(s == "connecting")
            {
                Form1.Instance.ClientPCInfos.Add(this);
                Form1.Instance.AddClientPCIPList(IP_Address);
            }
            //TODO:接続の解除
            if (s == "disConnecting")
            {
                Form1.Instance.ClientPCInfos.Remove(this);
            }
            Console.WriteLine(s == "connecting");
        }

    }
}
