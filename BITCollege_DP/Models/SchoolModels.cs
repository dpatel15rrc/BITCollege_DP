/*
 * Name: Dharmi Patel
 * Program: Business Information Technology
 * Course: ADEV-3008 Programming 3
 * Created: 2024-01-03
 * Updated:
 */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Utility;
using BITCollege_DP.Data;

namespace BITCollege_DP.Models
{
    /// <summary>
    /// Student Model.
    /// Represents the Students table in the database.
    /// </summary>
    public class Student
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int StudentId { get; set; }

        [Required]
        [ForeignKey("GradePointState")]
        public int GradePointStateId { get; set; }

        [ForeignKey("AcademicProgram")]
        public int? AcademicProgramId { get; set; }

        //[Required]
        //[Range(10000000, 99999999)]
        [Display(Name = "Student\nNumber")]
        public long StudentNumber { get; set; }

        [Required]
        [Display(Name = "First\nName")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last\nName")]
        public string LastName { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string City { get; set; }

        [Required(ErrorMessage = "Enter a Valid Canadian Province Code")]
        [RegularExpression("^(N[BLSTU]|[AMN]B|[BQ]C|ON|PE|SK|YT)")]  // Valid Canadian Province Regex
        public string Province { get; set; }

        [Required]
        [Display(Name = "Date")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime DateCreated { get; set; }

        [Display(Name = "Grade\nPoint\nAverage")]
        [Range(0.00, 4.5)]
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public double? GradePointAverage { get; set; }

        [Required]
        [Display(Name = "Fees")]
        [DisplayFormat(DataFormatString = "{0:c2}")]
        public double OutstandingFees { get; set; }

        public string Notes { get; set; }

        [Display(Name = "Name")]
        public string FullName
        {
            get
            {
                return String.Format("{0} {1}", FirstName, LastName);
            }
        }

        [Display(Name = "Address")]
        public string FullAddress
        {
            get
            {
                return String.Format("{0} {1} {2}", Address, City, Province);
            }
        }

        // navigation properties
        public virtual AcademicProgram AcademicProgram { get; set; }
        public virtual ICollection<Registration> Registration { get; set; }
        public virtual GradePointState GradePointState { get; set; }

        /// <summary>
        /// the data context object.
        /// </summary>
        private BITCollege_DPContext db = new BITCollege_DPContext();

        /// <summary>
        /// Initiate the process of ensuring that the student is always associated with the correct state.
        /// </summary>
        public void ChangeState()
        {
            GradePointState before = db.GradePointStates.Find(GradePointStateId);
            int after = 0;

            while (after != before.GradePointStateId)
            {
                before.StateChangeCheck(this);
                after = before.GradePointStateId;
                before = db.GradePointStates.Find(GradePointStateId);
            }
        }

        /// <summary>
        /// sets the StudentNumber property to next available value
        /// </summary>
        public void SetNextStudentNumber()
        {
            long? nextNumber = StoredProcedures.NextNumber("NextStudent");

            if (nextNumber.HasValue)
            {
                this.StudentNumber = nextNumber.Value;
            }
        }
    }

    /// <summary>
    /// AcademicProgram Model.
    /// Represents the AcademicPrograms table in the database.
    /// </summary>
    public class AcademicProgram
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int AcademicProgramId { get; set; }

        [Required]
        [Display(Name = "Program")]
        public string ProgramAcronym { get; set; }

        [Required]
        [Display(Name = "Program\nName")]
        public string Description { get; set; }

        // navigation properties
        public virtual ICollection<Student> Student { get; set; }
        public virtual ICollection<Course> Course { get; set; }

    }

    /// <summary>
    /// Registration Model.
    /// Represents the Registrations table in the database.
    /// </summary>
    public class Registration
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int RegistrationId { get; set; }

        [Required]
        [ForeignKey("Student")]
        public int StudentId { get; set; }

        [Required]
        [ForeignKey("Course")]
        public int CourseId { get; set; }

        //[Required]
        [Display(Name = "Registration\nNumber")]
        public long RegistrationNumber { get; set; }

        [Required]
        [Display(Name = "Date")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime RegistrationDate { get; set; }

        [Range(0, 1)]
        [DisplayFormat(NullDisplayText = "Ungraded")]
        public double? Grade { get; set; }

        public string Notes { get; set; }

        // navigation properties
        public virtual Student Student { get; set; }
        public virtual Course Course { get; set; }

        /// <summary>
        /// Set the next available value for RegistrationNumber property.
        /// </summary>
        public void SetNextRegistrationNumber()
        {
            // Call the NextNumber method with the discriminator
            long? nextNumber = StoredProcedures.NextNumber("NextRegistration");

            // Check if the nextNumber is not null before setting the RegistrationNumber
            if (nextNumber.HasValue)
            {
                this.RegistrationNumber = nextNumber.Value;
            }
        }
    }

    /// <summary>
    /// GradePointState Model.
    /// Represents the GradePointStates table in the database.
    /// </summary>
    public abstract class GradePointState
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Key]
        public int GradePointStateId { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        [Display(Name = "Lower\nLimit")]
        public double LowerLimit { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        [Display(Name = "Upper\nLimit")]
        public double UpperLimit { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        [Display(Name = "Tuition\nRate\nFactor")]
        public double TuitionRateFactor { get; set; }

        [Display(Name = "State")]
        public string Description
        {
            get
            {
                // call method from utility project.
                return BusinessRules.ParseString(GetType().Name, "State");
            }
        }

        // navigation properties
        public virtual ICollection<Student> Student { get; set; }

        /// <summary>
        /// the data context object.
        /// </summary>
        protected static BITCollege_DPContext db = new BITCollege_DPContext();
    
        /// <summary>
        /// Evaluates the state boundary values to determine the appropriate grade point state
        /// </summary>
        /// <param name="student"></param>
        public abstract void StateChangeCheck(Student student);

        /// <summary>
        /// Adjusts the tuition amount rate based on the Grade Point State of the student.
        /// </summary>
        /// <param name="student"></param>
        /// <returns>Adjusted tuition rate.</returns>
        public abstract double TuitionRateAdjustment(Student student);
    }

    /// <summary>
    /// Represents the Suspended State class
    /// </summary>
    public class SuspendedState : GradePointState
    {
        private static SuspendedState suspendedState;

        /// <summary>
        /// Private constructor for creating new internal instance.
        /// </summary>
        private SuspendedState()
        {
            LowerLimit = 0;
            UpperLimit = 1.00;
            TuitionRateFactor = 1.1;
        }

        /// <summary>
        /// Gets the Instance of the suspended state from database or creates a new one and adds it to the database.
        /// </summary>
        /// <returns> The single state instance of the probation state.</returns>
        public static SuspendedState GetInstance()
        {
            if (suspendedState == null)
            {
                suspendedState = db.SuspendedStates.SingleOrDefault();

                if (suspendedState == null)
                {
                    suspendedState = new SuspendedState();
                    db.SuspendedStates.Add(suspendedState);
                    db.SaveChanges();
                }
            }
            return suspendedState;
        }

        /// <summary>
        /// Overrides the method to check for state changes specific to suspended state.
        /// </summary>
        /// <param name="student"> a student object</param>
        public override void StateChangeCheck(Student student)
        {
            if (student.GradePointAverage > 1)
            {
                student.GradePointStateId = ProbationState.GetInstance().GradePointStateId;
            }
        }

        /// <summary>
        /// Modifies the tuition based on GPA, courses taken and the time
        /// Depends on the current state.
        /// </summary>
        /// <param name="student"> a student object</param>
        /// <returns>Adjusted tuition rate.</returns>
        public override double TuitionRateAdjustment(Student student)
        {
            double tuitionRate = TuitionRateFactor;

            if(student.GradePointAverage < 0.75 && student.GradePointAverage >= 0.5)
            {
                tuitionRate += 0.02;
            }
            else if(student.GradePointAverage < 0.5)
            {
                tuitionRate += 0.05;
            }
            return tuitionRate;
        }
    }

    /// <summary>
    /// Represents the Probation State class
    /// </summary>
    public class ProbationState : GradePointState
    {
        private static ProbationState probationState;

        /// <summary>
        /// Private constructor for creating internal instance.
        /// </summary>
        private ProbationState()
        {
            LowerLimit = 1.00;
            UpperLimit = 2.00;
            TuitionRateFactor = 1.075;
        }

        /// <summary>
        /// Gets the Instance of the probation state from database or creates a new one and adds it to the database.
        /// </summary>
        /// <returns>The single state instance of the Probation State.</returns>
        public static ProbationState GetInstance()
        {
            if( probationState == null)
            {
                probationState = db.ProbationStates.SingleOrDefault();

                if (probationState == null)
                {
                    probationState = new ProbationState();
                    db.GradePointStates.Add(probationState);
                    db.SaveChanges();
                }
            }
            return probationState;
        }

        /// <summary>
        /// Overrides the method to check for state changes specific to probation state.
        /// </summary>
        /// <param name="student"> a student object</param>
        public override void StateChangeCheck(Student student)
        {
            if (student.GradePointAverage < 1)
            {
                student.GradePointStateId = SuspendedState.GetInstance().GradePointStateId;
            }
            if (student.GradePointAverage > 2)
            {
                student.GradePointStateId = RegularState.GetInstance().GradePointStateId;
            }
        }

        /// <summary>
        /// Modifies the tuition based on GPA, courses taken and the time
        /// Depends on the current state. 
        /// </summary>
        /// <param name="student"> a student object</param>
        /// <returns>Adjusted tuition rate.</returns>
        public override double TuitionRateAdjustment(Student student)
        {
            double tuitionRate = TuitionRateFactor;

            IQueryable<Registration> studentCourses = db.Registrations.
                                                      Where(x => x.StudentId == student.StudentId && x.Grade != null);
            
            int courseCount = studentCourses.Count();

            if(courseCount >= 5)
            {
                tuitionRate -= .04;
            }
            return tuitionRate;
        }
    }

    /// <summary>
    /// Represents the Regular State class
    /// </summary>
    public class RegularState : GradePointState
    {
        private static RegularState regularState;

        /// <summary>
        /// Private constructor for creating new internal instance.
        /// </summary>
        private RegularState()
        {
            LowerLimit = 2.00;
            UpperLimit = 3.70;
            TuitionRateFactor = 1.0;
        }

        /// <summary>
        /// Gets the Instance of the regular state from database or creates a new one and adds it to the database.        
        /// </summary>
        /// <returns>The single state instance of the Regular State.</returns>
        public static RegularState GetInstance()
        {
            if (regularState == null)
            {
                regularState = db.RegularStates.SingleOrDefault();

                if (regularState == null)
                {
                    regularState = new RegularState();
                    db.GradePointStates.Add(regularState);
                    db.SaveChanges();
                }
            }
            return regularState;
        }

        /// <summary>
        /// Overrides the method to check for state changes specific to regular state.
        /// </summary>
        /// <param name="student"> a student object</param>
        public override void StateChangeCheck(Student student)
        {
            if (student.GradePointAverage < 2)
            {
                student.GradePointStateId = ProbationState.GetInstance().GradePointStateId;
            }
            if (student.GradePointAverage > 3.7)
            {
                student.GradePointStateId = HonoursState.GetInstance().GradePointStateId;
            }
        }

        /// <summary>
        /// Modifies the tuition based on GPA, courses taken and the time
        /// Depends on the current state.
        /// </summary>
        /// <param name="student"> a student object</param>
        /// <returns>Adjusted tuition rate.</returns>
        public override double TuitionRateAdjustment(Student student)
        {
            double tuitionRate = TuitionRateFactor;

            return tuitionRate;
        }
    }

    /// <summary>
    /// Represents the Honours State class
    /// </summary>
    public class HonoursState : GradePointState
    {
        private static HonoursState honoursState;

        /// <summary>
        /// Private constructor for creating new internal state.
        /// </summary>
        private HonoursState()
        {
            LowerLimit = 3.70;
            UpperLimit = 4.50;
            TuitionRateFactor = 0.9;
        }

        /// <summary>
        /// Gets the Instance of the honours state from database or creates a new one and adds it to the database.        
        /// </summary>
        /// <returns> The instance of the Honours State.</returns>
        public static HonoursState GetInstance()
        {
            if (honoursState == null)
            {
                honoursState = db.HonoursStates.SingleOrDefault();

                if (honoursState == null)
                {
                    honoursState = new HonoursState();
                    db.GradePointStates.Add(honoursState);
                    db.SaveChanges();
                }
            }
            return honoursState;
        }

        /// <summary>
        /// Overrides the method to check for state changes specific to honours state.
        /// </summary>
        /// <param name="student"> a student object</param>
        public override void StateChangeCheck(Student student)
        {
            if (student.GradePointAverage < 3.7)
            {
                student.GradePointStateId = RegularState.GetInstance().GradePointStateId;
            }
        }

        /// <summary>
        /// Modifies the tuition based on GPA, courses taken and the time
        /// Depends on the current state.
        /// </summary>
        /// <param name="student"> a student object</param>
        /// <returns> Adjusted tuition rate.</returns>
        public override double TuitionRateAdjustment(Student student)
        {
            double tuitionRate = TuitionRateFactor;

            IQueryable<Registration> studentCourses = db.Registrations.
                                                      Where(x => x.StudentId == student.StudentId && x.Grade != null);

            int courseCount = studentCourses.Count();

            if (courseCount >= 5)
            {
                tuitionRate -= 0.05;
            }
            if (student.GradePointAverage > 4.25)
            {
                tuitionRate -= 0.02;
            }
            return tuitionRate;
        }
    }

    /// <summary>
    /// Course Model.
    /// Represents the Courses table in the database.
    /// </summary>
    public abstract class Course
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Key]
        public int CourseId { get; set; }

        [ForeignKey("AcademicProgram")]
        public int? AcademicProgramId { get; set; }

        //[Required]
        [Display(Name = "Course\nNumber")]
        public string CourseNumber { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        [Display(Name = "Credit\nHours")]
        public double CreditHours { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:c2}", ApplyFormatInEditMode = true)]
        [Display(Name = "Tuition")]
        public double TuitionAmount { get; set; }

        [Display(Name = "Course\nType")]
        public string CourseType
        {
            get
            {
                return BusinessRules.ParseString(GetType().Name, "Course");
            }
        }

        public string Notes { get; set; }

        // navigation properties
        public virtual AcademicProgram AcademicProgram { get; set; }
        public virtual ICollection<Registration> Registration { get; set; }

        /// <summary>
        /// Sets the next available value for the CourseNumber property
        /// </summary>
        public abstract void SetNextCourseNumber();
        
    }

    /// <summary>
    /// Represents the Graded Course class
    /// </summary>
    public class GradedCourse : Course
    {
        [Required]
        [DisplayFormat(DataFormatString = "{0:p2}")]
        [Display(Name = "Assignments")]
        public double AssignmentWeight { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:p2}")]
        [Display(Name = "Exams")]
        public double ExamWeight { get; set; }

        /// <summary>
        /// Set the next available CourseNumber value to "G-" followed by value returned from the NextNumber method.
        /// </summary>
        public override void SetNextCourseNumber()
        {
            long? nextNumber = StoredProcedures.NextNumber("NextGradedCourse");

            if (nextNumber.HasValue)
            {
                this.CourseNumber = $"G-{nextNumber}";
            }
        }
    }

    /// <summary>
    /// Represents the Mastery Course class
    /// </summary>
    public class MasteryCourse : Course
    {
        [Required]
        [Display(Name = "Maximum\nAttempts")]
        public int MaximumAttempts { get; set; }

        /// <summary>
        /// Set the next available CourseNumber value to "M-" followed by value returned from the NextNumber method.
        /// </summary>
        public override void SetNextCourseNumber()
        {
            long? nextNumber = StoredProcedures.NextNumber("NextMasteryCourse");

            if (nextNumber.HasValue)
            {
                this.CourseNumber = $"M-{nextNumber}";
            }
        }
    }

    /// <summary>
    /// Represents the Audit Course class
    /// </summary>
    public class AuditCourse : Course
    {
        /// <summary>
        /// Set the next available CourseNumber value to "A-" followed by value returned from the NextNumber method.
        /// </summary>
        public override void SetNextCourseNumber()
        {
            long? nextNumber = StoredProcedures.NextNumber("NextAuditCourse");

            if (nextNumber.HasValue)
            {
                this.CourseNumber = $"A-{nextNumber}";
            }
        }
    }

    /// <summary>
    /// Next Unique Number Model.
    /// Represents the next available unique numbers.
    /// </summary>
    public abstract class NextUniqueNumber
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Key]
        public int NextUniqueNumberId { get; set; }

