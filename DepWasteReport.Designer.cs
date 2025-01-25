using Cutting.DataSet1TableAdapters;

namespace Cutting
{
    partial class DepWasteReport
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
            this.components = new System.ComponentModel.Container();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource1 = new Microsoft.Reporting.WinForms.ReportDataSource();
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.label2 = new System.Windows.Forms.Label();
            this.dtDepToDate = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.dtDepFromDate = new System.Windows.Forms.DateTimePicker();
            this.btn_DepShow = new System.Windows.Forms.Button();
            this.DataTable1BindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.DataSet1 = new DataSet1();
            this.DataTable1TableAdapter = new DataTable1TableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.DataTable1BindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DataSet1)).BeginInit();
            this.SuspendLayout();
            // 
            // reportViewer1
            // 
            reportDataSource1.Name = "DataSet1";
            reportDataSource1.Value = this.DataTable1BindingSource;
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource1);
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "Cutting.Report1.rdlc";
            this.reportViewer1.Location = new System.Drawing.Point(12, 61);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.Size = new System.Drawing.Size(826, 478);
            this.reportViewer1.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(234, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 16);
            this.label2.TabIndex = 12;
            this.label2.Text = "To Date :";
            // 
            // dtDepToDate
            // 
            this.dtDepToDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtDepToDate.Location = new System.Drawing.Point(317, 20);
            this.dtDepToDate.Name = "dtDepToDate";
            this.dtDepToDate.Size = new System.Drawing.Size(109, 20);
            this.dtDepToDate.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(23, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 16);
            this.label1.TabIndex = 13;
            this.label1.Text = "From Date :";
            // 
            // dtDepFromDate
            // 
            this.dtDepFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtDepFromDate.Location = new System.Drawing.Point(106, 20);
            this.dtDepFromDate.Name = "dtDepFromDate";
            this.dtDepFromDate.Size = new System.Drawing.Size(109, 20);
            this.dtDepFromDate.TabIndex = 11;
            // 
            // btn_DepShow
            // 
            this.btn_DepShow.Location = new System.Drawing.Point(469, 19);
            this.btn_DepShow.Name = "btn_DepShow";
            this.btn_DepShow.Size = new System.Drawing.Size(75, 23);
            this.btn_DepShow.TabIndex = 9;
            this.btn_DepShow.Text = "Show";
            this.btn_DepShow.UseVisualStyleBackColor = true;
            this.btn_DepShow.Click += new System.EventHandler(this.btn_DepShow_Click);
            // 
            // DataTable1BindingSource
            // 
            this.DataTable1BindingSource.DataMember = "DataTable1";
            this.DataTable1BindingSource.DataSource = this.DataSet1;
            // 
            // DataSet1
            // 
            this.DataSet1.DataSetName = "DataSet1";
            this.DataSet1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // DataTable1TableAdapter
            // 
            this.DataTable1TableAdapter.ClearBeforeFill = true;
            // 
            // DepWasteReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(850, 551);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dtDepToDate);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dtDepFromDate);
            this.Controls.Add(this.btn_DepShow);
            this.Controls.Add(this.reportViewer1);
            this.Name = "DepWasteReport";
            this.Text = "DepWasteReport";
            this.Load += new System.EventHandler(this.DepWasteReport_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DataTable1BindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DataSet1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private System.Windows.Forms.BindingSource DataTable1BindingSource;
        private DataSet1 DataSet1;
        private DataSet1TableAdapters.DataTable1TableAdapter DataTable1TableAdapter;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtDepToDate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtDepFromDate;
        private System.Windows.Forms.Button btn_DepShow;
    }
}