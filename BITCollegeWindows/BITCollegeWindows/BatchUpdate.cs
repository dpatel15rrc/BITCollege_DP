using BITCollege_DP.Data;
using BITCollege_DP.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BITCollegeWindows
{
    public partial class BatchUpdate : Form
    {
        private BITCollege_DPContext db = new BITCollege_DPContext();
        private Batch batch = new Batch();

        public BatchUpdate()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the Batch processing
        /// Further code to be added.
        /// </summary>
        private void lnkProcess_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (radSelect.Checked)
            {
                string acronym = programAcronymComboBox.SelectedValue.ToString();
                batch.ProcessTransmission(acronym);

                string log = batch.WriteLogData();
                rtxtLog.Text += log;
            }
            else if (radAll.Checked)
            {
                foreach (var item in programAcronymComboBox.Items)
                {
                    string acronym = ((AcademicProgram)item).ProgramAcronym;  
                    batch.ProcessTransmission(acronym);

                    string log = batch.WriteLogData();
                    rtxtLog.Text += log;
                }

            }
        }

        /// <summary>
        /// given:  Always open this form in top right of frame.
        /// Further code to be added.
        /// </summary>
        private void BatchUpdate_Load(object sender, EventArgs e)
        {
            this.Location = new Point(0, 0);

            IQueryable<AcademicProgram> academicPrograms = db.AcademicPrograms;
            academicProgramBindingSource.DataSource = academicPrograms.ToList();
        }

        //test button func.
        private void button1_Click(object sender, EventArgs e)
        {
            //Batch batch = new Batch();
            batch.ProcessTransmission("VT");
            rtxtLog.Text = batch.WriteLogData();
        }

        /// <summary>
        /// enable the comboBox if the "select" radio button is checked.
        /// </summary>
        private void radSelect_CheckedChanged(object sender, EventArgs e)
        {
            programAcronymComboBox.Enabled = radSelect.Checked;
        }
    }
}