        [Required]
        public long NextAvailableNumber { get; set; }

        protected static BITCollege_DPContext db = new BITCollege_DPContext();
    }

    /// <summary>
    /// Represents the Next Student class.
    /// Retrieves the next available StudentNumber
    /// </summary>
    public class NextStudent : NextUniqueNumber
    {
        private static NextStudent nextStudent;

        /// <summary>
        /// Private constructor for creating new internal instance.
        /// </summary>
        private NextStudent() 
        {
            this.NextAvailableNumber = 20000000;
        }

        /// <summary>
        /// Gets the Instance of the next student from database or creates a new one and adds it to the database.
        /// </summary>
        /// <returns>Next Available Student number</returns>
        public static NextStudent GetInstance()
        {
            if(nextStudent == null)
            {
                nextStudent = db.NextStudents.SingleOrDefault();

                if(nextStudent == null)
                {
                    nextStudent = new NextStudent();
                    db.NextStudents.Add(nextStudent);
                    db.SaveChanges();
                }
            }
            return nextStudent;

        }
    }

    /// <summary>
    /// Represents the Next Registration class.
    /// Retrieves the next available RegistrationNumber
    /// </summary>
    public class NextRegistration : NextUniqueNumber
    {
        private static NextRegistration nextRegistration;

        /// <summary>
        /// Private constructor for creating new internal instance.
        /// </summary>
        private NextRegistration()
        {
            this.NextAvailableNumber = 700;
        }

        /// <summary>
        /// Gets the Instance of the next registration from database or creates a new one and adds it to the database.
        /// </summary>
        /// <returns>Next Available Registration number</returns>
        public static NextRegistration GetInstance()
        {
            if(nextRegistration == null)
            {
                nextRegistration = db.NextRegistrations.SingleOrDefault();

                if(nextRegistration == null)
                {
                    nextRegistration = new NextRegistration();
                    db.NextRegistrations.Add(nextRegistration);
                    db.SaveChanges();
                }
            }
            return nextRegistration;

        }
    }

    /// <summary>
    /// Represents the Next Graded Course class.
    /// Retrieves the next available CourseNumber
    /// </summary>
    public class NextGradedCourse : NextUniqueNumber
    {
        private static NextGradedCourse nextGradedCourse;

        /// <summary>
        /// Private constructor for creating new internal instance.
        /// </summary>
        private NextGradedCourse()
        {
            this.NextAvailableNumber = 200000;
        }

        /// <summary>
        /// Gets the Instance of the next GradedCourse from database or creates a new one and adds it to the database.
        /// </summary>
        /// <returns>Next Available Course number</returns>
        public static NextGradedCourse GetInstance()
        {
            if(nextGradedCourse == null)
            {
                nextGradedCourse = db.NextGradedCourses.SingleOrDefault();

                if(nextGradedCourse == null)
                {
                    nextGradedCourse = new NextGradedCourse();
                    db.NextGradedCourses.Add(nextGradedCourse);
                    db.SaveChanges();
                }
            }
            return nextGradedCourse;

        }
    }

    /// <summary>
    /// Represents the Next Audit Course class.
    /// Retrieves the next available CourseNumber
    /// </summary>
    public class NextAuditCourse : NextUniqueNumber
    {
        private static NextAuditCourse nextAuditCourse;

        /// <summary>
        /// Private constructor for creating new internal instance.
        /// </summary>
        private NextAuditCourse()
        {
            this.NextAvailableNumber = 2000;
        }

        /// <summary>
        /// Gets the Instance of the next AuditCourse from database or creates a new one and adds it to the database.
        /// </summary>
        /// <returns>Next Available Course number</returns>
        public static NextAuditCourse GetInstance()
        {
            if(nextAuditCourse == null)
            {
                nextAuditCourse = db.NextAuditCourses.SingleOrDefault();

                if(nextAuditCourse == null)
                {
                    nextAuditCourse = new NextAuditCourse();
                    db.NextAuditCourses.Add(nextAuditCourse);
                    db.SaveChanges();
                }
            }
            return nextAuditCourse;

        }
    }

    /// <summary>
    /// Represents the Next Mastery Course class.
    /// Retrieves the next available CourseNumber
    /// </summary>
    public class NextMasteryCourse : NextUniqueNumber
    {
        private static NextMasteryCourse nextMasteryCourse;

        /// <summary>
        /// Private constructor for creating new internal instance.
        /// </summary>
        private NextMasteryCourse()
        {
            this.NextAvailableNumber = 20000;
        }

        /// <summary>
        /// Gets the Instance of the next MasteryCourse from database or creates a new one and adds it to the database.
        /// </summary>
        /// <returns>Next Available Course number</returns>
        public static NextMasteryCourse GetInstance()
        {
            if(nextMasteryCourse == null)
            {
                nextMasteryCourse = db.NextMasteryCourses.SingleOrDefault();

                if(nextMasteryCourse == null)
                {
                    nextMasteryCourse = new NextMasteryCourse();
                    db.NextMasteryCourses.Add(nextMasteryCourse);
                    db.SaveChanges();
                }
            }
            return nextMasteryCourse;

        }
    }

}