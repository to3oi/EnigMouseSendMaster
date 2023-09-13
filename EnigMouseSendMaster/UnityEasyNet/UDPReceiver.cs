using System.Net;
using System.Net.Sockets;

namespace UnityEasyNet
{
    /// <summary>
    /// UDPの受信の開始と受け取りをする
    /// </summary>
    public class UDPReceiver : IDisposable
    {
        //TODO:IDisposableをよくわかってないのでDisposeの処理を実行するタイミングがない？

        private UdpClient mUDP;

        /// <summary>
        /// データを受信した際に受信したデータを通知する
        /// </summary>
        public Action<byte[]> OnDataReceivedBytes;

        /// <summary>
        /// データを受信した際に送信元の情報を通知する
        /// </summary>
        public Action<IPEndPoint> OnIPEndPointReceived;

        #region Constructors

        /// <summary>
        /// 指定したポート番号でUDPの受信を開始します
        /// </summary>
        /// <param name="_port">受信するポート番号</param>
        /// <param name="_OnDataReceivedBytes">受信したデータを通知するメソッド</param>
        public UDPReceiver(int _port, Action<byte[]> _OnDataReceivedBytes)
        {
            try
            {
                //アクションの登録
                OnDataReceivedBytes = _OnDataReceivedBytes;

                mUDP = new UdpClient(_port);
                mUDP.BeginReceive(UDPReceive, mUDP);
                Console.WriteLine($"受信開始");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        /// <summary>
        /// 指定したポート番号でUDPの受信を開始します
        /// </summary>
        /// <param name="_port">受信するポート番号</param>
        /// <param name="_OnDataReceivedBytes">受信したデータを通知するメソッド</param>
        /// <param name="_OnIPEndPointReceived">送信元のIPEndPointを通知するメソッド</param>
        public UDPReceiver(int _port, Action<byte[]> _OnDataReceivedBytes, Action<IPEndPoint> _OnIPEndPointReceived)
        {
            try
            {
                //アクションの登録
                OnDataReceivedBytes = _OnDataReceivedBytes;
                OnIPEndPointReceived = _OnIPEndPointReceived;

                mUDP = new UdpClient(_port);
                mUDP.BeginReceive(UDPReceive, mUDP);
                Console.WriteLine($"受信開始");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        /// <summary>
        /// 指定したポート番号でUDPの受信を開始します
        /// </summary>
        /// <param name="_port">受信するポート番号</param>
        /// <param name="_OnIPEndPointReceived">送信元のIPEndPointを通知するメソッド</param>
        public UDPReceiver(int _port, Action<IPEndPoint> _OnIPEndPointReceived)
        {
            try
            {
                //アクションの登録
                OnIPEndPointReceived = _OnIPEndPointReceived;

                mUDP = new UdpClient(_port);
                mUDP.BeginReceive(UDPReceive, mUDP);
                Console.WriteLine($"受信開始");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        #endregion

        /// <summary>
        /// 受信したときの処理
        /// </summary>
        /// <param name="res"></param>
        void UDPReceive(IAsyncResult res)
        {
            UdpClient getUDP = (UdpClient)res.AsyncState;
            IPEndPoint ipEnd = null;
            
            byte[] bytes = getUDP.EndReceive(res, ref ipEnd);
            try
            {
                //byte[]を通知
                OnDataReceivedBytes.Invoke(bytes);
                //IPEndPointを通知
                OnIPEndPointReceived?.Invoke(ipEnd);
            }
            catch (SocketException e)
            {
                Console.WriteLine(e.ToString());
                return;
            }
            catch (ObjectDisposedException e)
            {
                Console.WriteLine(e.ToString());
                return;
            }

            //再度受信
            getUDP.BeginReceive(UDPReceive, getUDP);
        }

        public void Dispose()
        {
            mUDP?.Dispose();
        }
    }
}