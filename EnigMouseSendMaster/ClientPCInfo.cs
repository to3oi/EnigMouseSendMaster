using System.Net;
using System.Text;
using UnityEasyNet;

namespace EnigMouseSendMaster
{
    /// <summary>
    /// ClientPCの情報
    /// </summary>
    public class ClientPCInfo : IDisposable
    {
        #region UDPSender
        /// <summary>
        /// 通信の確立を送信するUDPSender
        /// </summary>
        public UDPSender ClientPC_UDPSender = null;

        /// <summary>
        /// 画像を送信する UDPSender 
        /// </summary>
        public UDPSender Image_UDPSender = null;
        #endregion

        /// <summary>
        /// trueで結果を待機中
        /// </summary>
        public bool WaitingForInput { get; set; } = false;
        public string IP_Address { get; set; }

        public ClientPCInfo(string iP_Address)
        {
            IP_Address = iP_Address;
            ClientPC_UDPSender = new UDPSender(iP_Address, Form1.CommunicationSendPort);
            ClientPC_UDPSender.Send(Encoding.UTF8.GetBytes("connecting"));

            Image_UDPSender = new UDPSender(iP_Address, Form1.ImageSendPort);

        }

        public void SendImage(byte[] bytes)
        {
            Image_UDPSender?.Send(bytes);
        }
        public void SetWait(bool b)
        {
            WaitingForInput = b;

        }
        public void Dispose()
        {
            ClientPC_UDPSender?.Dispose();
            Image_UDPSender?.Dispose();

        }
    }
}
