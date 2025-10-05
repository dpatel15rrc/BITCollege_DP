using BITCollege_DP.Data;
using BITCollege_DP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Utility;

namespace BITCollegeService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "CollegeRegistration" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select CollegeRegistration.svc or CollegeRegistration.svc.cs at the Solution Explorer and start debugging.
    public class CollegeRegistration : ICollegeRegistration
    {
        //data context object.
        BITCollege_DPContext db = new BITCollege_DPContext();

        public void DoWork()
        {
        }

        /// <summary>
        /// Implementation will retrieve the Registration record from database, remove it and persist the change.
        /// </summary>
        /// <param name="registrationId">Represents the registration  ID of the course. </param>
        /// <returns> True if the record was removed from the database, and false if it wasn't. </returns>
        public bool DropCourse(int registrationId)
        {
            bool result = false;

            try
            {
                Registration registrationToRemove = db.Registrations.Where(x => x.RegistrationId == registrationId).SingleOrDefault();

                if(registrationToRemove != null)
                {
                    db.Registrations.Remove(registrationToRemove);
                    db.SaveChanges();
                    result = true;
                }
                else 
                { 
                    result = false; 
                }
            }
            catch
            {
                result = false;
            }

            return result;
        }

        /// <summary>
        /// Implementation will register a student for a course.
        /// In the event that the course registration fails, the return code will indicate the reason
        /// </summary>
        /// <param name="studentId"> Represents the student ID. </param>
        /// <param name="courseId"> Represents the course ID. </param>
        /// <param name="notes"> Represents the notes. </param>
        /// <returns> Different errorCodes to indicate success or failure when registering for a course. </returns>
        public int RegisterCourse(int studentId, int courseId, string notes)
        {
            int returnCode = 0;

            IQueryable<Registration> allRegistrations = db.Registrations.Where(x => x.StudentId == studentId && x.CourseId == courseId);  

            IEnumerable<Registration> incompleteRegistrations = allRegistrations.Where(x => x.Grade == null);

            if (incompleteRegistrations.Count() > 0)
            {
                returnCode = -100;
            }

            Course course = db.Courses.Where(x => x.CourseId == courseId).SingleOrDefault();
            CourseType courseType = BusinessRules.CourseTypeLookup(course.CourseType);

            if (courseType == CourseType.MASTERY)
            {
                MasteryCourse masteryCourse = (MasteryCourse)course;

                int maxAttempts = masteryCourse.MaximumAttempts;

                IEnumerable<Registration> completedRegistrations = allRegistrations.Where(x => x.Grade != null);

                if (completedRegistrations.Count() >= maxAttempts)
                {
                    returnCode = -200;
                }
            }

            if (returnCode == 0)
            {
                try
                {
                    Registration newRegistration = new Registration
                    {
                        StudentId = studentId,
                        CourseId = courseId,
                        Notes = notes,
                        RegistrationDate = DateTime.Now,
                    };

                    newRegistration.SetNextRegistrationNumber();

                    db.Registrations.Add(newRegistration);
                    db.SaveChanges();

                    Student student = db.Students.Where(x => x.StudentId == studentId).SingleOrDefault();

                    double tuitionAmount = course.TuitionAmount;
                    double adjustedTuitionRate = student.GradePointState.TuitionRateAdjustment(student);
                    double adjustedTuitionAmount = tuitionAmount * adjustedTuitionRate;
                    student.OutstandingFees += adjustedTuitionAmount;
                    db.SaveChanges();

                }
                catch(Exception)
                {
                    returnCode = -300;
                }
            }

            return returnCode;
        }

        /// <summary>
        /// Implementation will set the grade for a student record.
        /// </summary>
        /// <param name="grade"> Represents the grade achieved by the student. </param>
        /// <param name="registrationId"> Represents the registration id of the student. </param>
        /// <param name="notes">Represents the notes. </param>
        /// <returns> The grade achieved by the student. </returns>
        public double? UpdateGrade(double grade, int registrationId, string notes)
        {
            double? result = null;

            Registration registration = db.Registrations.Where(x => x.RegistrationId == registrationId).SingleOrDefault();

            if (registration != null)
            {
                registration.Grade = grade;
                registration.Notes = notes;
                db.SaveChanges();

                double? calculatedGPA = CalculateGradePointAverage(registration.StudentId);

                result = calculatedGPA;
            }
            return result;
        }

        /// <summary>
        /// Calculate the Grade Point Average of a student by their total grade point values and total credit hours.
        /// </summary>
        /// <param name="studentId"> represents the student id of the student. </param>
        /// <returns> The calculated GPA of the student. </returns>
        private double? CalculateGradePointAverage(int studentId)
        {
            IQueryable<Registration> registrations = db.Registrations.Where(x => x.StudentId == studentId && x.Grade != null);

            double totalGradePointValue = 0;
            double totalCreditHours = 0;
            double? calculatedGradePointAverage = null;

            foreach (Registration record in registrations.ToList()) 
            {
                double grade = record.Grade.Value;

                CourseType courseType = BusinessRules.CourseTypeLookup(record.Course.CourseType);

                if (courseType != CourseType.AUDIT)
                {
                    double gradePoint = BusinessRules.GradeLookup(grade, courseType);
                    
                    double gradePointValue = gradePoint * record.Course.CreditHours;
                    totalGradePointValue += gradePointValue;
                    totalCreditHours += record.Course.CreditHours;
                }
            }

            if (totalCreditHours == 0)
            {
                calculatedGradePointAverage = null;
            }
            else
            {
                calculatedGradePointAverage = totalGradePointValue / totalCreditHours;

                Student student = db.Students.Where(x => x.StudentId == studentId).FirstOrDefault();
               
                if (student != null)
                {
                    student.GradePointAverage = calculatedGradePointAverage;
                    student.ChangeState();
                    db.SaveChanges();
                    //student.ChangeState();
                }
            }

            return calculatedGradePointAverage;
        }
    }
}
