using System.Windows.Forms;

namespace AIoTFontMaker
{
    partial class frm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private ComboBox comboBoxDisplayMode; // ComboBox chọn chế độ hiển thị
        private StatusStrip statusStrip1; // Thanh trạng thái
        private ToolStripStatusLabel statusLabel; // Nhãn trạng thái
        private TextBox textBoxIP; // Ô nhập IP
        private Label labelIP;     // Nhãn cho ô IP
        private TextBox textBoxNewText; // Ô nhập text mới
        private Label labelNewText;     // Nhãn cho ô nhập text
        private Button buttonSendText;  // Nút gửi text

        private TextBox textBoxNewTextLine2;
        private Label labelNewTextLine2;

        // Ô chọn màu sắc cho chữ (Line 1 và Line 2)
        private Label labelColorLine1;
        private Button buttonColorLine1; // Nút mở ColorDialog cho Line 1
        private Label labelColorLine2;
        private Button buttonColorLine2; // Nút mở ColorDialog cho Line 2

        // Độ sáng màn hình
        private Label labelBrightness;
        private NumericUpDown numericBrightness; // Giá trị từ 0-255

        // Hướng xoay chữ
        private Label labelRotation;
        private ComboBox comboBoxRotation; // Các lựa chọn: 0°, 90°, 180°, 270°

        // Căn chỉnh dòng chữ
        private Label labelAlignment;
        private ComboBox comboBoxAlignment; // Các lựa chọn: Left, Center, Right

        // Chế độ DHCP hoặc IP tĩnh
        private Label labelNetworkMode;
        private ComboBox comboBoxNetworkMode; // Các lựa chọn: DHCP, Static

        // Cổng kết nối
        private Label labelPort;
        private TextBox textBoxPort;
                                   // Các nút mới
        private Button buttonSendColor;      // Gửi màu sắc
        private Button buttonSendBrightness; // Gửi độ sáng
        private Button buttonSendRotation;   // Gửi hướng xoay
        private Button buttonSendAlignment;  // Gửi căn chỉnh


