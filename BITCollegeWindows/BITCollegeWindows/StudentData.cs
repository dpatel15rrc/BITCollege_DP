using BITCollege_DP.Data;
using BITCollege_DP.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BITCollegeWindows
{
    public partial class StudentData : Form
    {
        ///Given: Student and Registration data will be retrieved
        ///in this form and passed throughout application
        ///These variables will be used to store the current
        ///Student and selected Registration
        ConstructorData constructorData = new ConstructorData();

        //data context object
        BITCollege_DPContext db = new BITCollege_DPContext();

        /// <summary>
        /// This constructor will be used when this form is opened from
        /// the MDI Frame.
        /// </summary>
        public StudentData()
        {
            InitializeComponent();

        }
 
        /// <summary>
        /// given:  This constructor will be used when returning to StudentData
        /// from another form.  This constructor will pass back
        /// specific information about the student and registration
        /// based on activites taking place in another form.
        /// </summary>
        /// <param name="constructorData">constructorData object containing
        /// specific student and registration data.</param>
        public StudentData (ConstructorData constructor)
        {
            InitializeComponent();
            //Further code to be added.

            this.constructorData = constructor;

            studentNumberMaskedTextBox.Text = constructor.Student.StudentNumber.ToString();
            studentNumberMaskedTextBox_Leave(null, null);
        }

        /// <summary>
        /// given: Open grading form passing constructor data.
        /// </summary>
        private void lnkUpdateGrade_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            PopulateConstructorData();

            Grading grading = new Grading(constructorData);
            grading.MdiParent = this.MdiParent;
            grading.Show();
            this.Close();
        }


        /// <summary>
        /// given: Open history form passing constructor data.
        /// </summary>
        private void lnkViewDetails_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            PopulateConstructorData();

            History history = new History(constructorData);
            history.MdiParent = this.MdiParent;
            history.Show();
            this.Close();
        }

        /// <summary>
        /// given:  Opens the form in top right corner of the frame.
        /// </summary>
        private void StudentData_Load(object sender, EventArgs e)
        {
            //keeps location of form static when opened and closed
            this.Location = new Point(0, 0);
        }

        private void studentNumberMaskedTextBox_Leave(object sender, EventArgs e)
        {
            //Ensure the user has completed the requirements for the Mask
            if (studentNumberMaskedTextBox.MaskCompleted)
            {
                //Retrieve records from the Students table where studentNumber entered by the user matches one in the db.
                long studentNumber = long.Parse(studentNumberMaskedTextBox.Text);

                IQueryable<Student> studentRecords = db.Students.Where(x => x.StudentNumber == studentNumber);

                Student student = studentRecords.FirstOrDefault();

                //If no records found
                if(student == null)
                {
                    lnkUpdateGrade.Enabled = false;
                    lnkViewDetails.Enabled = false;

                    studentNumberMaskedTextBox.Focus();

                    //clear the Student BindingSource object
                    studentBindingSource.DataSource = typeof(Student);

                    //clear the Registration BindingSource object
                    registrationBindingSource.DataSource = typeof(Registration);

                    String message = $"Student {studentNumber} does not exist.",
                           caption = "Invalid Student Number";
                    MessageBoxButtons button = MessageBoxButtons.OK;
                    DialogResult result = MessageBox.Show(message, caption, button);

                    studentNumberMaskedTextBox.Focus();
                }
                //if the student record was retrieved
                else
                {
                    studentBindingSource.DataSource = student;

                    IQueryable<Registration> studentRegistration = db.Registrations.Where(x => x.StudentId == student.StudentId);
                    List<Registration> registrationList = studentRegistration.ToList();

                    //if no registrations present in the list
                    if (registrationList.Count == 0)
                    {
                        lnkUpdateGrade.Enabled = false;
                        lnkViewDetails.Enabled = false;

                        //clear the Registration BindingSource object
                        registrationBindingSource.DataSource = typeof(Registration);
                    }
                    else
                    {
                        registrationBindingSource.DataSource = registrationList;

                        lnkUpdateGrade.Enabled = true;
                        lnkViewDetails.Enabled = true;
                    }
                }

                //set registration number:
                if(constructorData.Registration != null)
                {
                    registrationNumberComboBox.Text = constructorData.Registration.RegistrationNumber.ToString();
                }
            }
        }
    
        /// <summary>
        /// Populates the constructor data with current student and registration data.
        /// </summary>
        private void PopulateConstructorData()
        {
            Student currentStudent = (Student)studentBindingSource.Current;
            Registration currentRegistration = (Registration)registrationBindingSource.Current;

            constructorData.Student = currentStudent;
            constructorData.Registration = currentRegistration;

        }
    }
}
