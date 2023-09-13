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

        #region �O���̃X�N���v�g�Ŏ擾�p
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
            ClientPCIPList.Items.Add (s);
        }
        #endregion
        #region �摜�����֌W
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
        //Kinect�������ϐ�
        Device kinect;
        //Depth�摜��Bitmap
        Bitmap depthBitmap;
        //IR�摜��Bitmap
        Bitmap irBitmap;
        bool loop = true;

        uint saveFileIndex = 0;

        //�t���[�����Œ肷��̂Ɏg�p����ϐ�
        DateTime preFrame;


        #region �ʐM�֌W�̕ϐ�
        //�Q�[���{�̂̒ʐM����
        private bool isGamePC_UDPSend = false;
        private UDPSender GamePC_UDPSender;
        private string GamePCIPAdress = "localhost";
        private static int GamePCPort = 12001; //�Q�[���{�̂ƒʐM����|�[�g�ԍ�


        //ClientPC�̒ʐM����

        /// <summary>
        /// �ʐM�̊m��
        /// </summary>
        private static int CommunicationSendPort = 12010;

        /// <summary>
        /// �摜�̑��M
        /// </summary>
        private static int ImageSendPort = 12011;

        /// <summary>
        /// ���ʂ̎擾
        /// </summary>
        private static int ResultReceivePort = 12012;

        public List<ClientPCInfo> ClientPCInfos = new List<ClientPCInfo>();

        #endregion


        public Form1()
        {
            Form1.Instance = this;

            InitializeComponent();

            //�f�o�b�O�p
            AllocConsole();
            /*            //IPv4�̃A�h���X���擾���ĕ\��
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

        //Kinect�̃f�[�^�X�V
        private async Task KinectUpdate()
        {
            preFrame = DateTime.Now;
            while (loop)
            {
                //�摜�F����15FPS�ɐ���
                if ((DateTime.Now - preFrame).Milliseconds < 67)
                {
                    this.Update();
                    continue;
                }
                else
                {
                    preFrame = DateTime.Now;
                }

                //�f�[�^�̎擾
                using (Capture capture = await Task.Run(() => kinect.GetCapture()).ConfigureAwait(true))
                {
                    #region Depth
                    //Depth�摜���擾
                    Image depthImage = capture.Depth;
                    //Depth�摜�̊e�s�N�Z���̒l(���s)�݂̂��擾
                    ushort[] depthArray = depthImage.GetPixels<ushort>().ToArray();
                    //depthBitmap�̊e��f�ɒl���������ޏ���
                    BitmapData bitmapData = depthBitmap.LockBits(new Rectangle(0, 0, depthBitmap.Width, depthBitmap.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
                    unsafe
                    {
                        //�e�s�N�Z���̒l�ւ̃|�C���^
                        byte* pixels = (byte*)bitmapData.Scan0;
                        int index;
                        int depth;
                        //��s�N�Z��������
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
                    //�������ݏI��
                    depthBitmap.UnlockBits(bitmapData);
                    depthImage.Dispose();
                    //pictureBox�ɉ摜��\��t��
                    depthBitmapBox.Image = depthBitmap;

                    #endregion

                    #region IR
                    //IR�摜���擾
                    Image irImage = capture.IR;
                    //IR�摜�̊e�s�N�Z���̒l(���s)�݂̂��擾
                    ushort[] irArray = irImage.GetPixels<ushort>().ToArray();
                    //irBitmap�̊e��f�ɒl���������ޏ���
                    BitmapData irData = irBitmap.LockBits(new Rectangle(0, 0, irBitmap.Width, irBitmap.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

                    unsafe
                    {
                        //�e�s�N�Z���̒l�ւ̃|�C���^
                        byte* pixels = (byte*)irData.Scan0;
                        int index;
                        int ir;
                        //��s�N�Z��������
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
                    //�������ݏI��
                    irBitmap.UnlockBits(irData);
                    irImage.Dispose();
                    //pictureBox�ɉ摜��\��t��
                    irBitmapBox.Image = irBitmap;

                    #endregion

                    #region Mask
                    //�[�x�J�����̏���
                    Mat depthMat = new Mat();
                    depthMat = BitmapConverter.ToMat(depthBitmap);
                    depthMat.Reshape(1);

                    //�[�x�J�����̉摜�̃`�����l�����ƃ^�C�v��ύX
                    Mat tempDepthMatGray = new Mat();
                    Cv2.CvtColor(depthMat, tempDepthMatGray, ColorConversionCodes.RGB2GRAY);
                    depthMat.Dispose();
                    Mat tempDepthMatBit = new Mat();
                    Cv2.Threshold(tempDepthMatGray, tempDepthMatBit, _depthThresholdMin, _depthThresholdMax, ThresholdTypes.Binary);
                    tempDepthMatGray.Dispose();
                    //IR�J�����̏���
                    Mat irMat = new Mat();
                    irMat = BitmapConverter.ToMat(irBitmap);
                    irMat.Reshape(1);

                    //IR�J�����̉摜�̃`�����l�����ƃ^�C�v��ύX
                    Mat tempIrMatGray = new Mat();
                    Cv2.CvtColor(irMat, tempIrMatGray, ColorConversionCodes.RGB2GRAY);
                    irMat.Dispose();
                    Mat tempIrMatBit = new Mat();
                    Cv2.Threshold(tempIrMatGray, tempIrMatBit, _irThresholdMin, _irThresholdMax, ThresholdTypes.BinaryInv);
                    tempIrMatGray.Dispose();

                    //�}�X�N��������
                    Mat outDst = new Mat();
                    Cv2.BitwiseAnd(tempDepthMatBit, tempDepthMatBit, outDst, tempIrMatBit);

                    #endregion
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

                            //�ۑ�
                            clipedMat.SaveImage(TempImageFilePath);
                        }
                    }

                    //�񓯊��ŉ摜�F�������s
                    //_ = Task.Run(() => ImageRecognition(TempImageFilePath));

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
                }
                //�\�����X�V
                this.Update();
            }

        }

        //Bitmap�摜�Ɋւ��鏉���ݒ�
        private void InitBitmap()
        {
            //Depth�摜�̉���(width)�Əc��(height)���擾
            int width = kinect.GetCalibration().DepthCameraCalibration.ResolutionWidth;
            int height = kinect.GetCalibration().DepthCameraCalibration.ResolutionHeight;

            //PictureBox�ɓ\��t����Bitmap�摜���쐬�B�T�C�Y��kinect��Depth�摜�Ɠ���
            depthBitmap = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            irBitmap = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
        }

        //Kinect�̏�����
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
            //�l��ǂݍ���
            TopMask.Text = Properties.Settings.Default.TopMask.ToString();
            BottomMask.Text = Properties.Settings.Default.BottomMask.ToString();
            LeftMask.Text = Properties.Settings.Default.LeftMask.ToString();
            RightMask.Text = Properties.Settings.Default.RightMask.ToString();
            GamePCIP.Text = Properties.Settings.Default.GetConnectIP;
        }

        //�A�v���I������Kinect�I��
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            loop = false;

            //�l��ۑ�
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
        private void ClientConnectButton_Click(object sender, EventArgs e)
        {
            ClientPCInfo clientPCInfo = new ClientPCInfo(ClientPCIP.Text);

            TCPSender tcpSender = new TCPSender(ClientPCIP.Text, CommunicationSendPort, clientPCInfo.CommunicationReceive);
            if (tcpSender.isConnection)
            {
                tcpSender.Send(Encoding.UTF8.GetBytes("connecting"));
            }

            ClientPCIP.Text = "";
            /*            _ipAdressText = ClientPCIP.Text;
                        ConnectViewIpAdress.Text = _ipAdressText.ToString();
                        ConnectViewPort.Text = GamePCPort.ToString();
                        GamePC_UDPSender = new UDPSender(_ipAdressText, GamePCPort);
                        isGamePC_UDPSend = true;*/

        }
        //UPD�̐ڑ����J�n����
        private void UDPConectStart_Click(object sender, EventArgs e)
        {
            /*
             _ipAdressText = ClientPCIP.Text;
             ConnectViewIpAdress.Text = _ipAdressText.ToString();
             ConnectViewPort.Text = _port.ToString();
             UDPSender = new UDPSender(_ipAdressText, _port);
             _isUDPSend = true;*/
        }

        private void KinectRun_Click(object sender, EventArgs e)
        {
            InitKinect();

            //Kinect�̐ݒ���Ɋ�Â���Bitmap�֘A����������
            InitBitmap();

            //�摜�F���̃N���X��������
            //imageRecognition = new ImageRecognition();

            //�f�[�^�擾
            //TODO:�L�����Z���ƍĎ��s�\�Ȃ�Ď��s���鏈��
            Task t = KinectUpdate();
        }

        #region �f�o�b�O
        //�f�o�b�O�p
        [DllImport("kernel32.dll")]
        private static extern bool AllocConsole();

        //�f�o�b�O�p
        [DllImport("kernel32.dll")]
        private static extern bool FreeConsole();

        private void DebugSender_Click(object sender, EventArgs e)
        {
            //�f�o�b�O�p
            AllocConsole();
            //�f�o�b�O
            Task tsl = TestSendLoop();
        }


        //�f�o�b�O
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

                    time += deltaTime.TotalSeconds; // ���݂̎��Ԃ��擾�i�b�P�ʁj
                    Console.WriteLine($"time = {time}");

                    double angle = 2 * Math.PI * time / 60; // ���Ԃ��p�x�ɕϊ�
                    Console.WriteLine($"angle = {angle}");

                    double dx = Math.Cos(angle); // x���W
                    double dy = Math.Sin(angle); // y���W

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
                //�\�����X�V
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