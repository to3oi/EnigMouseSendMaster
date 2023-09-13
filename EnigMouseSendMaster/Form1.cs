//using MessagePack;
//using Microsoft.Azure.Kinect.Sensor;
//using OpenCvSharp;
//using OpenCvSharp.Extensions;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Net;
using System.Runtime.InteropServices;
//using UnityEasyNet;
//using static KinectImageConvertSender.FilePath;
using BitmapData = System.Drawing.Imaging.BitmapData;
//using Image = Microsoft.Azure.Kinect.Sensor.Image;

namespace EnigMouseSendMaster
{
    public partial class Form1 : Form
    {
        //画像処理関係
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


        bool loop = true;
        private bool _isUDPSend = false;

        public Form1()
        {
            InitializeComponent();

            //デバッグ用
            //AllocConsole();
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
        }

        //Kinectのデータ更新
        /*private async Task KinectUpdate()
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
                    irBitmap.UnlockBits(bitmapData);
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

                    #region Maskを描画する

                    //背景用のbitmap
                    Bitmap bg_bitmap = BitmapConverter.ToBitmap(outDst);

                    //描画先とするImageオブジェクトを作成する
                    Bitmap canvas = new Bitmap(bg_bitmap.Width, bg_bitmap.Height);
                    Graphics graphics = Graphics.FromImage(canvas);

                    graphics.DrawImage(bg_bitmap, 0, 0, bg_bitmap.Width, bg_bitmap.Height);

                    //半透明のBrashを作成する
                    SolidBrush semiTransBrush = new SolidBrush(Color.FromArgb(128, 0, 0, 255));

                    //Left
                    if (LeftMask.Text != "")
                    {
                        int leftMask = int.Parse(LeftMask.Text);
                        graphics.FillRectangle(semiTransBrush, 0, 0, leftMask, bg_bitmap.Height);
                    }

                    //Right
                    if (RightMask.Text != "")
                    {
                        int rightMask = int.Parse(RightMask.Text);
                        graphics.FillRectangle(semiTransBrush, bg_bitmap.Width - rightMask, 0, rightMask, bg_bitmap.Height);
                    }
                    //Top
                    if (TopMask.Text != "")
                    {
                        int topMask = int.Parse(TopMask.Text);
                        graphics.FillRectangle(semiTransBrush, 0, 0, bg_bitmap.Width, topMask);
                    }
                    //Bottom
                    if (BottomMask.Text != "")
                    {
                        int bottomMask = int.Parse(BottomMask.Text);
                        graphics.FillRectangle(semiTransBrush, 0, bg_bitmap.Height - bottomMask, bg_bitmap.Width, bottomMask);
                    }

                    bg_bitmap.Dispose();
                    semiTransBrush.Dispose();
                    graphics.Dispose();

                    //表示
                    resultBitmapBox.Image = canvas;
                    #endregion

                    *//*                    //デバッグ
                                        Cv2.ImShow("result", outDst);*//*

                    //画像として保存するパスを作成
                    var TempImageFilePath = Path.Combine(assetsPath, "TempImage", $"{saveFileIndex}.jpeg");

                    //保存
                    outDst.SaveImage(TempImageFilePath);

                    //非同期で画像認識を実行
                    _ = Task.Run(() => ImageRecognition(TempImageFilePath));

                    if (saveFileIndex <= 100)
                    {
                        saveFileIndex++;
                    }
                    else
                    {
                        saveFileIndex = 0;
                    }

                    tempDepthMatBit.Dispose();
                    tempIrMatBit.Dispose();
                    capture.Dispose();
                }
                //表示を更新
                this.Update();
            }
            //ループが終了したらKinectも停止
            kinect.StopCameras();
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
                if (_isUDPSend)
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
                    UDPSender.Send(serializedData);

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
        #endregion*/
    }
}