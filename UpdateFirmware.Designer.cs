using System.Windows.Forms;

namespace AIoTFontMaker
{
    partial class UpdateFirmware
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private Label labelVersion;
        private string currentVersion = "Unknown";
        private TextBox tbFirmwareVersion;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UpdateFirmware));
            this.labelVersion = new System.Windows.Forms.Label();
            this.btnLoadFirm = new System.Windows.Forms.Button();
            this.gbFirmInfo = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tbFirmwareVersion = new System.Windows.Forms.TextBox();
            this.tbSerialNumberList = new System.Windows.Forms.TextBox();
            this.tbDeviceName = new System.Windows.Forms.TextBox();
            this.tbCompany = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCheckDevice = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.tbDeviceIpAddress = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tbDeviceSN = new System.Windows.Forms.TextBox();
            this.btnUpdateFirm = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.gbFirmInfo.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelVersion
            // 
            this.labelVersion.AutoSize = true;
            this.labelVersion.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.labelVersion.Location = new System.Drawing.Point(10, 185);
            this.labelVersion.Name = "labelVersion";
            this.labelVersion.Size = new System.Drawing.Size(164, 19);
            this.labelVersion.TabIndex = 0;
            this.labelVersion.Text = "Version thiết bị: Unknown";
            // 
            // btnLoadFirm
            // 
            this.btnLoadFirm.Location = new System.Drawing.Point(20, 4);
            this.btnLoadFirm.Margin = new System.Windows.Forms.Padding(4);
            this.btnLoadFirm.Name = "btnLoadFirm";
            this.btnLoadFirm.Size = new System.Drawing.Size(390, 68);
            this.btnLoadFirm.TabIndex = 0;
            this.btnLoadFirm.Text = "Tải Firm";
            this.btnLoadFirm.UseVisualStyleBackColor = true;
            this.btnLoadFirm.Click += new System.EventHandler(this.btnLoadFirm_Click);
            // 
            // gbFirmInfo
            // 
            this.gbFirmInfo.Controls.Add(this.label7);
            this.gbFirmInfo.Controls.Add(this.tbFirmwareVersion);
            this.gbFirmInfo.Controls.Add(this.tbSerialNumberList);
            this.gbFirmInfo.Controls.Add(this.tbDeviceName);
            this.gbFirmInfo.Controls.Add(this.tbCompany);
            this.gbFirmInfo.Controls.Add(this.label3);
            this.gbFirmInfo.Controls.Add(this.label2);
            this.gbFirmInfo.Controls.Add(this.label1);
            this.gbFirmInfo.Location = new System.Drawing.Point(20, 95);
            this.gbFirmInfo.Name = "gbFirmInfo";
            this.gbFirmInfo.Size = new System.Drawing.Size(390, 235);
            this.gbFirmInfo.TabIndex = 1;
            this.gbFirmInfo.TabStop = false;
            this.gbFirmInfo.Text = "Thông tin phần mềm";
            this.gbFirmInfo.Enter += new System.EventHandler(this.gbFirmInfo_Enter);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 86);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(119, 19);
            this.label7.TabIndex = 4;
            this.label7.Text = "Version Firmware:";
            // 
            // tbFirmwareVersion
            // 
            this.tbFirmwareVersion.Location = new System.Drawing.Point(156, 85);
            this.tbFirmwareVersion.Name = "tbFirmwareVersion";
            this.tbFirmwareVersion.Size = new System.Drawing.Size(213, 26);
            this.tbFirmwareVersion.TabIndex = 0;
            this.tbFirmwareVersion.ReadOnly = true;
            // 
            // tbSerialNumberList
            // 
            this.tbSerialNumberList.Location = new System.Drawing.Point(10, 158);
            this.tbSerialNumberList.Multiline = true;
            this.tbSerialNumberList.Name = "tbSerialNumberList";
            this.tbSerialNumberList.ReadOnly = true;
            this.tbSerialNumberList.Size = new System.Drawing.Size(316, 64);
            this.tbSerialNumberList.TabIndex = 3;
            this.tbSerialNumberList.TextChanged += new System.EventHandler(this.tbSerialNumberList_TextChanged);
            // 
            // tbDeviceName
            // 
            this.tbDeviceName.Location = new System.Drawing.Point(156, 52);
            this.tbDeviceName.Name = "tbDeviceName";
            this.tbDeviceName.ReadOnly = true;
            this.tbDeviceName.Size = new System.Drawing.Size(213, 26);
            this.tbDeviceName.TabIndex = 3;
            this.tbDeviceName.TextChanged += new System.EventHandler(this.tbDeviceName_TextChanged);
            // 
            // tbCompany
            // 
            this.tbCompany.Location = new System.Drawing.Point(156, 19);
            this.tbCompany.Name = "tbCompany";
            this.tbCompany.ReadOnly = true;
            this.tbCompany.Size = new System.Drawing.Size(213, 26);
            this.tbCompany.TabIndex = 3;
            this.tbCompany.TextChanged += new System.EventHandler(this.tbCompany_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 136);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(200, 19);
            this.label3.TabIndex = 2;
            this.label3.Text = "Danh mục Serial number hỗ trợ:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 19);
            this.label2.TabIndex = 1;
            this.label2.Text = "Thiết bị hỗ trợ :";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 19);
            this.label1.TabIndex = 0;
            this.label1.Text = "Nhà phát hành :";
            // 
            // btnCheckDevice
            // 
            this.btnCheckDevice.Location = new System.Drawing.Point(14, 22);
            this.btnCheckDevice.Margin = new System.Windows.Forms.Padding(4);
            this.btnCheckDevice.Name = "btnCheckDevice";
            this.btnCheckDevice.Size = new System.Drawing.Size(325, 55);
            this.btnCheckDevice.TabIndex = 0;
            this.btnCheckDevice.Text = "Kiểm tra thiết bị";
            this.btnCheckDevice.UseVisualStyleBackColor = true;
            this.btnCheckDevice.Click += new System.EventHandler(this.btnCheckDevice_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 85);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(119, 19);
            this.label4.TabIndex = 0;
            this.label4.Text = "Địa chỉ IP thiết bị :";
            // 
            // tbDeviceIpAddress
            // 
            this.tbDeviceIpAddress.Location = new System.Drawing.Point(152, 86);
            this.tbDeviceIpAddress.Name = "tbDeviceIpAddress";
            this.tbDeviceIpAddress.Size = new System.Drawing.Size(213, 26);
            this.tbDeviceIpAddress.TabIndex = 3;
            this.tbDeviceIpAddress.TextChanged += new System.EventHandler(this.tbDeviceIpAddress_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 135);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(81, 19);
            this.label5.TabIndex = 2;
            this.label5.Text = "SN thiết bị :";
            // 
            // tbDeviceSN
            // 
            this.tbDeviceSN.Location = new System.Drawing.Point(152, 129);
            this.tbDeviceSN.Name = "tbDeviceSN";
            this.tbDeviceSN.ReadOnly = true;
            this.tbDeviceSN.Size = new System.Drawing.Size(213, 26);
            this.tbDeviceSN.TabIndex = 3;
            this.tbDeviceSN.TextChanged += new System.EventHandler(this.tbDeviceSN_TextChanged);
            // 
            // btnUpdateFirm
            // 
            this.btnUpdateFirm.Location = new System.Drawing.Point(430, 4);
            this.btnUpdateFirm.Margin = new System.Windows.Forms.Padding(4);
            this.btnUpdateFirm.Name = "btnUpdateFirm";
            this.btnUpdateFirm.Size = new System.Drawing.Size(371, 68);
            this.btnUpdateFirm.TabIndex = 0;
            this.btnUpdateFirm.Text = "Cập nhật Firmware\r\n";
            this.btnUpdateFirm.UseVisualStyleBackColor = true;
            this.btnUpdateFirm.Click += new System.EventHandler(this.btnUpdateFirm_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.labelVersion);
            this.groupBox1.Controls.Add(this.btnCheckDevice);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.tbDeviceIpAddress);
            this.groupBox1.Controls.Add(this.tbDeviceSN);
            this.groupBox1.Location = new System.Drawing.Point(430, 95);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(371, 235);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Thông tin thiết bị";
            // 
            // UpdateFirmware
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(834, 487);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.gbFirmInfo);
            this.Controls.Add(this.btnUpdateFirm);
            this.Controls.Add(this.btnLoadFirm);
            this.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "UpdateFirmware";
            this.Text = "AIoT UpdateFirmware";
            this.gbFirmInfo.ResumeLayout(false);
            this.gbFirmInfo.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnLoadFirm;
        private System.Windows.Forms.GroupBox gbFirmInfo;
        private System.Windows.Forms.TextBox tbCompany;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbSerialNumberList;
        private System.Windows.Forms.TextBox tbDeviceName;
        private System.Windows.Forms.Button btnCheckDevice;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbDeviceIpAddress;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbDeviceSN;
        private System.Windows.Forms.Button btnUpdateFirm;
        private Label label7;
        private Label label3;
        private GroupBox groupBox1;
    }
}