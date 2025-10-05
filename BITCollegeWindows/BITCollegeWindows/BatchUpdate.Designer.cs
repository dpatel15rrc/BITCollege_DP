namespace BITCollegeWindows
{
    partial class BatchUpdate
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
            System.Windows.Forms.Label programAcronymLabel;
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.programAcronymComboBox = new System.Windows.Forms.ComboBox();
            this.academicProgramBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.lnkProcess = new System.Windows.Forms.LinkLabel();
            this.radSelect = new System.Windows.Forms.RadioButton();
            this.radAll = new System.Windows.Forms.RadioButton();
            this.rtxtLog = new System.Windows.Forms.RichTextBox();
            programAcronymLabel = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.academicProgramBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // programAcronymLabel
            // 
            programAcronymLabel.AutoSize = true;
            programAcronymLabel.Location = new System.Drawing.Point(66, 122);
            programAcronymLabel.Name = "programAcronymLabel";
            programAcronymLabel.Size = new System.Drawing.Size(118, 16);
            programAcronymLabel.TabIndex = 4;
            programAcronymLabel.Text = "Program Acronym:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(programAcronymLabel);
            this.groupBox1.Controls.Add(this.programAcronymComboBox);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.lnkProcess);
            this.groupBox1.Controls.Add(this.radSelect);
            this.groupBox1.Controls.Add(this.radAll);
            this.groupBox1.Location = new System.Drawing.Point(35, 46);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(952, 208);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Batch Selection";
            // 
            // programAcronymComboBox
            // 
            this.programAcronymComboBox.DataSource = this.academicProgramBindingSource;
            this.programAcronymComboBox.DisplayMember = "Description";
            this.programAcronymComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.programAcronymComboBox.Enabled = false;
            this.programAcronymComboBox.FormattingEnabled = true;
            this.programAcronymComboBox.Location = new System.Drawing.Point(190, 119);
            this.programAcronymComboBox.Name = "programAcronymComboBox";
            this.programAcronymComboBox.Size = new System.Drawing.Size(259, 24);
            this.programAcronymComboBox.TabIndex = 5;
            this.programAcronymComboBox.ValueMember = "ProgramAcronym";
            // 
            // academicProgramBindingSource
            // 
            this.academicProgramBindingSource.DataSource = typeof(BITCollege_DP.Models.AcademicProgram);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(599, 116);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "test";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // lnkProcess
            // 
            this.lnkProcess.AutoSize = true;
            this.lnkProcess.Location = new System.Drawing.Point(55, 164);
            this.lnkProcess.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lnkProcess.Name = "lnkProcess";
            this.lnkProcess.Size = new System.Drawing.Size(94, 16);
            this.lnkProcess.TabIndex = 2;
            this.lnkProcess.TabStop = true;
            this.lnkProcess.Text = "Process Batch";
            this.lnkProcess.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkProcess_LinkClicked);
            // 
            // radSelect
            // 
            this.radSelect.AutoSize = true;
            this.radSelect.Location = new System.Drawing.Point(59, 73);
            this.radSelect.Margin = new System.Windows.Forms.Padding(4);
            this.radSelect.Name = "radSelect";
            this.radSelect.Size = new System.Drawing.Size(267, 20);
            this.radSelect.TabIndex = 1;
            this.radSelect.Text = "Select a Program to Grade and Register";
            this.radSelect.UseVisualStyleBackColor = true;
            this.radSelect.CheckedChanged += new System.EventHandler(this.radSelect_CheckedChanged);
            // 
            // radAll
            // 
            this.radAll.AutoSize = true;
            this.radAll.Location = new System.Drawing.Point(59, 44);
            this.radAll.Margin = new System.Windows.Forms.Padding(4);
            this.radAll.Name = "radAll";
            this.radAll.Size = new System.Drawing.Size(252, 20);
            this.radAll.TabIndex = 0;
            this.radAll.Text = "Grade and Register for ALL Programs";
            this.radAll.UseVisualStyleBackColor = true;
            // 
            // rtxtLog
            // 
            this.rtxtLog.Location = new System.Drawing.Point(35, 294);
            this.rtxtLog.Margin = new System.Windows.Forms.Padding(4);
            this.rtxtLog.Name = "rtxtLog";
            this.rtxtLog.Size = new System.Drawing.Size(951, 207);
            this.rtxtLog.TabIndex = 1;
            this.rtxtLog.Text = "";
            // 
            // BatchUpdate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1067, 554);
            this.Controls.Add(this.rtxtLog);
            this.Controls.Add(this.groupBox1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "BatchUpdate";
            this.Text = "Batch Student Update";
            this.Load += new System.EventHandler(this.BatchUpdate_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.academicProgramBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.LinkLabel lnkProcess;
        private System.Windows.Forms.RadioButton radSelect;
        private System.Windows.Forms.RadioButton radAll;
        private System.Windows.Forms.RichTextBox rtxtLog;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.BindingSource academicProgramBindingSource;
        private System.Windows.Forms.ComboBox programAcronymComboBox;
    }
}