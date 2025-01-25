namespace Cutting
{
    partial class frmTime
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.comboMachine = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.comboType = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtRunTime = new System.Windows.Forms.TextBox();
            this.dateTimeProd = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.comboDep = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.comboProcess = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.DGV_Sear_details = new System.Windows.Forms.DataGridView();
            this.txtSQM = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtGlassID = new System.Windows.Forms.TextBox();
            this.txtDEPID = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.txtTimeSQM = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtMin = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_Sear_details)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Outset;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.comboMachine, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.label9, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.comboType, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtRunTime, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.dateTimeProd, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.comboDep, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.comboProcess, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.label10, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 5);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 23);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 6;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(417, 179);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // comboMachine
            // 
            this.comboMachine.Enabled = false;
            this.comboMachine.FormattingEnabled = true;
            this.comboMachine.Items.AddRange(new object[] {
            "Glass Robort ",
            "South Tech",
            "Automatic",
            "Manual"});
            this.comboMachine.Location = new System.Drawing.Point(124, 94);
            this.comboMachine.Name = "comboMachine";
            this.comboMachine.Size = new System.Drawing.Size(266, 21);
            this.comboMachine.TabIndex = 46;
            this.comboMachine.SelectedIndexChanged += new System.EventHandler(this.comboMachine_SelectedIndexChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(5, 91);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(109, 15);
            this.label9.TabIndex = 46;
            this.label9.Text = "Machine / Furnace";
            // 
            // comboType
            // 
            this.comboType.Enabled = false;
            this.comboType.FormattingEnabled = true;
            this.comboType.Location = new System.Drawing.Point(124, 65);
            this.comboType.Name = "comboType";
            this.comboType.Size = new System.Drawing.Size(266, 21);
            this.comboType.TabIndex = 31;
            this.comboType.SelectedIndexChanged += new System.EventHandler(this.comboType_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(5, 2);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "Date";
            // 
            // txtRunTime
            // 
            this.txtRunTime.Location = new System.Drawing.Point(124, 152);
            this.txtRunTime.Name = "txtRunTime";
            this.txtRunTime.Size = new System.Drawing.Size(101, 20);
            this.txtRunTime.TabIndex = 33;
            this.txtRunTime.TextChanged += new System.EventHandler(this.txtRunTime_TextChanged);
            this.txtRunTime.DoubleClick += new System.EventHandler(this.txtRunTime_DoubleClick);
            this.txtRunTime.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtRunTime_KeyPress);
            // 
            // dateTimeProd
            // 
            this.dateTimeProd.CustomFormat = "yyyy-mm-dd";
            this.dateTimeProd.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateTimeProd.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimeProd.Location = new System.Drawing.Point(124, 5);
            this.dateTimeProd.Name = "dateTimeProd";
            this.dateTimeProd.Size = new System.Drawing.Size(101, 23);
            this.dateTimeProd.TabIndex = 29;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(5, 33);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 15);
            this.label4.TabIndex = 4;
            this.label4.Text = "Department";
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // comboDep
            // 
            this.comboDep.FormattingEnabled = true;
            this.comboDep.Items.AddRange(new object[] {
            "Cutting",
            "Arrissing",
            "Grinding",
            "Printing",
            "SandBlast",
            "Tempering",
            "Lamination",
            "IGU",
            "Bonding"});
            this.comboDep.Location = new System.Drawing.Point(124, 36);
            this.comboDep.Name = "comboDep";
            this.comboDep.Size = new System.Drawing.Size(266, 21);
            this.comboDep.TabIndex = 30;
            this.comboDep.SelectedIndexChanged += new System.EventHandler(this.comboDep_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(5, 62);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 15);
            this.label3.TabIndex = 3;
            this.label3.Text = "Glass Type";
            // 
            // comboProcess
            // 
            this.comboProcess.Enabled = false;
            this.comboProcess.FormattingEnabled = true;
            this.comboProcess.Location = new System.Drawing.Point(124, 123);
            this.comboProcess.Name = "comboProcess";
            this.comboProcess.Size = new System.Drawing.Size(266, 21);
            this.comboProcess.TabIndex = 46;
            this.comboProcess.SelectedIndexChanged += new System.EventHandler(this.comboProcess_SelectedIndexChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(5, 120);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(76, 15);
            this.label10.TabIndex = 46;
            this.label10.Text = "Process type";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(5, 149);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(111, 15);
            this.label1.TabIndex = 32;
            this.label1.Text = "Run Time (Min.)";
            // 
            // button1
            // 
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Image = global::Cutting.Properties.Resources._1492947267_Save;
            this.button1.Location = new System.Drawing.Point(498, 23);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(118, 44);
            this.button1.TabIndex = 40;
            this.button1.Text = "Save";
            this.button1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // DGV_Sear_details
            // 
            this.DGV_Sear_details.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGV_Sear_details.Location = new System.Drawing.Point(15, 208);
            this.DGV_Sear_details.Name = "DGV_Sear_details";
            this.DGV_Sear_details.Size = new System.Drawing.Size(604, 163);
            this.DGV_Sear_details.TabIndex = 41;
            // 
            // txtSQM
            // 
            this.txtSQM.Enabled = false;
            this.txtSQM.Location = new System.Drawing.Point(78, 5);
            this.txtSQM.Name = "txtSQM";
            this.txtSQM.Size = new System.Drawing.Size(50, 20);
            this.txtSQM.TabIndex = 42;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(5, 2);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 15);
            this.label5.TabIndex = 43;
            this.label5.Text = "Total SQM";
            // 
            // txtGlassID
            // 
            this.txtGlassID.Enabled = false;
            this.txtGlassID.Location = new System.Drawing.Point(335, 5);
            this.txtGlassID.Name = "txtGlassID";
            this.txtGlassID.Size = new System.Drawing.Size(149, 20);
            this.txtGlassID.TabIndex = 43;
            // 
            // txtDEPID
            // 
            this.txtDEPID.Enabled = false;
            this.txtDEPID.Location = new System.Drawing.Point(548, 5);
            this.txtDEPID.Name = "txtDEPID";
            this.txtDEPID.Size = new System.Drawing.Size(50, 20);
            this.txtDEPID.TabIndex = 44;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(274, 2);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 15);
            this.label6.TabIndex = 44;
            this.label6.Text = "Glass ID";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.tableLayoutPanel2.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Outset;
            this.tableLayoutPanel2.ColumnCount = 8;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.Controls.Add(this.txtTimeSQM, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.label8, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.label5, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.txtSQM, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.txtDEPID, 7, 0);
            this.tableLayoutPanel2.Controls.Add(this.label7, 6, 0);
            this.tableLayoutPanel2.Controls.Add(this.txtGlassID, 5, 0);
            this.tableLayoutPanel2.Controls.Add(this.label6, 4, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(15, 377);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(604, 31);
            this.tableLayoutPanel2.TabIndex = 45;
            // 
            // txtTimeSQM
            // 
            this.txtTimeSQM.Enabled = false;
            this.txtTimeSQM.Location = new System.Drawing.Point(216, 5);
            this.txtTimeSQM.Name = "txtTimeSQM";
            this.txtTimeSQM.Size = new System.Drawing.Size(50, 20);
            this.txtTimeSQM.TabIndex = 46;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(136, 2);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(72, 15);
            this.label8.TabIndex = 46;
            this.label8.Text = "Time / SQM";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(492, 2);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(48, 15);
            this.label7.TabIndex = 46;
            this.label7.Text = "Dep. ID";
            // 
            // txtMin
            // 
            this.txtMin.Enabled = false;
            this.txtMin.Location = new System.Drawing.Point(483, 167);
            this.txtMin.Name = "txtMin";
            this.txtMin.Size = new System.Drawing.Size(50, 20);
            this.txtMin.TabIndex = 47;
            this.txtMin.Visible = false;
            // 
            // frmTime
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.ClientSize = new System.Drawing.Size(631, 420);
            this.Controls.Add(this.txtMin);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Controls.Add(this.DGV_Sear_details);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Location = new System.Drawing.Point(180, 20);
            this.Name = "frmTime";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Production time";
            this.Load += new System.EventHandler(this.frmTime_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_Sear_details)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dateTimeProd;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboType;
        private System.Windows.Forms.ComboBox comboDep;
        private System.Windows.Forms.TextBox txtRunTime;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridView DGV_Sear_details;
        private System.Windows.Forms.TextBox txtSQM;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtGlassID;
        private System.Windows.Forms.TextBox txtDEPID;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtTimeSQM;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox comboMachine;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox comboProcess;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtMin;
    }
}