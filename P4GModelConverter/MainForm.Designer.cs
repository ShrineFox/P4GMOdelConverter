namespace P4GModelConverter
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.btn_Extract = new System.Windows.Forms.Button();
            this.btn_Create = new System.Windows.Forms.Button();
            this.listBox_AnimationOrder = new System.Windows.Forms.ListBox();
            this.btn_Up = new System.Windows.Forms.Button();
            this.btn_Down = new System.Windows.Forms.Button();
            this.lbl_AnimOrder = new System.Windows.Forms.Label();
            this.chkBox_Extract = new System.Windows.Forms.CheckBox();
            this.chkBox_Animations = new System.Windows.Forms.CheckBox();
            this.lbl_AnimationsLoaded = new System.Windows.Forms.Label();
            this.btn_ExportAnim = new System.Windows.Forms.Button();
            this.btn_ImportAnim = new System.Windows.Forms.Button();
            this.chkBox_Dummy = new System.Windows.Forms.CheckBox();
            this.lbl_WpnBone = new System.Windows.Forms.Label();
            this.txt_WpnBone = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btn_Extract
            // 
            this.btn_Extract.AllowDrop = true;
            this.btn_Extract.Location = new System.Drawing.Point(16, 7);
            this.btn_Extract.Margin = new System.Windows.Forms.Padding(4);
            this.btn_Extract.Name = "btn_Extract";
            this.btn_Extract.Size = new System.Drawing.Size(200, 172);
            this.btn_Extract.TabIndex = 0;
            this.btn_Extract.Text = "Generate MDS\r\nfrom FBX or GMO\r\n\r\n(and load animation)";
            this.btn_Extract.UseVisualStyleBackColor = true;
            this.btn_Extract.Click += new System.EventHandler(this.btn_Extract_Click);
            this.btn_Extract.DragDrop += new System.Windows.Forms.DragEventHandler(this.btn_Extract_DragDrop);
            this.btn_Extract.DragEnter += new System.Windows.Forms.DragEventHandler(this.btn_Extract_DragEnter);
            // 
            // btn_Create
            // 
            this.btn_Create.AllowDrop = true;
            this.btn_Create.Location = new System.Drawing.Point(227, 7);
            this.btn_Create.Margin = new System.Windows.Forms.Padding(4);
            this.btn_Create.Name = "btn_Create";
            this.btn_Create.Size = new System.Drawing.Size(200, 172);
            this.btn_Create.TabIndex = 1;
            this.btn_Create.Text = "Generate new GMO\r\nfrom MDS\r\n\r\n(Including loaded animation)";
            this.btn_Create.UseVisualStyleBackColor = true;
            this.btn_Create.Click += new System.EventHandler(this.btn_Create_Click);
            this.btn_Create.DragDrop += new System.Windows.Forms.DragEventHandler(this.btn_Create_DragDrop);
            this.btn_Create.DragEnter += new System.Windows.Forms.DragEventHandler(this.btn_Create_DragEnter);
            // 
            // listBox_AnimationOrder
            // 
            this.listBox_AnimationOrder.FormattingEnabled = true;
            this.listBox_AnimationOrder.ItemHeight = 16;
            this.listBox_AnimationOrder.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23",
            "24",
            "25",
            "26",
            "27",
            "28",
            "29",
            "30",
            "31",
            "32",
            "33",
            "34",
            "35",
            "36",
            "37",
            "38",
            "39",
            "40",
            "41",
            "42",
            "43",
            "44",
            "45",
            "46",
            "47",
            "48",
            "49",
            "50",
            "51",
            "52",
            "53",
            "54",
            "55",
            "56",
            "57",
            "58",
            "59",
            "60",
            "61",
            "62",
            "63",
            "64",
            "65",
            "66",
            "67",
            "68",
            "69",
            "70",
            "71",
            "72",
            "73",
            "74",
            "75",
            "76",
            "77",
            "78",
            "79",
            "80",
            "81",
            "82",
            "83",
            "84",
            "85",
            "86",
            "87",
            "88",
            "89",
            "90",
            "91",
            "92",
            "93",
            "94",
            "95",
            "96",
            "97",
            "98",
            "99"});
            this.listBox_AnimationOrder.Location = new System.Drawing.Point(435, 30);
            this.listBox_AnimationOrder.Margin = new System.Windows.Forms.Padding(4);
            this.listBox_AnimationOrder.Name = "listBox_AnimationOrder";
            this.listBox_AnimationOrder.Size = new System.Drawing.Size(107, 148);
            this.listBox_AnimationOrder.TabIndex = 2;
            // 
            // btn_Up
            // 
            this.btn_Up.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.btn_Up.Location = new System.Drawing.Point(548, 29);
            this.btn_Up.Margin = new System.Windows.Forms.Padding(4);
            this.btn_Up.Name = "btn_Up";
            this.btn_Up.Size = new System.Drawing.Size(39, 71);
            this.btn_Up.TabIndex = 3;
            this.btn_Up.Text = "↑";
            this.btn_Up.UseVisualStyleBackColor = true;
            this.btn_Up.Click += new System.EventHandler(this.Up_Click);
            // 
            // btn_Down
            // 
            this.btn_Down.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.btn_Down.Location = new System.Drawing.Point(548, 108);
            this.btn_Down.Margin = new System.Windows.Forms.Padding(4);
            this.btn_Down.Name = "btn_Down";
            this.btn_Down.Size = new System.Drawing.Size(39, 71);
            this.btn_Down.TabIndex = 4;
            this.btn_Down.Text = "↓";
            this.btn_Down.UseVisualStyleBackColor = true;
            this.btn_Down.Click += new System.EventHandler(this.Down_Click);
            // 
            // lbl_AnimOrder
            // 
            this.lbl_AnimOrder.AutoSize = true;
            this.lbl_AnimOrder.Location = new System.Drawing.Point(455, 7);
            this.lbl_AnimOrder.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_AnimOrder.Name = "lbl_AnimOrder";
            this.lbl_AnimOrder.Size = new System.Drawing.Size(111, 17);
            this.lbl_AnimOrder.TabIndex = 5;
            this.lbl_AnimOrder.Text = "Animation Order";
            // 
            // chkBox_Extract
            // 
            this.chkBox_Extract.AutoSize = true;
            this.chkBox_Extract.Location = new System.Drawing.Point(16, 180);
            this.chkBox_Extract.Margin = new System.Windows.Forms.Padding(4);
            this.chkBox_Extract.Name = "chkBox_Extract";
            this.chkBox_Extract.Size = new System.Drawing.Size(169, 21);
            this.chkBox_Extract.TabIndex = 6;
            this.chkBox_Extract.Text = "Extract GMO Textures";
            this.chkBox_Extract.UseVisualStyleBackColor = true;
            // 
            // chkBox_Animations
            // 
            this.chkBox_Animations.AutoSize = true;
            this.chkBox_Animations.Checked = true;
            this.chkBox_Animations.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkBox_Animations.Location = new System.Drawing.Point(16, 204);
            this.chkBox_Animations.Margin = new System.Windows.Forms.Padding(4);
            this.chkBox_Animations.Name = "chkBox_Animations";
            this.chkBox_Animations.Size = new System.Drawing.Size(172, 21);
            this.chkBox_Animations.TabIndex = 7;
            this.chkBox_Animations.Text = "Load GMO Animations";
            this.chkBox_Animations.UseVisualStyleBackColor = true;
            // 
            // lbl_AnimationsLoaded
            // 
            this.lbl_AnimationsLoaded.AutoSize = true;
            this.lbl_AnimationsLoaded.Location = new System.Drawing.Point(436, 227);
            this.lbl_AnimationsLoaded.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_AnimationsLoaded.Name = "lbl_AnimationsLoaded";
            this.lbl_AnimationsLoaded.Size = new System.Drawing.Size(151, 17);
            this.lbl_AnimationsLoaded.TabIndex = 8;
            this.lbl_AnimationsLoaded.Text = "No Animations Loaded";
            // 
            // btn_ExportAnim
            // 
            this.btn_ExportAnim.AllowDrop = true;
            this.btn_ExportAnim.Enabled = false;
            this.btn_ExportAnim.Location = new System.Drawing.Point(435, 187);
            this.btn_ExportAnim.Margin = new System.Windows.Forms.Padding(4);
            this.btn_ExportAnim.Name = "btn_ExportAnim";
            this.btn_ExportAnim.Size = new System.Drawing.Size(73, 29);
            this.btn_ExportAnim.TabIndex = 10;
            this.btn_ExportAnim.Text = "Export";
            this.btn_ExportAnim.UseVisualStyleBackColor = true;
            this.btn_ExportAnim.Click += new System.EventHandler(this.btn_ExportAnim_Click);
            // 
            // btn_ImportAnim
            // 
            this.btn_ImportAnim.AllowDrop = true;
            this.btn_ImportAnim.Location = new System.Drawing.Point(514, 187);
            this.btn_ImportAnim.Margin = new System.Windows.Forms.Padding(4);
            this.btn_ImportAnim.Name = "btn_ImportAnim";
            this.btn_ImportAnim.Size = new System.Drawing.Size(73, 29);
            this.btn_ImportAnim.TabIndex = 11;
            this.btn_ImportAnim.Text = "Import";
            this.btn_ImportAnim.UseVisualStyleBackColor = true;
            this.btn_ImportAnim.Click += new System.EventHandler(this.btn_ImportAnim_Click);
            // 
            // chkBox_Dummy
            // 
            this.chkBox_Dummy.AutoSize = true;
            this.chkBox_Dummy.Checked = true;
            this.chkBox_Dummy.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkBox_Dummy.Location = new System.Drawing.Point(16, 226);
            this.chkBox_Dummy.Margin = new System.Windows.Forms.Padding(4);
            this.chkBox_Dummy.Name = "chkBox_Dummy";
            this.chkBox_Dummy.Size = new System.Drawing.Size(138, 21);
            this.chkBox_Dummy.TabIndex = 12;
            this.chkBox_Dummy.Text = "Dummy Materials";
            this.chkBox_Dummy.UseVisualStyleBackColor = true;
            // 
            // lbl_WpnBone
            // 
            this.lbl_WpnBone.AutoSize = true;
            this.lbl_WpnBone.Location = new System.Drawing.Point(224, 202);
            this.lbl_WpnBone.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_WpnBone.Name = "lbl_WpnBone";
            this.lbl_WpnBone.Size = new System.Drawing.Size(143, 17);
            this.lbl_WpnBone.TabIndex = 13;
            this.lbl_WpnBone.Text = "Weapon Bone Name:";
            // 
            // txt_WpnBone
            // 
            this.txt_WpnBone.Location = new System.Drawing.Point(227, 222);
            this.txt_WpnBone.Name = "txt_WpnBone";
            this.txt_WpnBone.Size = new System.Drawing.Size(200, 22);
            this.txt_WpnBone.TabIndex = 14;
            this.txt_WpnBone.Text = "Bip01 L Hand_Bone";
            // 
            // MainForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(592, 253);
            this.Controls.Add(this.txt_WpnBone);
            this.Controls.Add(this.lbl_WpnBone);
            this.Controls.Add(this.chkBox_Dummy);
            this.Controls.Add(this.btn_ImportAnim);
            this.Controls.Add(this.btn_ExportAnim);
            this.Controls.Add(this.lbl_AnimationsLoaded);
            this.Controls.Add(this.chkBox_Animations);
            this.Controls.Add(this.chkBox_Extract);
            this.Controls.Add(this.lbl_AnimOrder);
            this.Controls.Add(this.btn_Down);
            this.Controls.Add(this.btn_Up);
            this.Controls.Add(this.listBox_AnimationOrder);
            this.Controls.Add(this.btn_Create);
            this.Controls.Add(this.btn_Extract);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximumSize = new System.Drawing.Size(610, 300);
            this.MinimumSize = new System.Drawing.Size(610, 300);
            this.Name = "MainForm";
            this.Text = "P4GMOdel Converter 1.3";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_Extract;
        private System.Windows.Forms.Button btn_Create;
        private System.Windows.Forms.ListBox listBox_AnimationOrder;
        private System.Windows.Forms.Button btn_Up;
        private System.Windows.Forms.Button btn_Down;
        private System.Windows.Forms.Label lbl_AnimOrder;
        private System.Windows.Forms.CheckBox chkBox_Extract;
        private System.Windows.Forms.CheckBox chkBox_Animations;
        private System.Windows.Forms.Label lbl_AnimationsLoaded;
        private System.Windows.Forms.Button btn_ExportAnim;
        private System.Windows.Forms.Button btn_ImportAnim;
        private System.Windows.Forms.CheckBox chkBox_Dummy;
        private System.Windows.Forms.Label lbl_WpnBone;
        private System.Windows.Forms.TextBox txt_WpnBone;
    }
}

