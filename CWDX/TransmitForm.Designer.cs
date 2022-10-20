
namespace CWDX {
    partial class TransmitForm {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if(disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.tTXText = new System.Windows.Forms.TextBox();
            this.lblTextToTX = new System.Windows.Forms.Label();
            this.btnSendTX = new System.Windows.Forms.Button();
            this.btnCancelTX = new System.Windows.Forms.Button();
            this.tbTXWPM = new System.Windows.Forms.TrackBar();
            this.lblTXSpeed = new System.Windows.Forms.Label();
            this.lblTXWPM = new System.Windows.Forms.Label();
            this.tbGain = new System.Windows.Forms.TrackBar();
            this.lblGain = new System.Windows.Forms.Label();
            this.tbFreq = new System.Windows.Forms.TrackBar();
            this.lblFreq = new System.Windows.Forms.Label();
            this.cbWaveType = new System.Windows.Forms.ComboBox();
            this.lblWaveType = new System.Windows.Forms.Label();
            this.gbTransmit = new System.Windows.Forms.GroupBox();
            this.tcTX = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.pbTXTime = new System.Windows.Forms.ProgressBar();
            this.tTXLive = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.btnRevertTX = new System.Windows.Forms.Button();
            this.btnSaveTX = new System.Windows.Forms.Button();
            this.chbKeepText = new System.Windows.Forms.CheckBox();
            this.Macros = new System.Windows.Forms.TabPage();
            this.gbStation = new System.Windows.Forms.GroupBox();
            this.tcQSO = new System.Windows.Forms.TabControl();
            this.tpMyStation = new System.Windows.Forms.TabPage();
            this.tCallSignMe = new System.Windows.Forms.TextBox();
            this.tGridMe = new System.Windows.Forms.TextBox();
            this.lblCallSignMe = new System.Windows.Forms.Label();
            this.lblGridMe = new System.Windows.Forms.Label();
            this.lblQTHMe = new System.Windows.Forms.Label();
            this.tQTHMe = new System.Windows.Forms.TextBox();
            this.lblNameMe = new System.Windows.Forms.Label();
            this.tNameMe = new System.Windows.Forms.TextBox();
            this.btnEditMe = new System.Windows.Forms.Button();
            this.btnSaveMe = new System.Windows.Forms.Button();
            this.tpRemoteStation = new System.Windows.Forms.TabPage();
            this.lblCallSign = new System.Windows.Forms.Label();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.tCallSign = new System.Windows.Forms.TextBox();
            this.lblGrid = new System.Windows.Forms.Label();
            this.lblQTH = new System.Windows.Forms.Label();
            this.tName = new System.Windows.Forms.TextBox();
            this.tQTH = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.tbTXWPM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbGain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbFreq)).BeginInit();
            this.gbTransmit.SuspendLayout();
            this.tcTX.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.gbStation.SuspendLayout();
            this.tcQSO.SuspendLayout();
            this.tpMyStation.SuspendLayout();
            this.tpRemoteStation.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tTXText
            // 
            this.tTXText.AcceptsReturn = true;
            this.tTXText.AcceptsTab = true;
            this.tTXText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tTXText.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.tTXText.Location = new System.Drawing.Point(3, 22);
            this.tTXText.Multiline = true;
            this.tTXText.Name = "tTXText";
            this.tTXText.Size = new System.Drawing.Size(546, 105);
            this.tTXText.TabIndex = 0;
            // 
            // lblTextToTX
            // 
            this.lblTextToTX.AutoSize = true;
            this.lblTextToTX.Location = new System.Drawing.Point(3, 4);
            this.lblTextToTX.Name = "lblTextToTX";
            this.lblTextToTX.Size = new System.Drawing.Size(93, 15);
            this.lblTextToTX.TabIndex = 1;
            this.lblTextToTX.Text = "Text to Transmit:";
            // 
            // btnSendTX
            // 
            this.btnSendTX.Location = new System.Drawing.Point(3, 133);
            this.btnSendTX.Name = "btnSendTX";
            this.btnSendTX.Size = new System.Drawing.Size(75, 23);
            this.btnSendTX.TabIndex = 2;
            this.btnSendTX.Text = "Transmit";
            this.btnSendTX.UseVisualStyleBackColor = true;
            this.btnSendTX.Click += new System.EventHandler(this.btnSendTX_Click);
            // 
            // btnCancelTX
            // 
            this.btnCancelTX.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancelTX.Enabled = false;
            this.btnCancelTX.Location = new System.Drawing.Point(474, 133);
            this.btnCancelTX.Name = "btnCancelTX";
            this.btnCancelTX.Size = new System.Drawing.Size(75, 23);
            this.btnCancelTX.TabIndex = 3;
            this.btnCancelTX.Text = "Cancel TX";
            this.btnCancelTX.UseVisualStyleBackColor = true;
            this.btnCancelTX.Click += new System.EventHandler(this.btnCancelTX_Click);
            // 
            // tbTXWPM
            // 
            this.tbTXWPM.AutoSize = false;
            this.tbTXWPM.Location = new System.Drawing.Point(108, 6);
            this.tbTXWPM.Maximum = 60;
            this.tbTXWPM.Minimum = 5;
            this.tbTXWPM.Name = "tbTXWPM";
            this.tbTXWPM.Size = new System.Drawing.Size(180, 45);
            this.tbTXWPM.TabIndex = 4;
            this.tbTXWPM.TickFrequency = 5;
            this.tbTXWPM.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.tbTXWPM.Value = 10;
            this.tbTXWPM.Scroll += new System.EventHandler(this.tbTXWPM_Scroll);
            // 
            // lblTXSpeed
            // 
            this.lblTXSpeed.AutoSize = true;
            this.lblTXSpeed.Location = new System.Drawing.Point(4, 6);
            this.lblTXSpeed.Name = "lblTXSpeed";
            this.lblTXSpeed.Size = new System.Drawing.Size(90, 15);
            this.lblTXSpeed.TabIndex = 5;
            this.lblTXSpeed.Text = "Transmit Speed:";
            // 
            // lblTXWPM
            // 
            this.lblTXWPM.Location = new System.Drawing.Point(41, 25);
            this.lblTXWPM.Name = "lblTXWPM";
            this.lblTXWPM.Size = new System.Drawing.Size(61, 15);
            this.lblTXWPM.TabIndex = 6;
            this.lblTXWPM.Text = "10 WPM";
            this.lblTXWPM.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbGain
            // 
            this.tbGain.AutoSize = false;
            this.tbGain.LargeChange = 10;
            this.tbGain.Location = new System.Drawing.Point(108, 57);
            this.tbGain.Maximum = 100;
            this.tbGain.Name = "tbGain";
            this.tbGain.Size = new System.Drawing.Size(180, 24);
            this.tbGain.SmallChange = 5;
            this.tbGain.TabIndex = 7;
            this.tbGain.TickFrequency = 5;
            this.tbGain.TickStyle = System.Windows.Forms.TickStyle.None;
            this.tbGain.Value = 20;
            this.tbGain.Scroll += new System.EventHandler(this.tbGain_Scroll);
            // 
            // lblGain
            // 
            this.lblGain.AutoSize = true;
            this.lblGain.Location = new System.Drawing.Point(4, 57);
            this.lblGain.Name = "lblGain";
            this.lblGain.Size = new System.Drawing.Size(34, 15);
            this.lblGain.TabIndex = 8;
            this.lblGain.Text = "Gain:";
            // 
            // tbFreq
            // 
            this.tbFreq.AutoSize = false;
            this.tbFreq.LargeChange = 100;
            this.tbFreq.Location = new System.Drawing.Point(108, 87);
            this.tbFreq.Maximum = 1000;
            this.tbFreq.Minimum = 500;
            this.tbFreq.Name = "tbFreq";
            this.tbFreq.Size = new System.Drawing.Size(180, 24);
            this.tbFreq.SmallChange = 25;
            this.tbFreq.TabIndex = 10;
            this.tbFreq.TickFrequency = 25;
            this.tbFreq.TickStyle = System.Windows.Forms.TickStyle.None;
            this.tbFreq.Value = 700;
            this.tbFreq.Scroll += new System.EventHandler(this.tbFreq_Scroll);
            // 
            // lblFreq
            // 
            this.lblFreq.AutoSize = true;
            this.lblFreq.Location = new System.Drawing.Point(4, 87);
            this.lblFreq.Name = "lblFreq";
            this.lblFreq.Size = new System.Drawing.Size(65, 15);
            this.lblFreq.TabIndex = 11;
            this.lblFreq.Text = "Frequency:";
            // 
            // cbWaveType
            // 
            this.cbWaveType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWaveType.FormattingEnabled = true;
            this.cbWaveType.Location = new System.Drawing.Point(108, 117);
            this.cbWaveType.Name = "cbWaveType";
            this.cbWaveType.Size = new System.Drawing.Size(180, 23);
            this.cbWaveType.TabIndex = 12;
            this.cbWaveType.SelectedIndexChanged += new System.EventHandler(this.cbWaveType_SelectedIndexChanged);
            // 
            // lblWaveType
            // 
            this.lblWaveType.AutoSize = true;
            this.lblWaveType.Location = new System.Drawing.Point(4, 120);
            this.lblWaveType.Name = "lblWaveType";
            this.lblWaveType.Size = new System.Drawing.Size(66, 15);
            this.lblWaveType.TabIndex = 13;
            this.lblWaveType.Text = "Wave Type:";
            // 
            // gbTransmit
            // 
            this.gbTransmit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbTransmit.Controls.Add(this.tcTX);
            this.gbTransmit.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.gbTransmit.Location = new System.Drawing.Point(3, 24);
            this.gbTransmit.MinimumSize = new System.Drawing.Size(350, 250);
            this.gbTransmit.Name = "gbTransmit";
            this.gbTransmit.Size = new System.Drawing.Size(920, 338);
            this.gbTransmit.TabIndex = 14;
            this.gbTransmit.TabStop = false;
            this.gbTransmit.Text = "TRANSMITTING";
            // 
            // tcTX
            // 
            this.tcTX.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tcTX.Controls.Add(this.tabPage1);
            this.tcTX.Controls.Add(this.tabPage2);
            this.tcTX.Controls.Add(this.Macros);
            this.tcTX.HotTrack = true;
            this.tcTX.Location = new System.Drawing.Point(6, 22);
            this.tcTX.Name = "tcTX";
            this.tcTX.SelectedIndex = 0;
            this.tcTX.Size = new System.Drawing.Size(908, 310);
            this.tcTX.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.AutoScroll = true;
            this.tabPage1.Controls.Add(this.pbTXTime);
            this.tabPage1.Controls.Add(this.tTXText);
            this.tabPage1.Controls.Add(this.tTXLive);
            this.tabPage1.Controls.Add(this.btnCancelTX);
            this.tabPage1.Controls.Add(this.btnSendTX);
            this.tabPage1.Controls.Add(this.lblTextToTX);
            this.tabPage1.Location = new System.Drawing.Point(4, 24);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.tabPage1.Size = new System.Drawing.Size(900, 282);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Transmit";
            // 
            // pbTXTime
            // 
            this.pbTXTime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbTXTime.Location = new System.Drawing.Point(84, 133);
            this.pbTXTime.MarqueeAnimationSpeed = 1;
            this.pbTXTime.Name = "pbTXTime";
            this.pbTXTime.Size = new System.Drawing.Size(384, 23);
            this.pbTXTime.Step = 1;
            this.pbTXTime.TabIndex = 17;
            // 
            // tTXLive
            // 
            this.tTXLive.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tTXLive.Location = new System.Drawing.Point(3, 162);
            this.tTXLive.Multiline = true;
            this.tTXLive.Name = "tTXLive";
            this.tTXLive.ReadOnly = true;
            this.tTXLive.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tTXLive.Size = new System.Drawing.Size(546, 112);
            this.tTXLive.TabIndex = 15;
            this.tTXLive.TextChanged += new System.EventHandler(this.tTXLive_TextChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.btnRevertTX);
            this.tabPage2.Controls.Add(this.btnSaveTX);
            this.tabPage2.Controls.Add(this.cbWaveType);
            this.tabPage2.Controls.Add(this.chbKeepText);
            this.tabPage2.Controls.Add(this.tbTXWPM);
            this.tabPage2.Controls.Add(this.lblTXSpeed);
            this.tabPage2.Controls.Add(this.lblTXWPM);
            this.tabPage2.Controls.Add(this.tbGain);
            this.tabPage2.Controls.Add(this.lblGain);
            this.tabPage2.Controls.Add(this.lblWaveType);
            this.tabPage2.Controls.Add(this.tbFreq);
            this.tabPage2.Controls.Add(this.lblFreq);
            this.tabPage2.Location = new System.Drawing.Point(4, 24);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.tabPage2.Size = new System.Drawing.Size(901, 282);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "TX Settings";
            // 
            // btnRevertTX
            // 
            this.btnRevertTX.Location = new System.Drawing.Point(4, 171);
            this.btnRevertTX.Name = "btnRevertTX";
            this.btnRevertTX.Size = new System.Drawing.Size(98, 23);
            this.btnRevertTX.TabIndex = 18;
            this.btnRevertTX.Text = "Revert to Saved";
            this.btnRevertTX.UseVisualStyleBackColor = true;
            this.btnRevertTX.Click += new System.EventHandler(this.btnRevertTX_Click);
            // 
            // btnSaveTX
            // 
            this.btnSaveTX.Location = new System.Drawing.Point(198, 171);
            this.btnSaveTX.Name = "btnSaveTX";
            this.btnSaveTX.Size = new System.Drawing.Size(90, 23);
            this.btnSaveTX.TabIndex = 17;
            this.btnSaveTX.Text = "Save Settings";
            this.btnSaveTX.UseVisualStyleBackColor = true;
            this.btnSaveTX.Click += new System.EventHandler(this.btnSaveTX_Click);
            // 
            // chbKeepText
            // 
            this.chbKeepText.AutoSize = true;
            this.chbKeepText.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chbKeepText.Location = new System.Drawing.Point(65, 146);
            this.chbKeepText.Name = "chbKeepText";
            this.chbKeepText.Size = new System.Drawing.Size(223, 19);
            this.chbKeepText.TabIndex = 16;
            this.chbKeepText.Text = "Preserve typed text after transmission";
            this.chbKeepText.UseVisualStyleBackColor = true;
            this.chbKeepText.CheckedChanged += new System.EventHandler(this.chbKeepText_CheckedChanged);
            // 
            // Macros
            // 
            this.Macros.BackColor = System.Drawing.SystemColors.Control;
            this.Macros.Location = new System.Drawing.Point(4, 24);
            this.Macros.Name = "Macros";
            this.Macros.Size = new System.Drawing.Size(901, 282);
            this.Macros.TabIndex = 2;
            this.Macros.Text = "Macros";
            // 
            // gbStation
            // 
            this.gbStation.Controls.Add(this.tcQSO);
            this.gbStation.Location = new System.Drawing.Point(929, 24);
            this.gbStation.Name = "gbStation";
            this.gbStation.Size = new System.Drawing.Size(202, 207);
            this.gbStation.TabIndex = 15;
            this.gbStation.TabStop = false;
            this.gbStation.Text = "QSO";
            // 
            // tcQSO
            // 
            this.tcQSO.Controls.Add(this.tpMyStation);
            this.tcQSO.Controls.Add(this.tpRemoteStation);
            this.tcQSO.Location = new System.Drawing.Point(6, 22);
            this.tcQSO.Name = "tcQSO";
            this.tcQSO.SelectedIndex = 0;
            this.tcQSO.Size = new System.Drawing.Size(189, 179);
            this.tcQSO.TabIndex = 18;
            // 
            // tpMyStation
            // 
            this.tpMyStation.BackColor = System.Drawing.SystemColors.Control;
            this.tpMyStation.Controls.Add(this.tCallSignMe);
            this.tpMyStation.Controls.Add(this.tGridMe);
            this.tpMyStation.Controls.Add(this.lblCallSignMe);
            this.tpMyStation.Controls.Add(this.lblGridMe);
            this.tpMyStation.Controls.Add(this.lblQTHMe);
            this.tpMyStation.Controls.Add(this.tQTHMe);
            this.tpMyStation.Controls.Add(this.lblNameMe);
            this.tpMyStation.Controls.Add(this.tNameMe);
            this.tpMyStation.Controls.Add(this.btnEditMe);
            this.tpMyStation.Controls.Add(this.btnSaveMe);
            this.tpMyStation.Location = new System.Drawing.Point(4, 24);
            this.tpMyStation.Name = "tpMyStation";
            this.tpMyStation.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.tpMyStation.Size = new System.Drawing.Size(181, 151);
            this.tpMyStation.TabIndex = 0;
            this.tpMyStation.Text = "My Station";
            // 
            // tCallSignMe
            // 
            this.tCallSignMe.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.tCallSignMe.Enabled = false;
            this.tCallSignMe.Location = new System.Drawing.Point(66, 3);
            this.tCallSignMe.MaxLength = 10;
            this.tCallSignMe.Name = "tCallSignMe";
            this.tCallSignMe.Size = new System.Drawing.Size(100, 23);
            this.tCallSignMe.TabIndex = 1;
            // 
            // tGridMe
            // 
            this.tGridMe.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.tGridMe.Enabled = false;
            this.tGridMe.Location = new System.Drawing.Point(66, 91);
            this.tGridMe.MaxLength = 10;
            this.tGridMe.Name = "tGridMe";
            this.tGridMe.Size = new System.Drawing.Size(100, 23);
            this.tGridMe.TabIndex = 17;
            // 
            // lblCallSignMe
            // 
            this.lblCallSignMe.AutoSize = true;
            this.lblCallSignMe.Location = new System.Drawing.Point(4, 6);
            this.lblCallSignMe.Name = "lblCallSignMe";
            this.lblCallSignMe.Size = new System.Drawing.Size(56, 15);
            this.lblCallSignMe.TabIndex = 0;
            this.lblCallSignMe.Text = "Call Sign:";
            // 
            // lblGridMe
            // 
            this.lblGridMe.AutoSize = true;
            this.lblGridMe.Location = new System.Drawing.Point(28, 94);
            this.lblGridMe.Name = "lblGridMe";
            this.lblGridMe.Size = new System.Drawing.Size(32, 15);
            this.lblGridMe.TabIndex = 16;
            this.lblGridMe.Text = "Grid:";
            // 
            // lblQTHMe
            // 
            this.lblQTHMe.AutoSize = true;
            this.lblQTHMe.Location = new System.Drawing.Point(27, 65);
            this.lblQTHMe.Name = "lblQTHMe";
            this.lblQTHMe.Size = new System.Drawing.Size(33, 15);
            this.lblQTHMe.TabIndex = 2;
            this.lblQTHMe.Text = "QTH:";
            // 
            // tQTHMe
            // 
            this.tQTHMe.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.tQTHMe.Enabled = false;
            this.tQTHMe.Location = new System.Drawing.Point(66, 62);
            this.tQTHMe.MaxLength = 32;
            this.tQTHMe.Name = "tQTHMe";
            this.tQTHMe.Size = new System.Drawing.Size(100, 23);
            this.tQTHMe.TabIndex = 3;
            // 
            // lblNameMe
            // 
            this.lblNameMe.AutoSize = true;
            this.lblNameMe.Location = new System.Drawing.Point(18, 36);
            this.lblNameMe.Name = "lblNameMe";
            this.lblNameMe.Size = new System.Drawing.Size(42, 15);
            this.lblNameMe.TabIndex = 4;
            this.lblNameMe.Text = "Name:";
            // 
            // tNameMe
            // 
            this.tNameMe.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.tNameMe.Enabled = false;
            this.tNameMe.Location = new System.Drawing.Point(66, 33);
            this.tNameMe.MaxLength = 32;
            this.tNameMe.Name = "tNameMe";
            this.tNameMe.Size = new System.Drawing.Size(100, 23);
            this.tNameMe.TabIndex = 5;
            // 
            // btnEditMe
            // 
            this.btnEditMe.Location = new System.Drawing.Point(11, 120);
            this.btnEditMe.Name = "btnEditMe";
            this.btnEditMe.Size = new System.Drawing.Size(75, 23);
            this.btnEditMe.TabIndex = 6;
            this.btnEditMe.Text = "Edit";
            this.btnEditMe.UseVisualStyleBackColor = true;
            this.btnEditMe.Click += new System.EventHandler(this.btnEditMe_Click);
            // 
            // btnSaveMe
            // 
            this.btnSaveMe.Enabled = false;
            this.btnSaveMe.Location = new System.Drawing.Point(92, 120);
            this.btnSaveMe.Name = "btnSaveMe";
            this.btnSaveMe.Size = new System.Drawing.Size(75, 23);
            this.btnSaveMe.TabIndex = 7;
            this.btnSaveMe.Text = "Save";
            this.btnSaveMe.UseVisualStyleBackColor = true;
            this.btnSaveMe.Click += new System.EventHandler(this.btnSaveMe_Click);
            // 
            // tpRemoteStation
            // 
            this.tpRemoteStation.BackColor = System.Drawing.SystemColors.Control;
            this.tpRemoteStation.Controls.Add(this.lblCallSign);
            this.tpRemoteStation.Controls.Add(this.textBox4);
            this.tpRemoteStation.Controls.Add(this.tCallSign);
            this.tpRemoteStation.Controls.Add(this.lblGrid);
            this.tpRemoteStation.Controls.Add(this.lblQTH);
            this.tpRemoteStation.Controls.Add(this.tName);
            this.tpRemoteStation.Controls.Add(this.tQTH);
            this.tpRemoteStation.Controls.Add(this.lblName);
            this.tpRemoteStation.Location = new System.Drawing.Point(4, 24);
            this.tpRemoteStation.Name = "tpRemoteStation";
            this.tpRemoteStation.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.tpRemoteStation.Size = new System.Drawing.Size(181, 151);
            this.tpRemoteStation.TabIndex = 1;
            this.tpRemoteStation.Text = "Remote Station";
            // 
            // lblCallSign
            // 
            this.lblCallSign.AutoSize = true;
            this.lblCallSign.Location = new System.Drawing.Point(3, 6);
            this.lblCallSign.Name = "lblCallSign";
            this.lblCallSign.Size = new System.Drawing.Size(56, 15);
            this.lblCallSign.TabIndex = 8;
            this.lblCallSign.Text = "Call Sign:";
            // 
            // textBox4
            // 
            this.textBox4.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textBox4.Location = new System.Drawing.Point(65, 91);
            this.textBox4.MaxLength = 32;
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(100, 23);
            this.textBox4.TabIndex = 15;
            // 
            // tCallSign
            // 
            this.tCallSign.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.tCallSign.Location = new System.Drawing.Point(65, 3);
            this.tCallSign.MaxLength = 10;
            this.tCallSign.Name = "tCallSign";
            this.tCallSign.Size = new System.Drawing.Size(100, 23);
            this.tCallSign.TabIndex = 9;
            // 
            // lblGrid
            // 
            this.lblGrid.AutoSize = true;
            this.lblGrid.Location = new System.Drawing.Point(27, 94);
            this.lblGrid.Name = "lblGrid";
            this.lblGrid.Size = new System.Drawing.Size(32, 15);
            this.lblGrid.TabIndex = 14;
            this.lblGrid.Text = "Grid:";
            // 
            // lblQTH
            // 
            this.lblQTH.AutoSize = true;
            this.lblQTH.Location = new System.Drawing.Point(26, 65);
            this.lblQTH.Name = "lblQTH";
            this.lblQTH.Size = new System.Drawing.Size(33, 15);
            this.lblQTH.TabIndex = 10;
            this.lblQTH.Text = "QTH:";
            // 
            // tName
            // 
            this.tName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.tName.Location = new System.Drawing.Point(65, 33);
            this.tName.MaxLength = 32;
            this.tName.Name = "tName";
            this.tName.Size = new System.Drawing.Size(100, 23);
            this.tName.TabIndex = 13;
            // 
            // tQTH
            // 
            this.tQTH.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.tQTH.Location = new System.Drawing.Point(65, 62);
            this.tQTH.MaxLength = 32;
            this.tQTH.Name = "tQTH";
            this.tQTH.Size = new System.Drawing.Size(100, 23);
            this.tQTH.TabIndex = 11;
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(17, 36);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(42, 15);
            this.lblName.TabIndex = 12;
            this.lblName.Text = "Name:";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 81.69014F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 18.30986F));
            this.tableLayoutPanel1.Controls.Add(this.gbStation, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.gbTransmit, 0, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 12);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.063291F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 94.93671F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1134, 434);
            this.tableLayoutPanel1.TabIndex = 16;
            // 
            // TransmitForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1165, 465);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "TransmitForm";
            this.Text = "Transmit";
            this.Load += new System.EventHandler(this.TransmitForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.tbTXWPM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbGain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbFreq)).EndInit();
            this.gbTransmit.ResumeLayout(false);
            this.tcTX.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.gbStation.ResumeLayout(false);
            this.tcQSO.ResumeLayout(false);
            this.tpMyStation.ResumeLayout(false);
            this.tpMyStation.PerformLayout();
            this.tpRemoteStation.ResumeLayout(false);
            this.tpRemoteStation.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tTXText;
        private System.Windows.Forms.Label lblTextToTX;
        private System.Windows.Forms.Button btnSendTX;
        private System.Windows.Forms.Button btnCancelTX;
        private System.Windows.Forms.TrackBar tbTXWPM;
        private System.Windows.Forms.Label lblTXSpeed;
        private System.Windows.Forms.Label lblTXWPM;
        private System.Windows.Forms.TrackBar tbGain;
        private System.Windows.Forms.Label lblGain;
        private System.Windows.Forms.TrackBar tbFreq;
        private System.Windows.Forms.Label lblFreq;
        private System.Windows.Forms.ComboBox cbWaveType;
        private System.Windows.Forms.Label lblWaveType;
        private System.Windows.Forms.GroupBox gbTransmit;
        private System.Windows.Forms.TextBox tTXLive;
        private System.Windows.Forms.CheckBox chbKeepText;
        private System.Windows.Forms.ProgressBar pbTXTime;
        private System.Windows.Forms.TabControl tcTX;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage Macros;
        private System.Windows.Forms.GroupBox gbStation;
        private System.Windows.Forms.TextBox tGridMe;
        private System.Windows.Forms.Label lblGridMe;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.Label lblGrid;
        private System.Windows.Forms.TextBox tName;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox tQTH;
        private System.Windows.Forms.Label lblQTH;
        private System.Windows.Forms.TextBox tCallSign;
        private System.Windows.Forms.Label lblCallSign;
        private System.Windows.Forms.Button btnSaveMe;
        private System.Windows.Forms.Button btnEditMe;
        private System.Windows.Forms.TextBox tNameMe;
        private System.Windows.Forms.Label lblNameMe;
        private System.Windows.Forms.TextBox tQTHMe;
        private System.Windows.Forms.Label lblQTHMe;
        private System.Windows.Forms.TextBox tCallSignMe;
        private System.Windows.Forms.Label lblCallSignMe;
        private System.Windows.Forms.TabControl tcQSO;
        private System.Windows.Forms.TabPage tpMyStation;
        private System.Windows.Forms.TabPage tpRemoteStation;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnSaveTX;
        private System.Windows.Forms.Button btnRevertTX;
    }
}

