using BITCollege_DP.Data;
using BITCollegeWindows.CollegeRegistrationService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utility;

namespace BITCollegeWindows
{
    public partial class Grading : Form
    {

        ///given:  student and registration data will passed throughout 
        ///application. This object will be used to store the current
        ///student and selected registration
        ConstructorData constructorData;

        //data context object
        BITCollege_DPContext db = new BITCollege_DPContext();

        //service object
        CollegeRegistrationService.CollegeRegistrationClient service = new CollegeRegistrationService.CollegeRegistrationClient();

        /// <summary>
        /// given:  This constructor will be used when called from the
        /// Student form.  This constructor will receive 
        /// specific information about the student and registration
        /// further code required:  
        /// </summary>
        /// <param name="constructorData">constructorData object containing
        /// specific student and registration data.</param>
        public Grading(ConstructorData constructor)
        {
            InitializeComponent();

            this.constructorData = constructor;

            if (constructorData != null)
            {
                studentBindingSource.DataSource = constructor.Student;
                registrationBindingSource.DataSource = constructor.Registration;
            }
        }

        /// <summary>
        /// given: This code will navigate back to the Student form with
        /// the specific student and registration data that launched
        /// this form.
        /// </summary>
        private void lnkReturn_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //return to student with the data selected for this form
            StudentData student = new StudentData(constructorData);
            student.MdiParent = this.MdiParent;
            student.Show();
            this.Close();
        }

        /// <summary>
        /// given:  Always open in this form in the top right corner of the frame.
        /// further code required:
        /// </summary>
        private void Grading_Load(object sender, EventArgs e)
        {
            this.Location = new Point(0, 0);

            if(constructorData != null)
            {
                string courseType = constructorData.Registration.Course.CourseType;
                courseNumberMaskedLabel.Mask = BusinessRules.CourseFormat(courseType);
            }

            //if a grade has been previously entered, ie, the course has already been graded.
            if(constructorData.Registration.Grade != null)
            {
                gradeTextBox.Enabled = false;
                lnkUpdate.Enabled = false;
                lblExisting.Visible = true;
            }
            //if no grade has been previously entered, ie, the course is ungraded.
            else
            {
                gradeTextBox.Enabled = true;
                lnkUpdate.Enabled = true;
                lblExisting.Visible = false;
            }
        }

        /// <summary>
        /// Handles the logic for updating a student grade
        /// </summary>
        private void lnkUpdate_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //extract user input from textbox and strip the formatting.
            string formattedGrade = Numeric.ClearFormatting(this.gradeTextBox.Text, "%");

            //check if the input is a numeric value
            if(Utility.Numeric.IsNumeric(formattedGrade, System.Globalization.NumberStyles.Any))
            {
                double parsedGrade = Convert.ToDouble(formattedGrade) / 100;
                //double rawGrade = parsedGrade / 100;

                //check if the decimal grade is between 0 and 1.
                if (parsedGrade >= 0 &&  parsedGrade <= 1)
                {
                    try
                    {
                        service.UpdateGrade(parsedGrade, this.constructorData.Registration.RegistrationId, this.constructorData.Registration.Notes);

                        gradeTextBox.Enabled = false;
                        lnkUpdate.Enabled = false;
                        //lblExisting.Visible = true;

                        string message1 = "The grade update successful.",
                               caption1 = "Update Success";
                        MessageBoxButtons buttons = MessageBoxButtons.OK;

                        MessageBox.Show(message1, caption1, buttons);
                    }
                    catch (Exception ex)
                    {
                        string message = ex.Message,
                               caption = "Update Failed";
                        MessageBoxButtons buttons = MessageBoxButtons.OK;

                        MessageBox.Show(message, caption, buttons);
                    }
                }
                else
                {
                    string message = "The grade must be entered as a decimal value",
                           caption = "Invalid Grade";
                    MessageBoxButtons buttons = MessageBoxButtons.OK;

                    MessageBox.Show(message, caption, buttons);
                }
            }
            else
            {
                string message = "The grade must be entered as a numeric value",
                       caption = "Invalid Grade";
                MessageBoxButtons buttons = MessageBoxButtons.OK;

                MessageBox.Show(message, caption, buttons);
            }
        }
    }
}
