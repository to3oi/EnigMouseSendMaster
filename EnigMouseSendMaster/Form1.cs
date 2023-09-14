using MessagePack;
using Microsoft.Azure.Kinect.Sensor;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Text;
using UnityEasyNet;
using static EnigMouseSendMaster.FilePath;
using BitmapData = System.Drawing.Imaging.BitmapData;
using Image = Microsoft.Azure.Kinect.Sensor.Image;

namespace EnigMouseSendMaster
{
    public partial class Form1 : Form
    {

        #region 外部のスクリプトで取得用
        private static Form1 _form1Instance;
        public static Form1 Instance
        {
            get
            {
                return _form1Instance;
            }
            private set
            {
                _form1Instance = value;
            }
        }

        public void AddClientPCIPList(string s)
        {
            ClientPCIPList.Items.Add(s);
        }
        #endregion

        #region 画像処理関係
        private int _depthDistanceMin = 500;
        private int _depthDistanceMax = 1500;
        private int _depthThresholdMaxColor = 200;

        private int _irDistanceMin = 500;
        private int _irDistanceMax = 1500;
        private int _irThresholdMaxColor = 255;

        private int _depthThresholdMin = 254;
        private int _depthThresholdMax = 255;

        private int _irThresholdMin = 254;
        private int _irThresholdMax = 255;
        #endregion

        //Kinectを扱う変数
        Device kinect;
        //Depth画像のBitmap
        Bitmap depthBitmap;
        //IR画像のBitmap
        Bitmap irBitmap;
        bool loop = true;

        uint saveFileIndex = 0;

        //フレームを固定するのに使用する変数
        DateTime preFrame;


        #region 通信関係の変数

        #region ゲーム本体の通信周り
        private bool isGamePC_UDPSend = false;
        private UDPSender GamePC_UDPSender;
        private string GamePCIPAdress = "localhost";
        private static int GamePCPort = 12001; //ゲーム本体と通信するポート番号
        #endregion

        #region ClientPCの通信周り
        public List<ClientPCInfo> ClientPCInfos = new List<ClientPCInfo>();

        /// <summary>
        /// 通信の確立を送信するポート番号
        /// </summary>
        public static int CommunicationSendPort = 12010;

        /// <summary>
        /// 通信の確立を受け取るポート番号
        /// </summary>
        public static int CommunicationResponsPort = 12011;

        /// <summary>
        /// 画像の送信をするポート番号
        /// </summary>
        public static int ImageSendPort = 12012;

        /// <summary>
        /// 物体検出の結果を取得するポート番号
        /// </summary>
        public static int ResultReceivePort = 12013;

        #endregion

        #region UDPReceiver
        /// <summary>
        /// 通信の確立を受信するUDPReceiver
        /// </summary>
        private UDPReceiver ClientPCRespons_UDPReceiver;

        /// <summary>
        /// 物体検出の結果を受信するUDPReceiver 
        /// </summary>
        private UDPReceiver ClientPC_Result_UDPReceiver;
        #endregion

        #endregion


        /// <summary>
        /// IPAddressからClientPCInfoを取得する
        /// ない場合はnullが返る
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        private ClientPCInfo? GetClientPCInfo(string ipAddress)
        {
            foreach (var clientPCInfo in ClientPCInfos)
            {
                if (clientPCInfo.IP_Address == ipAddress)
                {
                    return clientPCInfo;
                }
            }
            return null;
        }

        public Form1()
        {
            Form1.Instance = this;

            InitializeComponent();

            //デバッグ用
            AllocConsole();
            /*            //IPv4のアドレスを取得して表示
                        IPHostEntry ipHostEntry = Dns.GetHostEntry(Dns.GetHostName());

                        foreach (IPAddress ip in ipHostEntry.AddressList)
                        {
                            if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                            {
                                PCViewIpAdress.Text = ip.ToString();
                                break;
                            }
                        }*/

            //UDPReceiverを準備
            ClientPCRespons_UDPReceiver = new UDPReceiver(CommunicationResponsPort, ClientPCRespons_Receive);
            //ClientPC_Result_UDPReceiver = new UDPReceiver();
        }

        private void ClientPCRespons_Receive(byte[] bytes)
        {

            //送信が返ってきたらCheckConnectingIPListのIPと照合して一致していたらClientPCとみなす
            for (int i = 0; i < CheckConnectingPCInfoList.Count; i++)
            {
                if (CheckConnectingPCInfoList[i].IP_Address == Encoding.UTF8.GetString(bytes))
                {
                    var pcInfo = CheckConnectingPCInfoList[i];
                    ClientPCIPList.Items.Add(pcInfo.IP_Address);
                    //照合が済んだらClientPCInfoをClientPCInfosに移動する
                    CheckConnectingPCInfoList.RemoveAt(i);
                    ClientPCInfos.Add(pcInfo);
                    break;
                }
            }
        }