        private Label labelTextX1;
        private NumericUpDown numericUpDownTextX1;
        private Label labelTextY1;
        private NumericUpDown numericUpDownTextY1;
        private Label labelTextX2;
        private NumericUpDown numericUpDownTextX2;
        private Label labelTextY2;
        private NumericUpDown numericUpDownTextY2;


        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnSelectFont = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnCreateFont = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.labelIP = new System.Windows.Forms.Label();
            this.textBoxIP = new System.Windows.Forms.TextBox();
            this.labelNewText = new System.Windows.Forms.Label();
            this.textBoxNewText = new System.Windows.Forms.TextBox();
            this.buttonSendText = new System.Windows.Forms.Button();
            this.labelNewTextLine2 = new System.Windows.Forms.Label();
            this.textBoxNewTextLine2 = new System.Windows.Forms.TextBox();
            this.labelColorLine1 = new System.Windows.Forms.Label();
            this.buttonColorLine1 = new System.Windows.Forms.Button();
            this.labelColorLine2 = new System.Windows.Forms.Label();
            this.buttonColorLine2 = new System.Windows.Forms.Button();
            this.labelBrightness = new System.Windows.Forms.Label();
            this.numericBrightness = new System.Windows.Forms.NumericUpDown();
            this.labelRotation = new System.Windows.Forms.Label();
            this.comboBoxRotation = new System.Windows.Forms.ComboBox();
            this.labelAlignment = new System.Windows.Forms.Label();
            this.comboBoxAlignment = new System.Windows.Forms.ComboBox();
            this.labelNetworkMode = new System.Windows.Forms.Label();
            this.comboBoxNetworkMode = new System.Windows.Forms.ComboBox();
            this.labelPort = new System.Windows.Forms.Label();
            this.buttonSendColor = new System.Windows.Forms.Button();
            this.buttonSendBrightness = new System.Windows.Forms.Button();
            this.buttonSendRotation = new System.Windows.Forms.Button();
            this.buttonSendAlignment = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.textBoxPort = new System.Windows.Forms.TextBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.labelTextX1 = new System.Windows.Forms.Label();
            this.numericUpDownTextX1 = new System.Windows.Forms.NumericUpDown();
            this.labelTextY1 = new System.Windows.Forms.Label();
            this.numericUpDownTextY1 = new System.Windows.Forms.NumericUpDown();
            this.labelTextX2 = new System.Windows.Forms.Label();
            this.numericUpDownTextX2 = new System.Windows.Forms.NumericUpDown();
            this.labelTextY2 = new System.Windows.Forms.Label();
            this.numericUpDownTextY2 = new System.Windows.Forms.NumericUpDown();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.comboBoxDisplayMode = new System.Windows.Forms.ComboBox();
            this.labelDisplayMode = new System.Windows.Forms.Label();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericBrightness)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTextX1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTextY1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTextX2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTextY2)).BeginInit();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 572);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(785, 22);
            this.statusStrip1.TabIndex = 35;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(54, 17);
            this.statusLabel.Text = "Sẵn sàng";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(6, 19);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(691, 60);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // btnSelectFont
            // 
            this.btnSelectFont.BackColor = System.Drawing.Color.PaleTurquoise;
            this.btnSelectFont.Location = new System.Drawing.Point(233, 24);
            this.btnSelectFont.Name = "btnSelectFont";
            this.btnSelectFont.Size = new System.Drawing.Size(86, 23);
            this.btnSelectFont.TabIndex = 1;
            this.btnSelectFont.Text = "Chọn font";
            this.btnSelectFont.UseVisualStyleBackColor = false;
            this.btnSelectFont.Click += new System.EventHandler(this.btnSelectFont_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(74, 55);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(151, 20);
            this.textBox1.TabIndex = 2;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(125, 26);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(100, 20);
            this.textBox2.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label2.Location = new System.Drawing.Point(5, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Kích thước:";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(74, 26);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(45, 20);
            this.textBox3.TabIndex = 9;
            this.textBox3.TextChanged += new System.EventHandler(this.textBox3_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label4.Location = new System.Drawing.Point(6, 58);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(40, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Bộ mã:";
            // 
            // btnCreateFont
            // 
            this.btnCreateFont.BackColor = System.Drawing.Color.PaleTurquoise;
            this.btnCreateFont.Location = new System.Drawing.Point(247, 8);
            this.btnCreateFont.Name = "btnCreateFont";
            this.btnCreateFont.Size = new System.Drawing.Size(72, 44);
            this.btnCreateFont.TabIndex = 15;
            this.btnCreateFont.Text = "Tạo font";
            this.btnCreateFont.UseVisualStyleBackColor = false;
            this.btnCreateFont.Click += new System.EventHandler(this.btnCreateFont_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(233, 55);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(86, 21);
            this.comboBox1.TabIndex = 18;
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.ForeColor = System.Drawing.Color.Black;
            this.numericUpDown1.Location = new System.Drawing.Point(105, 26);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(44, 20);
            this.numericUpDown1.TabIndex = 25;
            this.numericUpDown1.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label5.Location = new System.Drawing.Point(149, 29);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 13);
            this.label5.TabIndex = 21;
            this.label5.Text = "Pixel";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label6.Location = new System.Drawing.Point(6, 29);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(96, 13);
            this.label6.TabIndex = 22;
            this.label6.Text = "Độ rộng dấu cách:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.btnSelectFont);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.textBox2);
            this.groupBox1.Controls.Add(this.textBox3);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.comboBox1);
            this.groupBox1.Location = new System.Drawing.Point(7, 85);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(337, 105);
            this.groupBox1.TabIndex = 30;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Input";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.textBox6);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.numericUpDown1);
            this.groupBox2.Controls.Add(this.btnCreateFont);
            this.groupBox2.Location = new System.Drawing.Point(372, 85);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(325, 105);
            this.groupBox2.TabIndex = 31;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Output";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.PaleTurquoise;
            this.button1.Location = new System.Drawing.Point(247, 58);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(72, 42);
            this.button1.TabIndex = 24;
            this.button1.Text = "Gửi font";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label8.Location = new System.Drawing.Point(66, 73);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(136, 13);
            this.label8.TabIndex = 23;
            this.label8.Text = "( đặt tên bất kỳ không dấu)";
            // 
            // textBox6
            // 
            this.textBox6.ForeColor = System.Drawing.SystemColors.MenuBar;
            this.textBox6.Location = new System.Drawing.Point(59, 50);
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(151, 20);
            this.textBox6.TabIndex = 19;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label3.Location = new System.Drawing.Point(6, 53);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 13);
            this.label3.TabIndex = 20;
            this.label3.Text = "Tên font";
            // 
            // labelIP
            // 
            this.labelIP.AutoSize = true;
            this.labelIP.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.labelIP.Location = new System.Drawing.Point(11, 63);
            this.labelIP.Name = "labelIP";
            this.labelIP.Size = new System.Drawing.Size(23, 13);
            this.labelIP.TabIndex = 25;
            this.labelIP.Text = "IP :";
            // 
            // textBoxIP
            // 
            this.textBoxIP.Enabled = false;
            this.textBoxIP.ForeColor = System.Drawing.Color.Black;
            this.textBoxIP.Location = new System.Drawing.Point(90, 59);
            this.textBoxIP.Name = "textBoxIP";
            this.textBoxIP.Size = new System.Drawing.Size(100, 20);
            this.textBoxIP.TabIndex = 26;
            // 
            // labelNewText
            // 
            this.labelNewText.AutoSize = true;
            this.labelNewText.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.labelNewText.Location = new System.Drawing.Point(18, 32);
            this.labelNewText.Name = "labelNewText";
            this.labelNewText.Size = new System.Drawing.Size(45, 13);
            this.labelNewText.TabIndex = 27;
            this.labelNewText.Text = "Dòng 1:";
            // 
            // textBoxNewText
            // 
            this.textBoxNewText.ForeColor = System.Drawing.Color.Black;
            this.textBoxNewText.Location = new System.Drawing.Point(89, 28);
            this.textBoxNewText.Name = "textBoxNewText";
            this.textBoxNewText.Size = new System.Drawing.Size(227, 20);
            this.textBoxNewText.TabIndex = 28;
            this.textBoxNewText.Text = "Nhập văn bản dòng 1";
            // 
            // buttonSendText
            // 
            this.buttonSendText.BackColor = System.Drawing.Color.PaleTurquoise;
            this.buttonSendText.Location = new System.Drawing.Point(267, 109);
            this.buttonSendText.Name = "buttonSendText";
            this.buttonSendText.Size = new System.Drawing.Size(98, 53);
            this.buttonSendText.TabIndex = 29;
            this.buttonSendText.Text = "Gửi Text";
            this.buttonSendText.UseVisualStyleBackColor = false;
            this.buttonSendText.Click += new System.EventHandler(this.buttonSendText_Click);
            // 
            // labelNewTextLine2
            // 
            this.labelNewTextLine2.AutoSize = true;
            this.labelNewTextLine2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.labelNewTextLine2.Location = new System.Drawing.Point(18, 71);
            this.labelNewTextLine2.Name = "labelNewTextLine2";
            this.labelNewTextLine2.Size = new System.Drawing.Size(45, 13);
            this.labelNewTextLine2.TabIndex = 30;
            this.labelNewTextLine2.Text = "Dòng 2:";
            // 
            // textBoxNewTextLine2
            // 
            this.textBoxNewTextLine2.ForeColor = System.Drawing.Color.Black;
            this.textBoxNewTextLine2.Location = new System.Drawing.Point(89, 68);
            this.textBoxNewTextLine2.Name = "textBoxNewTextLine2";
            this.textBoxNewTextLine2.Size = new System.Drawing.Size(227, 20);
            this.textBoxNewTextLine2.TabIndex = 31;
            this.textBoxNewTextLine2.Text = "Nhập văn bản dòng 2";
            // 
            // labelColorLine1
            // 
            this.labelColorLine1.AutoSize = true;
            this.labelColorLine1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.labelColorLine1.Location = new System.Drawing.Point(18, 188);
            this.labelColorLine1.Name = "labelColorLine1";
            this.labelColorLine1.Size = new System.Drawing.Size(63, 13);
            this.labelColorLine1.TabIndex = 0;
            this.labelColorLine1.Text = "Màu Line 1:";
            // 
            // buttonColorLine1
            // 
            this.buttonColorLine1.ForeColor = System.Drawing.Color.Black;
            this.buttonColorLine1.Location = new System.Drawing.Point(151, 183);
            this.buttonColorLine1.Name = "buttonColorLine1";
            this.buttonColorLine1.Size = new System.Drawing.Size(75, 23);
            this.buttonColorLine1.TabIndex = 1;
            this.buttonColorLine1.Text = "Chọn màu";
            this.buttonColorLine1.Click += new System.EventHandler(this.buttonColorLine1_Click);
            // 
            // labelColorLine2
            // 
            this.labelColorLine2.AutoSize = true;
            this.labelColorLine2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.labelColorLine2.Location = new System.Drawing.Point(18, 227);
            this.labelColorLine2.Name = "labelColorLine2";
            this.labelColorLine2.Size = new System.Drawing.Size(63, 13);
            this.labelColorLine2.TabIndex = 2;
            this.labelColorLine2.Text = "Màu Line 2:";
            // 
            // buttonColorLine2
            // 
            this.buttonColorLine2.ForeColor = System.Drawing.Color.Black;
            this.buttonColorLine2.Location = new System.Drawing.Point(151, 222);
            this.buttonColorLine2.Name = "buttonColorLine2";
            this.buttonColorLine2.Size = new System.Drawing.Size(75, 23);
            this.buttonColorLine2.TabIndex = 3;
            this.buttonColorLine2.Text = "Chọn màu";
            this.buttonColorLine2.Click += new System.EventHandler(this.buttonColorLine2_Click);
            // 
            // labelBrightness
            // 
            this.labelBrightness.AutoSize = true;
            this.labelBrightness.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.labelBrightness.Location = new System.Drawing.Point(18, 266);
            this.labelBrightness.Name = "labelBrightness";
            this.labelBrightness.Size = new System.Drawing.Size(86, 13);
            this.labelBrightness.TabIndex = 4;
            this.labelBrightness.Text = "Độ sáng (0-255):";
            // 
            // numericBrightness
            // 
            this.numericBrightness.ForeColor = System.Drawing.SystemColors.MenuBar;
            this.numericBrightness.Location = new System.Drawing.Point(151, 264);
            this.numericBrightness.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numericBrightness.Name = "numericBrightness";
            this.numericBrightness.Size = new System.Drawing.Size(75, 20);
            this.numericBrightness.TabIndex = 5;
            this.numericBrightness.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // labelRotation
            // 
            this.labelRotation.AutoSize = true;
            this.labelRotation.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.labelRotation.Location = new System.Drawing.Point(9, 82);
            this.labelRotation.Name = "labelRotation";
            this.labelRotation.Size = new System.Drawing.Size(67, 13);
            this.labelRotation.TabIndex = 6;
            this.labelRotation.Text = "Hướng xoay:";
            // 
            // comboBoxRotation
            // 
            this.comboBoxRotation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxRotation.ForeColor = System.Drawing.SystemColors.WindowText;
            this.comboBoxRotation.Items.AddRange(new object[] {
            "0°",
            "90°",
            "180°",
            "270°"});
            this.comboBoxRotation.Location = new System.Drawing.Point(97, 78);
            this.comboBoxRotation.Name = "comboBoxRotation";
            this.comboBoxRotation.Size = new System.Drawing.Size(75, 21);
            this.comboBoxRotation.TabIndex = 7;
            // 
            // labelAlignment
            // 
            this.labelAlignment.AutoSize = true;
            this.labelAlignment.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.labelAlignment.Location = new System.Drawing.Point(9, 124);
            this.labelAlignment.Name = "labelAlignment";
            this.labelAlignment.Size = new System.Drawing.Size(58, 13);
            this.labelAlignment.TabIndex = 8;
            this.labelAlignment.Text = "Căn chỉnh:";
            // 
            // comboBoxAlignment
            // 
            this.comboBoxAlignment.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxAlignment.Items.AddRange(new object[] {
            "Left",
            "Center",
            "Right"});
            this.comboBoxAlignment.Location = new System.Drawing.Point(97, 116);
            this.comboBoxAlignment.Name = "comboBoxAlignment";
            this.comboBoxAlignment.Size = new System.Drawing.Size(75, 21);
            this.comboBoxAlignment.TabIndex = 9;
            // 
            // labelNetworkMode
            // 
            this.labelNetworkMode.AutoSize = true;
            this.labelNetworkMode.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.labelNetworkMode.Location = new System.Drawing.Point(11, 35);
            this.labelNetworkMode.Name = "labelNetworkMode";
            this.labelNetworkMode.Size = new System.Drawing.Size(74, 13);
            this.labelNetworkMode.TabIndex = 10;
            this.labelNetworkMode.Text = "Chế độ mạng:";
            // 
            // comboBoxNetworkMode
            // 
            this.comboBoxNetworkMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxNetworkMode.Items.AddRange(new object[] {
            "DHCP",
            "Static"});
            this.comboBoxNetworkMode.Location = new System.Drawing.Point(90, 32);
            this.comboBoxNetworkMode.Name = "comboBoxNetworkMode";
            this.comboBoxNetworkMode.Size = new System.Drawing.Size(75, 21);
            this.comboBoxNetworkMode.TabIndex = 11;
            this.comboBoxNetworkMode.SelectedIndexChanged += new System.EventHandler(this.comboBoxNetworkMode_SelectedIndexChanged);
            // 
            // labelPort
            // 
            this.labelPort.AutoSize = true;
            this.labelPort.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.labelPort.Location = new System.Drawing.Point(11, 91);
            this.labelPort.Name = "labelPort";
            this.labelPort.Size = new System.Drawing.Size(29, 13);
            this.labelPort.TabIndex = 18;
            this.labelPort.Text = "Port:";
            // 
            // buttonSendColor
            // 
            this.buttonSendColor.BackColor = System.Drawing.Color.PaleTurquoise;
            this.buttonSendColor.Location = new System.Drawing.Point(267, 183);
            this.buttonSendColor.Name = "buttonSendColor";
            this.buttonSendColor.Size = new System.Drawing.Size(98, 60);
            this.buttonSendColor.TabIndex = 26;
            this.buttonSendColor.Text = "Gửi Màu";
            this.buttonSendColor.UseVisualStyleBackColor = false;
            this.buttonSendColor.Click += new System.EventHandler(this.buttonSendColor_Click);
            // 
            // buttonSendBrightness
            // 
            this.buttonSendBrightness.BackColor = System.Drawing.Color.PaleTurquoise;
            this.buttonSendBrightness.Location = new System.Drawing.Point(267, 249);
            this.buttonSendBrightness.Name = "buttonSendBrightness";
            this.buttonSendBrightness.Size = new System.Drawing.Size(98, 35);
            this.buttonSendBrightness.TabIndex = 27;
            this.buttonSendBrightness.Text = "Gửi Độ Sáng";
            this.buttonSendBrightness.UseVisualStyleBackColor = false;
            this.buttonSendBrightness.Click += new System.EventHandler(this.buttonSendBrightness_Click);
            // 
            // buttonSendRotation
            // 
            this.buttonSendRotation.BackColor = System.Drawing.Color.PaleTurquoise;
            this.buttonSendRotation.Location = new System.Drawing.Point(195, 78);
            this.buttonSendRotation.Name = "buttonSendRotation";
            this.buttonSendRotation.Size = new System.Drawing.Size(131, 23);
            this.buttonSendRotation.TabIndex = 28;
            this.buttonSendRotation.Text = "Gửi Hướng Xoay";
            this.buttonSendRotation.UseVisualStyleBackColor = false;
            this.buttonSendRotation.Click += new System.EventHandler(this.buttonSendRotation_Click);
            // 
            // buttonSendAlignment
            // 
            this.buttonSendAlignment.BackColor = System.Drawing.Color.PaleTurquoise;
            this.buttonSendAlignment.Location = new System.Drawing.Point(195, 114);
            this.buttonSendAlignment.Name = "buttonSendAlignment";
            this.buttonSendAlignment.Size = new System.Drawing.Size(131, 23);
            this.buttonSendAlignment.TabIndex = 29;
            this.buttonSendAlignment.Text = "Gửi Căn Chỉnh";
            this.buttonSendAlignment.UseVisualStyleBackColor = false;
            this.buttonSendAlignment.Click += new System.EventHandler(this.buttonSendAlignment_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.Color.Gainsboro;
            this.groupBox3.Controls.Add(this.textBoxPort);
            this.groupBox3.Controls.Add(this.labelIP);
            this.groupBox3.Controls.Add(this.textBoxIP);
            this.groupBox3.Controls.Add(this.labelNetworkMode);
            this.groupBox3.Controls.Add(this.labelPort);
            this.groupBox3.Controls.Add(this.comboBoxNetworkMode);
            this.groupBox3.Location = new System.Drawing.Point(11, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(345, 130);
            this.groupBox3.TabIndex = 32;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Cấu hình mạng";
            // 
            // textBoxPort
            // 
            this.textBoxPort.ForeColor = System.Drawing.Color.Black;
            this.textBoxPort.Location = new System.Drawing.Point(90, 85);
            this.textBoxPort.Name = "textBoxPort";
            this.textBoxPort.Size = new System.Drawing.Size(50, 20);
            this.textBoxPort.TabIndex = 19;
            this.textBoxPort.Text = "80";
            // 
            // groupBox4
            // 
            this.groupBox4.BackColor = System.Drawing.Color.Gainsboro;
            this.groupBox4.Controls.Add(this.textBoxNewTextLine2);
            this.groupBox4.Controls.Add(this.buttonSendText);
            this.groupBox4.Controls.Add(this.labelColorLine1);
            this.groupBox4.Controls.Add(this.textBoxNewText);
            this.groupBox4.Controls.Add(this.buttonColorLine1);
            this.groupBox4.Controls.Add(this.labelNewText);
            this.groupBox4.Controls.Add(this.labelColorLine2);
            this.groupBox4.Controls.Add(this.labelNewTextLine2);
            this.groupBox4.Controls.Add(this.buttonColorLine2);
            this.groupBox4.Controls.Add(this.labelBrightness);
            this.groupBox4.Controls.Add(this.numericBrightness);
            this.groupBox4.Controls.Add(this.buttonSendBrightness);
            this.groupBox4.Controls.Add(this.buttonSendColor);
            this.groupBox4.Controls.Add(this.labelTextX1);
            this.groupBox4.Controls.Add(this.numericUpDownTextX1);
            this.groupBox4.Controls.Add(this.labelTextY1);
            this.groupBox4.Controls.Add(this.numericUpDownTextY1);
            this.groupBox4.Controls.Add(this.labelTextX2);
            this.groupBox4.Controls.Add(this.numericUpDownTextX2);
            this.groupBox4.Controls.Add(this.labelTextY2);
            this.groupBox4.Controls.Add(this.numericUpDownTextY2);
            this.groupBox4.Location = new System.Drawing.Point(365, 12);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(384, 305);
            this.groupBox4.TabIndex = 33;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Điều khiển led";
            // 
            // labelTextX1
            // 
            this.labelTextX1.AutoSize = true;
            this.labelTextX1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.labelTextX1.Location = new System.Drawing.Point(16, 110);
            this.labelTextX1.Name = "labelTextX1";
            this.labelTextX1.Size = new System.Drawing.Size(62, 13);
            this.labelTextX1.TabIndex = 32;
            this.labelTextX1.Text = "X1 (ngang):";
            // 
            // numericUpDownTextX1
            // 
            this.numericUpDownTextX1.ForeColor = System.Drawing.Color.Black;
            this.numericUpDownTextX1.Location = new System.Drawing.Point(88, 110);
            this.numericUpDownTextX1.Maximum = new decimal(new int[] {
            128,
            0,
            0,
            0});
            this.numericUpDownTextX1.Minimum = new decimal(new int[] {
            128,
            0,
            0,
            -2147483648});
            this.numericUpDownTextX1.Name = "numericUpDownTextX1";
            this.numericUpDownTextX1.Size = new System.Drawing.Size(50, 20);
            this.numericUpDownTextX1.TabIndex = 33;
            this.numericUpDownTextX1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // labelTextY1
            // 
            this.labelTextY1.AutoSize = true;
            this.labelTextY1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.labelTextY1.Location = new System.Drawing.Point(149, 110);
            this.labelTextY1.Name = "labelTextY1";
            this.labelTextY1.Size = new System.Drawing.Size(50, 13);
            this.labelTextY1.TabIndex = 34;
            this.labelTextY1.Text = "Y1 (dọc):";
            // 
            // numericUpDownTextY1
            // 
            this.numericUpDownTextY1.ForeColor = System.Drawing.Color.Black;
            this.numericUpDownTextY1.Location = new System.Drawing.Point(204, 110);
            this.numericUpDownTextY1.Maximum = new decimal(new int[] {
            32,
            0,
            0,
            0});
            this.numericUpDownTextY1.Name = "numericUpDownTextY1";
            this.numericUpDownTextY1.Size = new System.Drawing.Size(50, 20);
            this.numericUpDownTextY1.TabIndex = 35;
            this.numericUpDownTextY1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // labelTextX2
            // 
            this.labelTextX2.AutoSize = true;
            this.labelTextX2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.labelTextX2.Location = new System.Drawing.Point(16, 148);
            this.labelTextX2.Name = "labelTextX2";
            this.labelTextX2.Size = new System.Drawing.Size(62, 13);
            this.labelTextX2.TabIndex = 36;
            this.labelTextX2.Text = "X2 (ngang):";
            // 
            // numericUpDownTextX2
            // 
            this.numericUpDownTextX2.ForeColor = System.Drawing.Color.Black;
            this.numericUpDownTextX2.Location = new System.Drawing.Point(87, 142);
            this.numericUpDownTextX2.Maximum = new decimal(new int[] {
            128,
            0,
            0,
            0});
            this.numericUpDownTextX2.Minimum = new decimal(new int[] {
            128,
            0,
            0,
            -2147483648});
            this.numericUpDownTextX2.Name = "numericUpDownTextX2";
            this.numericUpDownTextX2.Size = new System.Drawing.Size(50, 20);
            this.numericUpDownTextX2.TabIndex = 37;
            this.numericUpDownTextX2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // labelTextY2
            // 
            this.labelTextY2.AutoSize = true;
            this.labelTextY2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.labelTextY2.Location = new System.Drawing.Point(149, 148);
            this.labelTextY2.Name = "labelTextY2";
            this.labelTextY2.Size = new System.Drawing.Size(50, 13);
            this.labelTextY2.TabIndex = 38;
            this.labelTextY2.Text = "Y2 (dọc):";
            // 
            // numericUpDownTextY2
            // 
            this.numericUpDownTextY2.ForeColor = System.Drawing.Color.Black;
            this.numericUpDownTextY2.Location = new System.Drawing.Point(205, 142);
            this.numericUpDownTextY2.Maximum = new decimal(new int[] {
            32,
            0,
            0,
            0});
            this.numericUpDownTextY2.Name = "numericUpDownTextY2";
            this.numericUpDownTextY2.Size = new System.Drawing.Size(50, 20);
            this.numericUpDownTextY2.TabIndex = 39;
            this.numericUpDownTextY2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numericUpDownTextY2.Value = new decimal(new int[] {
            17,
            0,
            0,
            0});
            // 
            // groupBox5
            // 
            this.groupBox5.BackColor = System.Drawing.Color.Gainsboro;
            this.groupBox5.Controls.Add(this.pictureBox1);
            this.groupBox5.Controls.Add(this.groupBox1);
            this.groupBox5.Controls.Add(this.groupBox2);
            this.groupBox5.Location = new System.Drawing.Point(13, 335);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(736, 207);
            this.groupBox5.TabIndex = 34;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Điều khiển font chữ";
            // 
            // groupBox6
            // 
            this.groupBox6.BackColor = System.Drawing.Color.Gainsboro;
            this.groupBox6.Controls.Add(this.comboBoxDisplayMode);
            this.groupBox6.Controls.Add(this.labelDisplayMode);
            this.groupBox6.Controls.Add(this.comboBoxRotation);
            this.groupBox6.Controls.Add(this.labelAlignment);
            this.groupBox6.Controls.Add(this.comboBoxAlignment);
            this.groupBox6.Controls.Add(this.labelRotation);
            this.groupBox6.Controls.Add(this.buttonSendRotation);
            this.groupBox6.Controls.Add(this.buttonSendAlignment);
            this.groupBox6.Location = new System.Drawing.Point(13, 160);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(343, 157);
            this.groupBox6.TabIndex = 36;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Chế độ hiển thị";
            // 
            // comboBoxDisplayMode
            // 
            this.comboBoxDisplayMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxDisplayMode.Items.AddRange(new object[] {
            "Hiển thị 1 dòng",
            "Hiển thị 2 dòng"});
            this.comboBoxDisplayMode.Location = new System.Drawing.Point(97, 40);
            this.comboBoxDisplayMode.Name = "comboBoxDisplayMode";
            this.comboBoxDisplayMode.Size = new System.Drawing.Size(120, 21);
            this.comboBoxDisplayMode.TabIndex = 1;
            this.comboBoxDisplayMode.SelectedIndexChanged += new System.EventHandler(this.comboBoxDisplayMode_SelectedIndexChanged);
            // 
            // labelDisplayMode
            // 
            this.labelDisplayMode.AutoSize = true;
            this.labelDisplayMode.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.labelDisplayMode.Location = new System.Drawing.Point(9, 40);
            this.labelDisplayMode.Name = "labelDisplayMode";
            this.labelDisplayMode.Size = new System.Drawing.Size(82, 13);
            this.labelDisplayMode.TabIndex = 0;
            this.labelDisplayMode.Text = "Chế độ hiển thị:";
            // 
            // frm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(785, 594);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frm";
            this.Text = "AIoT Updatefont";
            this.MinimumSizeChanged += new System.EventHandler(this.Form1_MinimumSizeChanged);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericBrightness)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTextX1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTextY1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTextX2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTextY2)).EndInit();
            this.groupBox5.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private PictureBox pictureBox1;
        private Button btnSelectFont;
        private TextBox textBox1;
        private TextBox textBox2;
        private Label label2;
        private TextBox textBox3;
        private Label label4;
        private Button btnCreateFont;
        private ComboBox comboBox1;
        private NumericUpDown numericUpDown1;
        private Label label5;
        private Label label6;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private Button button1;
        private Label label8;
        private TextBox textBox6;
        private Label label3;
        #endregion

        private GroupBox groupBox3;
        private GroupBox groupBox4;
        private GroupBox groupBox5;
        private GroupBox groupBox6;
    }
}