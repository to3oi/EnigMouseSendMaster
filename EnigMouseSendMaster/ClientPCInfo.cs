using UnityEasyNet;

namespace EnigMouseSendMaster
{
    public struct ClientPCInfo
    {
        public bool WaitingForInput { get; set; } = false;
        public UDPSender UDPSender { get; private set; }
        public string IP_Address { get; private set; }

        public ClientPCInfo(UDPSender uDPSender, string iP_Address)
        {
            UDPSender = uDPSender;
            IP_Address = iP_Address;
        }
    }
}