        //Kinectのデータ更新
        private async Task KinectUpdate()
        {
            preFrame = DateTime.Now;
            while (loop)
            {
                //画像認識を15FPSに制限
                if ((DateTime.Now - preFrame).Milliseconds < 67)
                {
                    this.Update();
                    continue;
                }
                else
                {
                    preFrame = DateTime.Now;
                }

                //データの取得
                using (Capture capture = await Task.Run(() => kinect.GetCapture()).ConfigureAwait(true))
                {
                    #region Depth
                    //Depth画像を取得
                    Image depthImage = capture.Depth;
                    //Depth画像の各ピクセルの値(奥行)のみを取得
                    ushort[] depthArray = depthImage.GetPixels<ushort>().ToArray();
                    //depthBitmapの各画素に値を書き込む準備
                    BitmapData bitmapData = depthBitmap.LockBits(new Rectangle(0, 0, depthBitmap.Width, depthBitmap.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
                    unsafe
                    {
                        //各ピクセルの値へのポインタ
                        byte* pixels = (byte*)bitmapData.Scan0;
                        int index;
                        int depth;
                        //一ピクセルずつ処理
                        for (int i = 0; i < depthArray.Length; i++)
                        {
                            depth = 255 - (int)(255 * (depthArray[i] - _depthDistanceMin) / _depthDistanceMax);
                            if (depth < 0)
                            {
                                depth = 0;
                            }
                            else if (depth > _depthThresholdMaxColor)
                            {
                                depth = 255;
                            }
                            index = i * 4;
                            pixels[index++] = (byte)depth;
                            pixels[index++] = (byte)depth;
                            pixels[index++] = (byte)depth;
                            pixels[index++] = 255;
                        }
                    }
                    //書き込み終了
                    depthBitmap.UnlockBits(bitmapData);
                    depthImage.Dispose();
                    //pictureBoxに画像を貼り付け
                    depthBitmapBox.Image = depthBitmap;

                    #endregion

                    #region IR
                    //IR画像を取得
                    Image irImage = capture.IR;
                    //IR画像の各ピクセルの値(奥行)のみを取得
                    ushort[] irArray = irImage.GetPixels<ushort>().ToArray();
                    //irBitmapの各画素に値を書き込む準備
                    BitmapData irData = irBitmap.LockBits(new Rectangle(0, 0, irBitmap.Width, irBitmap.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

                    unsafe
                    {
                        //各ピクセルの値へのポインタ
                        byte* pixels = (byte*)irData.Scan0;
                        int index;
                        int ir;
                        //一ピクセルずつ処理
                        for (int i = 0; i < irArray.Length; i++)
                        {
                            ir = 255 - (int)(255 * (irArray[i] - _irDistanceMin) / _irDistanceMax);
                            if (ir < 0)
                            {
                                ir = 0;
                            }
                            else if (ir > _irThresholdMaxColor)
                            {
                                ir = 255;
                            }
                            index = i * 4;
                            pixels[index++] = (byte)ir;
                            pixels[index++] = (byte)ir;
                            pixels[index++] = (byte)ir;
                            pixels[index++] = 255;
                        }
                    }
                    //書き込み終了
                    irBitmap.UnlockBits(irData);
                    irImage.Dispose();
                    //pictureBoxに画像を貼り付け
                    irBitmapBox.Image = irBitmap;

                    #endregion

                    #region Mask
                    //深度カメラの処理
                    Mat depthMat = new Mat();
                    depthMat = BitmapConverter.ToMat(depthBitmap);
                    depthMat.Reshape(1);

                    //深度カメラの画像のチャンネル数とタイプを変更
                    Mat tempDepthMatGray = new Mat();
                    Cv2.CvtColor(depthMat, tempDepthMatGray, ColorConversionCodes.RGB2GRAY);
                    depthMat.Dispose();
                    Mat tempDepthMatBit = new Mat();
                    Cv2.Threshold(tempDepthMatGray, tempDepthMatBit, _depthThresholdMin, _depthThresholdMax, ThresholdTypes.Binary);
                    tempDepthMatGray.Dispose();
                    //IRカメラの処理
                    Mat irMat = new Mat();
                    irMat = BitmapConverter.ToMat(irBitmap);
                    irMat.Reshape(1);

                    //IRカメラの画像のチャンネル数とタイプを変更
                    Mat tempIrMatGray = new Mat();
                    Cv2.CvtColor(irMat, tempIrMatGray, ColorConversionCodes.RGB2GRAY);
                    irMat.Dispose();
                    Mat tempIrMatBit = new Mat();
                    Cv2.Threshold(tempIrMatGray, tempIrMatBit, _irThresholdMin, _irThresholdMax, ThresholdTypes.BinaryInv);
                    tempIrMatGray.Dispose();

                    //マスクをかける
                    Mat outDst = new Mat();
                    Cv2.BitwiseAnd(tempDepthMatBit, tempDepthMatBit, outDst, tempIrMatBit);

                    #endregion

                    #region OffsetMask
                    if (RightMask.Text != "" &&
                        LeftMask.Text != "" &&
                        TopMask.Text != "" &&
                        BottomMask.Text != "")
                    {
                        int rightMask = int.Parse(RightMask.Text);
                        int leftMask = int.Parse(LeftMask.Text);
                        int topMask = int.Parse(TopMask.Text);
                        int bottomMask = int.Parse(BottomMask.Text);
                        /* ------------>  x
                         * |
                         * |
                         * |
                         * |
                         * |
                         * y
                         */
                        var bottomValue = outDst.Height - topMask - bottomMask;
                        var rightValue = outDst.Width - leftMask - rightMask;
                        if (outDst.Width >= rightValue &&
                            outDst.Height >= bottomValue)
                        {
                            Mat clipedMat = outDst.Clone(new OpenCvSharp.Rect(leftMask, topMask,
                                rightValue,
                               bottomValue));
                            Cv2.Resize(clipedMat, clipedMat, new OpenCvSharp.Size(), 640 / clipedMat.Cols, 576 / clipedMat.Rows);
                            resultBitmapBox.Image = BitmapConverter.ToBitmap(clipedMat);

                            var TempImageFilePath = Path.Combine(assetsPath, "TempImage", $"{saveFileIndex}.jpeg");

                            //保存
                            clipedMat.SaveImage(TempImageFilePath);

                            if (saveFileIndex <= 100)
                            {
                                saveFileIndex++;
                            }
                            else
                            {
                                saveFileIndex = 0;
                            }

                            /*
                            var buffer = new byte[clipedMat.Rows * clipedMat.Cols * clipedMat.Channels()];
                            Cv2.ImEncode(".jpg", clipedMat, out buffer);
                            Mat mat = Cv2.ImDecode(buffer, ImreadModes.Color);
                            Cv2.ImShow("test", mat);*/


                            #region 画像データを送信
                            foreach (var client in ClientPCInfos)
                            {
                                //物体検出の結果を取得済みで現在フリーな状態の場合
                                if (!client.WaitingForInput)
                                {
                                    //画像データbyte[]に変換する
                                    byte[] imageData = File.ReadAllBytes(TempImageFilePath);
                                    //送信
                                    client.SendImage(imageData);
                                    break;
                                }
                            }
                            #endregion}

                        }
                        #endregion
                        //非同期で画像認識を実行
                        //_ = Task.Run(() => ImageRecognition(TempImageFilePath));



                        tempDepthMatBit.Dispose();
                        tempIrMatBit.Dispose();
                        await Task.Delay(100);
                    }
                    //表示を更新
                    this.Update();
                }

            }
        }

        //Bitmap画像に関する初期設定
        private void InitBitmap()
        {
            //Depth画像の横幅(width)と縦幅(height)を取得
            int width = kinect.GetCalibration().DepthCameraCalibration.ResolutionWidth;
            int height = kinect.GetCalibration().DepthCameraCalibration.ResolutionHeight;

            //PictureBoxに貼り付けるBitmap画像を作成。サイズはkinectのDepth画像と同じ
            depthBitmap = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            irBitmap = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
        }

        //Kinectの初期化
        private void InitKinect()
        {
            kinect = Device.Open(0);
            kinect.StartCameras(new DeviceConfiguration
            {
                ColorFormat = Microsoft.Azure.Kinect.Sensor.ImageFormat.ColorBGRA32,
                ColorResolution = ColorResolution.R720p,
                DepthMode = DepthMode.NFOV_Unbinned,
                SynchronizedImagesOnly = true,
                CameraFPS = FPS.FPS30
            });
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //値を読み込み
            TopMask.Text = Properties.Settings.Default.TopMask.ToString();
            BottomMask.Text = Properties.Settings.Default.BottomMask.ToString();
            LeftMask.Text = Properties.Settings.Default.LeftMask.ToString();
            RightMask.Text = Properties.Settings.Default.RightMask.ToString();
            GamePCIP.Text = Properties.Settings.Default.GetConnectIP;
        }

        //アプリ終了時にKinect終了
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            loop = false;

            //値を保存
            Properties.Settings.Default.TopMask = int.Parse(TopMask.Text);
            Properties.Settings.Default.BottomMask = int.Parse(BottomMask.Text);
            Properties.Settings.Default.LeftMask = int.Parse(LeftMask.Text);
            Properties.Settings.Default.RightMask = int.Parse(RightMask.Text);
            Properties.Settings.Default.GetConnectIP = GamePCIP.Text;

            Properties.Settings.Default.Save();
            kinect?.StopCameras();
        }

        private void GamePCConnectButton_Click(object sender, EventArgs e)
        {
            GamePCIPAdress = GamePCIP.Text;
            ConnectedGamePCIP.Text = GamePCIPAdress.ToString();
            ConnectedGamePCPort.Text = GamePCPort.ToString();
            GamePC_UDPSender = new UDPSender(GamePCIPAdress, GamePCPort);
            isGamePC_UDPSend = true;
        }

        private List<ClientPCInfo> CheckConnectingPCInfoList = new List<ClientPCInfo>();
        /// <summary>
        /// ClietnPCに接続する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClientConnectButton_Click(object sender, EventArgs e)
        {
            //入力欄が空なら何もしないで終了
            if (ClientPCIP.Text == "") { return; }

            Console.WriteLine(ClientPCIP.Text);
            ClientPCInfo clientPCInfo = new ClientPCInfo(ClientPCIP.Text);
            CheckConnectingPCInfoList.Add(clientPCInfo);

            //すべての作業が終わったらIPアドレスの入力欄を空にする
            ClientPCIP.Text = "";
        }
        private void KinectRun_Click(object sender, EventArgs e)
        {
            InitKinect();

            //Kinectの設定情報に基づいてBitmap関連情報を初期化
            InitBitmap();

            //画像認識のクラスを初期化
            //imageRecognition = new ImageRecognition();

            //データ取得
            //TODO:キャンセルと再実行可能なら再実行する処理
            Task t = KinectUpdate();
        }

        #region デバッグ
        //デバッグ用
        [DllImport("kernel32.dll")]
        private static extern bool AllocConsole();

        //デバッグ用
        [DllImport("kernel32.dll")]
        private static extern bool FreeConsole();

        private void DebugSender_Click(object sender, EventArgs e)
        {
            //デバッグ用
            AllocConsole();
            //デバッグ
            Task tsl = TestSendLoop();
        }


        //デバッグ
        double time = 0;
        private async Task TestSendLoop()
        {
            this.Show();
            stopwatch.Start();
            while (loop)
            {
                if (isGamePC_UDPSend)
                {
                    TimeSpan deltaTime = GetDeltaTime();

                    time += deltaTime.TotalSeconds; // 現在の時間を取得（秒単位）
                    Console.WriteLine($"time = {time}");

                    double angle = 2 * Math.PI * time / 60; // 時間を角度に変換
                    Console.WriteLine($"angle = {angle}");

                    double dx = Math.Cos(angle); // x座標
                    double dy = Math.Sin(angle); // y座標

                    dx = (dx + 1) / 2;
                    dy = (dy + 1) / 2;
                    dx *= 500;
                    dy *= 500;

                    Console.WriteLine($"dx = {dx},dy = {dy}");


                    List<ResultStruct> results = new List<ResultStruct>(){
                    new ResultStruct{ Label = "Cross", PosX = (int)Math.Round(dx), PosY = (int)Math.Round(dy), Confidence = 0.8f }
                    };


                    byte[] serializedData = MessagePackSerializer.Serialize(results);
                    GamePC_UDPSender.Send(serializedData);

                }
                //表示を更新
                this.Update();
                await Task.Delay(TimeSpan.FromSeconds(0.25f));
            }
        }

        private static Stopwatch stopwatch = new Stopwatch();
        private static TimeSpan lastFrameTime;
        public static TimeSpan GetDeltaTime()
        {
            TimeSpan currentTime = stopwatch.Elapsed;
            TimeSpan deltaTime = currentTime - lastFrameTime;
            lastFrameTime = currentTime;

            return deltaTime;
        }
        #endregion
    }
}