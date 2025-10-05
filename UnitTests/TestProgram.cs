using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using BITCollege_DP;
using BITCollege_DP.Data;
using BITCollege_DP.Models;
using Utility;

namespace UnitTests
{
    class TestProgram
    {
        private static BITCollege_DPContext db = new BITCollege_DPContext();
 
        static void Main(string[] args)
        {
            Console.WriteLine("Testing Suspended State:");
            Suspended_State_GPA44();
            Suspended_State_GPA60();
            Suspended_State_GPA88();

            Console.WriteLine("\nTesting Probation State:");
            Probation_State_3GPA115();
            Probation_State_7GPA115();

            Console.WriteLine("\nTesting Regular State:");
            Regular_State_GPA250();

            Console.WriteLine("\nTesting Honours State:");
            Honours_State_3GPA390();
            Honours_State_4GPA427();
            Honours_State_7GPA440();
            Honours_State_7GPA410();

            Console.ReadKey();
        }

        static void Suspended_State_GPA44()
        {
            Console.WriteLine("TEST CASE 1: GPA = 0.44");

            //set up test student
            Student student = db.Students.Find(4);
            student.GradePointAverage = 0.44;
            student.GradePointStateId = 1;
            db.SaveChanges();

            //get an instance of the student's state
            GradePointState state = db.GradePointStates.Find(student.GradePointStateId);

            //call the tuition rate adjustment
            double tuitionRate = 1000 * state.TuitionRateAdjustment(student);

            Console.WriteLine("Expected: 1150");
            Console.WriteLine("Actual: " + tuitionRate);
        }

        static void Suspended_State_GPA60()
        {
            Console.WriteLine("TEST CASE 2: GPA = 0.60");

            //set up test student
            Student student = db.Students.Find(4);
            student.GradePointAverage = 0.60;
            student.GradePointStateId = 1;
            db.SaveChanges();

            //get an instance of the student's state
            GradePointState state = db.GradePointStates.Find(student.GradePointStateId);

            //call the tuition rate adjustment
            double tuitionRate = 1000 * state.TuitionRateAdjustment(student);

            Console.WriteLine("Expected: 1120");
            Console.WriteLine("Actual: " + tuitionRate);
        }
        
        static void Suspended_State_GPA88()
        {
            Console.WriteLine("TEST CASE 3: GPA = 0.88");

            //set up test student
            Student student = db.Students.Find(4);
            student.GradePointAverage = 0.88;
            student.GradePointStateId = 1;
            db.SaveChanges();

            //get an instance of the student's state
            GradePointState state = db.GradePointStates.Find(student.GradePointStateId);

            //call the tuition rate adjustment
            double tuitionRate = 1000 * state.TuitionRateAdjustment(student);

            Console.WriteLine("Expected: 1100");
            Console.WriteLine("Actual: " + tuitionRate);
        }

        static void Probation_State_3GPA115()
        {
            Console.WriteLine("TEST CASE 1: GPA = 1.15; COURSES: 3");

            //set up test student
            Student student = db.Students.Find(1);
            student.GradePointAverage = 1.15;
            student.GradePointStateId = 2;
            db.SaveChanges();

            //get an instance of the student's state
            GradePointState state = db.GradePointStates.Find(student.GradePointStateId);

            //call the tuition rate adjustment
            double tuitionRate = 1000 * state.TuitionRateAdjustment(student);

            Console.WriteLine("Expected: 1075");
            Console.WriteLine("Actual: " + tuitionRate);
        }

        static void Probation_State_7GPA115()
        {
            Console.WriteLine("TEST CASE 2: GPA = 1.15; COURSES: 7");

            //set up test student
            Student student = db.Students.Find(4);
            student.GradePointAverage = 1.15;
            student.GradePointStateId = 2;
            db.SaveChanges();

            //get an instance of the student's state
            GradePointState state = db.GradePointStates.Find(student.GradePointStateId);

            //call the tuition rate adjustment
            double tuitionRate = 1000 * state.TuitionRateAdjustment(student);

            Console.WriteLine("Expected: 1035");
            Console.WriteLine("Actual: " + tuitionRate);
        }

        static void Regular_State_GPA250()
        {
            Console.WriteLine("TEST CASE 1: GPA = 2.50");

            //set up test student
            Student student = db.Students.Find(4);
            student.GradePointAverage = 2.50;
            student.GradePointStateId = 3;
            db.SaveChanges();

            //get an instance of the student's state
            GradePointState state = db.GradePointStates.Find(student.GradePointStateId);

            //call the tuition rate adjustment
            double tuitionRate = 1000 * state.TuitionRateAdjustment(student);

            Console.WriteLine("Expected: 1000");
            Console.WriteLine("Actual: " + tuitionRate);
        }

        static void Honours_State_3GPA390()
        {
            Console.WriteLine("TEST CASE 1: GPA = 3.90; COURSES: 3");

            //set up test student
            Student student = db.Students.Find(1);
            student.GradePointAverage = 3.90;
            student.GradePointStateId = 4;
            db.SaveChanges();

            //get an instance of the student's state
            GradePointState state = db.GradePointStates.Find(student.GradePointStateId);

            //call the tuition rate adjustment
            double tuitionRate = 1000 * state.TuitionRateAdjustment(student);

            Console.WriteLine("Expected: 900");
            Console.WriteLine("Actual: " + tuitionRate);
        }

        static void Honours_State_4GPA427()
        {
            Console.WriteLine("TEST CASE 2: GPA = 4.27; COURSES: 4");

            //set up test student
            Student student = db.Students.Find(1);
            student.GradePointAverage = 4.27;
            student.GradePointStateId = 4;
            db.SaveChanges();

            //get an instance of the student's state
            GradePointState state = db.GradePointStates.Find(student.GradePointStateId);

            //call the tuition rate adjustment
            double tuitionRate = 1000 * state.TuitionRateAdjustment(student);

            Console.WriteLine("Expected: 880");
            Console.WriteLine("Actual: " + tuitionRate);
        }

        static void Honours_State_7GPA440()
        {
            Console.WriteLine("TEST CASE 3: GPA = 4.40; COURSES: 7");

            //set up test student
            Student student = db.Students.Find(4);
            student.GradePointAverage = 4.40;
            student.GradePointStateId = 4;
            db.SaveChanges();

            //get an instance of the student's state
            GradePointState state = db.GradePointStates.Find(student.GradePointStateId);

            //call the tuition rate adjustment
            double tuitionRate = 1000 * state.TuitionRateAdjustment(student);

            Console.WriteLine("Expected: 830");
            Console.WriteLine("Actual: " + tuitionRate);
        }

        static void Honours_State_7GPA410()
        {
            Console.WriteLine("TEST CASE 4: GPA = 4.10; COURSES: 7");

            //set up test student
            Student student = db.Students.Find(4);
            student.GradePointAverage = 4.10;
            student.GradePointStateId = 4;
            db.SaveChanges();

            //get an instance of the student's state
            GradePointState state = db.GradePointStates.Find(student.GradePointStateId);

            //call the tuition rate adjustment
            double tuitionRate = 1000 * state.TuitionRateAdjustment(student);

            Console.WriteLine("Expected: 850");
            Console.WriteLine("Actual: " + tuitionRate);
        }


    }
}
