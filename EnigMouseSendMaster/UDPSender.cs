using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

namespace UnityEasyNet
{
    /// <summary>
    /// UDPの送信をする
    /// </summary>　
    public class UDPSender : IDisposable
    {
        //TODO:IDisposableをよくわかってないのでDisposeの処理を実行するタイミングがない？
        protected UdpClient mUDP;

        #region Constructors

        /// <summary>
        /// 指定したホスト名のポートにメッセージを送る準備を開始します
        /// </summary>
        /// <param name="_hostName">送信先のリモートホストのDNS名</param>
        /// <param name="_port">送信先のポート番号</param>
        public UDPSender(string _hostName, int _port)
        {
            try
            {
                mUDP = new UdpClient();
                mUDP.Connect(_hostName, _port);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
        }

        /// <summary>
        /// 指定したIPEndPointにメッセージを送る準備を開始します
        /// </summary>
        /// <param name="_ipEndPoint">送信先の情報が入ったIPEndPoint</param>
        public UDPSender(IPEndPoint _ipEndPoint)
        {
            try
            {
                mUDP = new UdpClient();
                mUDP.Connect(_ipEndPoint);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());

            }
        }

        /// <summary>
        /// 指定したIPアドレスのポートにメッセージを送る準備を開始します
        /// </summary>
        /// <param name="_ipAddress">送信先のIPアドレス</param>
        /// <param name="_port">送信先のポート番号</param>
        public UDPSender(IPAddress _ipAddress, int _port)
        {
            try
            {
                mUDP = new UdpClient();
                mUDP.Connect(_ipAddress, _port);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());

            }
        }

        /// <summary>
        /// ローカル内の指定したポートにメッセージを送る準備を開始します
        /// </summary>
        /// <param name="_port">送信先のポート番号</param>
        public UDPSender(int _port)
        {
            try
            {
                mUDP = new UdpClient();
                mUDP.Connect("localhost", _port);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());

            }
        }

        #endregion


        /// <summary>
        /// 登録したポートにbyte配列を送信する
        /// </summary>
        /// <param name="bytes">送信するbyte配列</param>
        public void Send(byte[] bytes)
        {
            try
            {
                mUDP.SendAsync(bytes, bytes.Length);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());

            }
        }

        public void Dispose()
        {
            mUDP?.Dispose();
        }
    }
}