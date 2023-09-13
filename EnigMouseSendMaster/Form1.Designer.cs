namespace EnigMouseSendMaster
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            tableLayoutPanel1 = new TableLayoutPanel();
            groupBox3 = new GroupBox();
            ConnectedGamePCPort = new Label();
            ConnectedGamePCIP = new Label();
            label4 = new Label();
            label3 = new Label();
            groupBox2 = new GroupBox();
            GamePCConnectButton = new Button();
            label2 = new Label();
            GamePCIP = new TextBox();
            SendClientPCAdd = new GroupBox();
            ClientConnectButton = new Button();
            label1 = new Label();
            ClientPCIP = new TextBox();
            groupBox1 = new GroupBox();
            ClientPCIPList = new ListBox();
            splitContainer2 = new SplitContainer();
            groupBox4 = new GroupBox();
            BottomMask = new NumericUpDown();
            RightMask = new NumericUpDown();
            LeftMask = new NumericUpDown();
            TopMask = new NumericUpDown();
            groupBox5 = new GroupBox();
            DebugSenderButton = new Button();
            KinectRunButton = new Button();
            splitContainer1 = new SplitContainer();
            tableLayoutPanel2 = new TableLayoutPanel();
            g_Result = new GroupBox();
            resultBitmapBox = new PictureBox();
            g_IR = new GroupBox();
            irBitmapBox = new PictureBox();
            g_Depth = new GroupBox();
            depthBitmapBox = new PictureBox();
            tableLayoutPanel1.SuspendLayout();
            groupBox3.SuspendLayout();
            groupBox2.SuspendLayout();
            SendClientPCAdd.SuspendLayout();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
            splitContainer2.Panel2.SuspendLayout();
            splitContainer2.SuspendLayout();
            groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)BottomMask).BeginInit();
            ((System.ComponentModel.ISupportInitialize)RightMask).BeginInit();
            ((System.ComponentModel.ISupportInitialize)LeftMask).BeginInit();
            ((System.ComponentModel.ISupportInitialize)TopMask).BeginInit();
            groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            g_Result.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)resultBitmapBox).BeginInit();
            g_IR.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)irBitmapBox).BeginInit();
            g_Depth.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)depthBitmapBox).BeginInit();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(groupBox3, 1, 1);
            tableLayoutPanel1.Controls.Add(groupBox2, 0, 1);
            tableLayoutPanel1.Controls.Add(SendClientPCAdd, 0, 0);
            tableLayoutPanel1.Controls.Add(groupBox1, 1, 0);
            tableLayoutPanel1.Controls.Add(splitContainer2, 0, 2);
            tableLayoutPanel1.Controls.Add(groupBox5, 1, 2);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel1.Size = new Size(856, 327);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(ConnectedGamePCPort);
            groupBox3.Controls.Add(ConnectedGamePCIP);
            groupBox3.Controls.Add(label4);
            groupBox3.Controls.Add(label3);
            groupBox3.Dock = DockStyle.Fill;
            groupBox3.Location = new Point(431, 112);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(422, 103);
            groupBox3.TabIndex = 5;
            groupBox3.TabStop = false;
            groupBox3.Text = "接続済みのゲームPC";
            // 
            // ConnectedGamePCPort
            // 
            ConnectedGamePCPort.AutoSize = true;
            ConnectedGamePCPort.Location = new Point(82, 51);
            ConnectedGamePCPort.Name = "ConnectedGamePCPort";
            ConnectedGamePCPort.Size = new Size(0, 15);
            ConnectedGamePCPort.TabIndex = 3;
            // 
            // ConnectedGamePCIP
            // 
            ConnectedGamePCIP.AutoSize = true;
            ConnectedGamePCIP.Location = new Point(82, 25);
            ConnectedGamePCIP.Name = "ConnectedGamePCIP";
            ConnectedGamePCIP.Size = new Size(0, 15);
            ConnectedGamePCIP.TabIndex = 2;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(6, 51);
            label4.Name = "label4";
            label4.Size = new Size(70, 15);
            label4.TabIndex = 1;
            label4.Text = "ゲームPCPort";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(6, 25);
            label3.Name = "label3";
            label3.Size = new Size(58, 15);
            label3.TabIndex = 0;
            label3.Text = "ゲームPCIP";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(GamePCConnectButton);
            groupBox2.Controls.Add(label2);
            groupBox2.Controls.Add(GamePCIP);
            groupBox2.Dock = DockStyle.Fill;
            groupBox2.Location = new Point(3, 112);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(422, 103);
            groupBox2.TabIndex = 4;
            groupBox2.TabStop = false;
            groupBox2.Text = "ゲームPCを追加";
            // 
            // GamePCConnectButton
            // 
            GamePCConnectButton.Location = new Point(14, 72);
            GamePCConnectButton.Name = "GamePCConnectButton";
            GamePCConnectButton.Size = new Size(111, 23);
            GamePCConnectButton.TabIndex = 2;
            GamePCConnectButton.Text = "ゲームPCを追加";
            GamePCConnectButton.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(14, 25);
            label2.Name = "label2";
            label2.Size = new Size(58, 15);
            label2.TabIndex = 1;
            label2.Text = "ゲームPCIP";
            // 
            // GamePCIP
            // 
            GamePCIP.Location = new Point(14, 43);
            GamePCIP.Name = "GamePCIP";
            GamePCIP.Size = new Size(231, 23);
            GamePCIP.TabIndex = 0;
            // 
            // SendClientPCAdd
            // 
            SendClientPCAdd.Controls.Add(ClientConnectButton);
            SendClientPCAdd.Controls.Add(label1);
            SendClientPCAdd.Controls.Add(ClientPCIP);
            SendClientPCAdd.Dock = DockStyle.Fill;
            SendClientPCAdd.Location = new Point(3, 3);
            SendClientPCAdd.Name = "SendClientPCAdd";
            SendClientPCAdd.Size = new Size(422, 103);
            SendClientPCAdd.TabIndex = 1;
            SendClientPCAdd.TabStop = false;
            SendClientPCAdd.Text = "ClientPCを追加";
            // 
            // ClientConnectButton
            // 
            ClientConnectButton.Location = new Point(14, 72);
            ClientConnectButton.Name = "ClientConnectButton";
            ClientConnectButton.Size = new Size(111, 23);
            ClientConnectButton.TabIndex = 2;
            ClientConnectButton.Text = "ClientPCを追加";
            ClientConnectButton.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(14, 25);
            label1.Name = "label1";
            label1.Size = new Size(47, 15);
            label1.TabIndex = 1;
            label1.Text = "ClientIP";
            // 
            // ClientPCIP
            // 
            ClientPCIP.Location = new Point(14, 43);
            ClientPCIP.Name = "ClientPCIP";
            ClientPCIP.Size = new Size(231, 23);
            ClientPCIP.TabIndex = 0;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(ClientPCIPList);
            groupBox1.Dock = DockStyle.Fill;
            groupBox1.Location = new Point(431, 3);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(422, 103);
            groupBox1.TabIndex = 2;
            groupBox1.TabStop = false;
            groupBox1.Text = "接続済みのClientPC";
            // 
            // ClientPCIPList
            // 
            ClientPCIPList.Dock = DockStyle.Fill;
            ClientPCIPList.FormattingEnabled = true;
            ClientPCIPList.ItemHeight = 15;
            ClientPCIPList.Location = new Point(3, 19);
            ClientPCIPList.Name = "ClientPCIPList";
            ClientPCIPList.Size = new Size(416, 81);
            ClientPCIPList.TabIndex = 0;
            // 
            // splitContainer2
            // 
            splitContainer2.Dock = DockStyle.Fill;
            splitContainer2.Location = new Point(3, 221);
            splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel2
            // 
            splitContainer2.Panel2.Controls.Add(groupBox4);
            splitContainer2.Size = new Size(422, 103);
            splitContainer2.SplitterDistance = 199;
            splitContainer2.TabIndex = 6;
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(BottomMask);
            groupBox4.Controls.Add(RightMask);
            groupBox4.Controls.Add(LeftMask);
            groupBox4.Controls.Add(TopMask);
            groupBox4.Dock = DockStyle.Fill;
            groupBox4.Location = new Point(0, 0);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(219, 103);
            groupBox4.TabIndex = 0;
            groupBox4.TabStop = false;
            groupBox4.Text = "Mask";
            // 
            // BottomMask
            // 
            BottomMask.Anchor = AnchorStyles.Bottom;
            BottomMask.Location = new Point(84, 74);
            BottomMask.Name = "BottomMask";
            BottomMask.Size = new Size(55, 23);
            BottomMask.TabIndex = 3;
            // 
            // RightMask
            // 
            RightMask.Anchor = AnchorStyles.Right;
            RightMask.Location = new Point(161, 47);
            RightMask.Name = "RightMask";
            RightMask.Size = new Size(55, 23);
            RightMask.TabIndex = 2;
            // 
            // LeftMask
            // 
            LeftMask.Anchor = AnchorStyles.Left;
            LeftMask.Location = new Point(6, 47);
            LeftMask.Name = "LeftMask";
            LeftMask.Size = new Size(55, 23);
            LeftMask.TabIndex = 1;
            // 
            // TopMask
            // 
            TopMask.Anchor = AnchorStyles.Top;
            TopMask.Location = new Point(84, 22);
            TopMask.Name = "TopMask";
            TopMask.Size = new Size(55, 23);
            TopMask.TabIndex = 0;
            // 
            // groupBox5
            // 
            groupBox5.Controls.Add(DebugSenderButton);
            groupBox5.Controls.Add(KinectRunButton);
            groupBox5.Dock = DockStyle.Fill;
            groupBox5.Location = new Point(431, 221);
            groupBox5.Name = "groupBox5";
            groupBox5.Size = new Size(422, 103);
            groupBox5.TabIndex = 7;
            groupBox5.TabStop = false;
            groupBox5.Text = "SendType";
            // 
            // DebugSenderButton
            // 
            DebugSenderButton.Location = new Point(151, 47);
            DebugSenderButton.Name = "DebugSenderButton";
            DebugSenderButton.Size = new Size(87, 23);
            DebugSenderButton.TabIndex = 1;
            DebugSenderButton.Text = "DebugSender";
            DebugSenderButton.UseVisualStyleBackColor = true;
            DebugSenderButton.Click += DebugSender_Click;
            // 
            // KinectRunButton
            // 
            KinectRunButton.Location = new Point(25, 45);
            KinectRunButton.Name = "KinectRunButton";
            KinectRunButton.Size = new Size(75, 23);
            KinectRunButton.TabIndex = 0;
            KinectRunButton.Text = "KinectRun";
            KinectRunButton.UseVisualStyleBackColor = true;
            KinectRunButton.Click += KinectRun_Click;
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(tableLayoutPanel1);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(tableLayoutPanel2);
            splitContainer1.Size = new Size(856, 655);
            splitContainer1.SplitterDistance = 327;
            splitContainer1.TabIndex = 1;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 3;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel2.Controls.Add(g_Result, 0, 0);
            tableLayoutPanel2.Controls.Add(g_IR, 0, 0);
            tableLayoutPanel2.Controls.Add(g_Depth, 0, 0);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(0, 0);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel2.Size = new Size(856, 324);
            tableLayoutPanel2.TabIndex = 0;
            // 
            // g_Result
            // 
            g_Result.Controls.Add(resultBitmapBox);
            g_Result.Location = new Point(573, 3);
            g_Result.Name = "g_Result";
            g_Result.Size = new Size(280, 318);
            g_Result.TabIndex = 34;
            g_Result.TabStop = false;
            g_Result.Text = "Result";
            // 
            // resultBitmapBox
            // 
            resultBitmapBox.Dock = DockStyle.Fill;
            resultBitmapBox.Location = new Point(3, 19);
            resultBitmapBox.Margin = new Padding(4);
            resultBitmapBox.Name = "resultBitmapBox";
            resultBitmapBox.Size = new Size(274, 296);
            resultBitmapBox.SizeMode = PictureBoxSizeMode.Zoom;
            resultBitmapBox.TabIndex = 18;
            resultBitmapBox.TabStop = false;
            // 
            // g_IR
            // 
            g_IR.Controls.Add(irBitmapBox);
            g_IR.Location = new Point(288, 3);
            g_IR.Name = "g_IR";
            g_IR.Size = new Size(279, 318);
            g_IR.TabIndex = 33;
            g_IR.TabStop = false;
            g_IR.Text = "IR";
            // 
            // irBitmapBox
            // 
            irBitmapBox.Dock = DockStyle.Fill;
            irBitmapBox.Location = new Point(3, 19);
            irBitmapBox.Margin = new Padding(4);
            irBitmapBox.Name = "irBitmapBox";
            irBitmapBox.Size = new Size(273, 296);
            irBitmapBox.SizeMode = PictureBoxSizeMode.Zoom;
            irBitmapBox.TabIndex = 1;
            irBitmapBox.TabStop = false;
            // 
            // g_Depth
            // 
            g_Depth.Controls.Add(depthBitmapBox);
            g_Depth.Location = new Point(3, 3);
            g_Depth.Name = "g_Depth";
            g_Depth.Size = new Size(279, 318);
            g_Depth.TabIndex = 32;
            g_Depth.TabStop = false;
            g_Depth.Text = "Depth";
            // 
            // depthBitmapBox
            // 
            depthBitmapBox.Dock = DockStyle.Fill;
            depthBitmapBox.Location = new Point(3, 19);
            depthBitmapBox.Margin = new Padding(4);
            depthBitmapBox.Name = "depthBitmapBox";
            depthBitmapBox.Size = new Size(273, 296);
            depthBitmapBox.SizeMode = PictureBoxSizeMode.Zoom;
            depthBitmapBox.TabIndex = 0;
            depthBitmapBox.TabStop = false;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(856, 655);
            Controls.Add(splitContainer1);
            Name = "Form1";
            Text = "Form1";
            FormClosing += Form1_FormClosing; 
            tableLayoutPanel1.ResumeLayout(false);
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            SendClientPCAdd.ResumeLayout(false);
            SendClientPCAdd.PerformLayout();
            groupBox1.ResumeLayout(false);
            splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
            splitContainer2.ResumeLayout(false);
            groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)BottomMask).EndInit();
            ((System.ComponentModel.ISupportInitialize)RightMask).EndInit();
            ((System.ComponentModel.ISupportInitialize)LeftMask).EndInit();
            ((System.ComponentModel.ISupportInitialize)TopMask).EndInit();
            groupBox5.ResumeLayout(false);
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            g_Result.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)resultBitmapBox).EndInit();
            g_IR.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)irBitmapBox).EndInit();
            g_Depth.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)depthBitmapBox).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private GroupBox SendClientPCAdd;
        private Button ClientConnectButton;
        private Label label1;
        private TextBox ClientPCIP;
        private GroupBox groupBox1;
        private ListBox ClientPCIPList;
        private GroupBox groupBox3;
        private GroupBox groupBox2;
        private Button GamePCConnectButton;
        private Label label2;
        private TextBox GamePCIP;
        private Label ConnectedGamePCPort;
        private Label ConnectedGamePCIP;
        private Label label4;
        private Label label3;
        private SplitContainer splitContainer1;
        private SplitContainer splitContainer2;
        private GroupBox groupBox4;
        private NumericUpDown BottomMask;
        private NumericUpDown RightMask;
        private NumericUpDown LeftMask;
        private NumericUpDown TopMask;
        private GroupBox groupBox5;
        private Button DebugSenderButton;
        private Button KinectRunButton;
        private TableLayoutPanel tableLayoutPanel2;
        private GroupBox g_Depth;
        private PictureBox depthBitmapBox;
        private GroupBox g_Result;
        private PictureBox resultBitmapBox;
        private GroupBox g_IR;
        private PictureBox irBitmapBox;
    }
}