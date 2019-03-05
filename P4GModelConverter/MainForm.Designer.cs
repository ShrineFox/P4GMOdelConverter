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
            this.btn_Extract = new System.Windows.Forms.Button();
            this.btn_Create = new System.Windows.Forms.Button();
            this.listBox_AnimationOrder = new System.Windows.Forms.ListBox();
            this.btn_Up = new System.Windows.Forms.Button();
            this.btn_Down = new System.Windows.Forms.Button();
            this.lbl_AnimOrder = new System.Windows.Forms.Label();
            this.chkBox_Extract = new System.Windows.Forms.CheckBox();
            this.chkBox_Animations = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btn_Extract
            // 
            this.btn_Extract.AllowDrop = true;
            this.btn_Extract.Location = new System.Drawing.Point(12, 6);
            this.btn_Extract.Name = "btn_Extract";
            this.btn_Extract.Size = new System.Drawing.Size(150, 140);
            this.btn_Extract.TabIndex = 0;
            this.btn_Extract.Text = "Drag a GMO or FBX model here to generate MDS";
            this.btn_Extract.UseVisualStyleBackColor = true;
            this.btn_Extract.DragDrop += new System.Windows.Forms.DragEventHandler(this.btn_Extract_DragDrop);
            this.btn_Extract.DragEnter += new System.Windows.Forms.DragEventHandler(this.btn_Extract_DragEnter);
            // 
            // btn_Create
            // 
            this.btn_Create.AllowDrop = true;
            this.btn_Create.Location = new System.Drawing.Point(170, 6);
            this.btn_Create.Name = "btn_Create";
            this.btn_Create.Size = new System.Drawing.Size(150, 140);
            this.btn_Create.TabIndex = 1;
            this.btn_Create.Text = "Drag a MDS here to generate new GMO model";
            this.btn_Create.UseVisualStyleBackColor = true;
            this.btn_Create.DragDrop += new System.Windows.Forms.DragEventHandler(this.btn_Create_DragDrop);
            this.btn_Create.DragEnter += new System.Windows.Forms.DragEventHandler(this.btn_Create_DragEnter);
            // 
            // listBox_AnimationOrder
            // 
            this.listBox_AnimationOrder.FormattingEnabled = true;
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
            this.listBox_AnimationOrder.Location = new System.Drawing.Point(326, 22);
            this.listBox_AnimationOrder.Name = "listBox_AnimationOrder";
            this.listBox_AnimationOrder.Size = new System.Drawing.Size(81, 134);
            this.listBox_AnimationOrder.TabIndex = 2;
            // 
            // btn_Up
            // 
            this.btn_Up.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.btn_Up.Location = new System.Drawing.Point(411, 22);
            this.btn_Up.Name = "btn_Up";
            this.btn_Up.Size = new System.Drawing.Size(29, 62);
            this.btn_Up.TabIndex = 3;
            this.btn_Up.Text = "↑";
            this.btn_Up.UseVisualStyleBackColor = true;
            this.btn_Up.Click += new System.EventHandler(this.Up_Click);
            // 
            // btn_Down
            // 
            this.btn_Down.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.btn_Down.Location = new System.Drawing.Point(411, 90);
            this.btn_Down.Name = "btn_Down";
            this.btn_Down.Size = new System.Drawing.Size(29, 62);
            this.btn_Down.TabIndex = 4;
            this.btn_Down.Text = "↓";
            this.btn_Down.UseVisualStyleBackColor = true;
            this.btn_Down.Click += new System.EventHandler(this.Down_Click);
            // 
            // lbl_AnimOrder
            // 
            this.lbl_AnimOrder.AutoSize = true;
            this.lbl_AnimOrder.Location = new System.Drawing.Point(322, 6);
            this.lbl_AnimOrder.Name = "lbl_AnimOrder";
            this.lbl_AnimOrder.Size = new System.Drawing.Size(82, 13);
            this.lbl_AnimOrder.TabIndex = 5;
            this.lbl_AnimOrder.Text = "Animation Order";
            // 
            // chkBox_Extract
            // 
            this.chkBox_Extract.AutoSize = true;
            this.chkBox_Extract.Checked = true;
            this.chkBox_Extract.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkBox_Extract.Location = new System.Drawing.Point(12, 152);
            this.chkBox_Extract.Name = "chkBox_Extract";
            this.chkBox_Extract.Size = new System.Drawing.Size(103, 17);
            this.chkBox_Extract.TabIndex = 6;
            this.chkBox_Extract.Text = "Extract Textures";
            this.chkBox_Extract.UseVisualStyleBackColor = true;
            // 
            // chkBox_Animations
            // 
            this.chkBox_Animations.AutoSize = true;
            this.chkBox_Animations.Checked = true;
            this.chkBox_Animations.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkBox_Animations.Location = new System.Drawing.Point(121, 152);
            this.chkBox_Animations.Name = "chkBox_Animations";
            this.chkBox_Animations.Size = new System.Drawing.Size(105, 17);
            this.chkBox_Animations.TabIndex = 7;
            this.chkBox_Animations.Text = "Keep Animations";
            this.chkBox_Animations.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(444, 171);
            this.Controls.Add(this.chkBox_Animations);
            this.Controls.Add(this.chkBox_Extract);
            this.Controls.Add(this.lbl_AnimOrder);
            this.Controls.Add(this.btn_Down);
            this.Controls.Add(this.btn_Up);
            this.Controls.Add(this.listBox_AnimationOrder);
            this.Controls.Add(this.btn_Create);
            this.Controls.Add(this.btn_Extract);
            this.MaximumSize = new System.Drawing.Size(460, 210);
            this.MinimumSize = new System.Drawing.Size(460, 200);
            this.Name = "MainForm";
            this.Text = "P4GMOdel Converter";
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
    }
}

