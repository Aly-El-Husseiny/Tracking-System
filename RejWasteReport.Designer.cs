using Cutting.DataSet2TableAdapters;

namespace Cutting
{
    partial class RejWasteReport
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
            this.DataSet2 = new DataSet2();
            this.DataTable1BindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.DataTable1TableAdapter = new DataTable1TableAdapter();
            this.label2 = new System.Windows.Forms.Label();
            this.dtRejToDate = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.dtRejFromDate = new System.Windows.Forms.DateTimePicker();
            this.btn_RejShow = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.DataSet2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DataTable1BindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // reportViewer1
            // 
            reportDataSource1.Name = "DataSet2";
            reportDataSource1.Value = this.DataTable1BindingSource;
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource1);
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "Cutting.Report2.rdlc";
            this.reportViewer1.Location = new System.Drawing.Point(12, 59);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.Size = new System.Drawing.Size(821, 482);
            this.reportViewer1.TabIndex = 0;
            // 
            // DataSet2
            // 
            this.DataSet2.DataSetName = "DataSet2";
            this.DataSet2.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // DataTable1BindingSource
            // 
            this.DataTable1BindingSource.DataMember = "DataTable1";
            this.DataTable1BindingSource.DataSource = this.DataSet2;
            // 
            // DataTable1TableAdapter
            // 
            this.DataTable1TableAdapter.ClearBeforeFill = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(223, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 16);
            this.label2.TabIndex = 17;
            this.label2.Text = "To Date :";
            // 
            // dtRejToDate
            // 
            this.dtRejToDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtRejToDate.Location = new System.Drawing.Point(306, 22);
            this.dtRejToDate.Name = "dtRejToDate";
            this.dtRejToDate.Size = new System.Drawing.Size(109, 20);
            this.dtRejToDate.TabIndex = 15;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 16);
            this.label1.TabIndex = 18;
            this.label1.Text = "From Date :";
            // 
            // dtRejFromDate
            // 
            this.dtRejFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtRejFromDate.Location = new System.Drawing.Point(95, 22);
            this.dtRejFromDate.Name = "dtRejFromDate";
            this.dtRejFromDate.Size = new System.Drawing.Size(109, 20);
            this.dtRejFromDate.TabIndex = 16;
            // 
            // btn_RejShow
            // 
            this.btn_RejShow.Location = new System.Drawing.Point(458, 21);
            this.btn_RejShow.Name = "btn_RejShow";
            this.btn_RejShow.Size = new System.Drawing.Size(75, 23);
            this.btn_RejShow.TabIndex = 14;
            this.btn_RejShow.Text = "Show";
            this.btn_RejShow.UseVisualStyleBackColor = true;
            this.btn_RejShow.Click += new System.EventHandler(this.btn_RejShow_Click);
            // 
            // RejWasteReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(845, 553);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dtRejToDate);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dtRejFromDate);
            this.Controls.Add(this.btn_RejShow);
            this.Controls.Add(this.reportViewer1);
            this.Name = "RejWasteReport";
            this.Text = "Rejection Report";
            this.Load += new System.EventHandler(this.RejWasteReport_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DataSet2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DataTable1BindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private System.Windows.Forms.BindingSource DataTable1BindingSource;
        private DataSet2 DataSet2;
        private DataSet2TableAdapters.DataTable1TableAdapter DataTable1TableAdapter;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtRejToDate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtRejFromDate;
        private System.Windows.Forms.Button btn_RejShow;
    }
}