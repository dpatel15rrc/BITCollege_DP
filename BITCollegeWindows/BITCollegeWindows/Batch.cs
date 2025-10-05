using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using BITCollege_DP.Data;
using BITCollege_DP.Models;
using System.Runtime.Remoting.Contexts;
using Utility;
using System.Data.Entity.Infrastructure;
using BITCollegeWindows.CollegeRegistrationService;

namespace BITCollegeWindows
{
    /// <summary>
    /// Batch:  This class provides functionality that will validate
    /// and process incoming xml files.
    /// </summary>
    public class Batch
    {
        BITCollege_DPContext db = new BITCollege_DPContext();
        CollegeRegistrationService.CollegeRegistrationClient service = new CollegeRegistrationService.CollegeRegistrationClient();

        private String inputFileName;
        private String logFileName;
        private String logData;

        /// <summary>
        /// This method will process all detail errors found within the current file being processed.
        /// </summary>
        /// <param name="beforeQuery">Represents the records that existed before the round of validation.</param>
        /// <param name="afterQuery">Represents the records that remained following the round of validation.</param>
        /// <param name="message">Represents the error message that is to be written to the log file based on the record failing the round of validation.</param>
        private void ProcessErrors(IEnumerable<XElement> beforeQuery, IEnumerable<XElement> afterQuery, String message)
        {
            IEnumerable<XElement> errors = beforeQuery.Except(afterQuery);

            foreach (XElement error in errors)
            {
                logData +=  "\r\n----------ERROR----------";
                logData += $"\r\nFile: {inputFileName}";
                logData += $"\r\nProgram: {error.Element("program")}";
                logData += $"\r\nStudent Number: {error.Element("student_no")}";
                logData += $"\r\nCourse Number: {error.Element("course_no")}";
                logData += $"\r\nRegistration Number: {error.Element("registration_no")}";
                logData += $"\r\nType: {error.Element("type")}";
                logData += $"\r\nGrade: {error.Element("grade")}";
                logData += $"\r\nNotes: {error.Element("notes")}";
                logData += $"\r\nNodes: {error.Nodes().Count()}";
                logData += $"\r\nMessage: {message}";
                logData +=  "\r\n--------------------------\n";
            }
        }

        /// <summary>
        /// This method is used to verify the attributes of the xml file’s root element. 
        /// If any of the attributes produce an error, the file is NOT to be processed.
        /// </summary>
        private void ProcessHeader()
        {
            XDocument xDoc = XDocument.Load(inputFileName);
            XElement root = xDoc.Element("student_update");

            //Attribute check.
            if (root.Attributes().Count() != 3)
            {
                throw new Exception($"Incorrect number of root attributed for file {inputFileName}\n");
            }

            //date check.
            if (!DateTime.Parse(root.Attribute("date").Value).Equals(DateTime.Today))
            {
                throw new Exception($"Incorrect date for file {inputFileName}\n");
            }

            //program acronym check.
            string programAcr = root.Attribute("program").Value;
            bool programExists = db.AcademicPrograms.Any(x => x.ProgramAcronym == programAcr);
            if (!programExists)
            {
                throw new Exception($"Invalid program acronym {programAcr} for file {inputFileName}\n");
            }

            //checksum check.
            int expectedChecksum = xDoc.Descendants("student_no").Sum(x => int.Parse(x.Value));
            int actualChecksum = int.Parse(root.Attribute("checksum").Value);
            if (expectedChecksum != actualChecksum)
            {
                throw new Exception($"Incorrect checksum value {expectedChecksum} for file {inputFileName}\n");
            }
        }

        /// <summary>
        /// This method is used to verify the contents of the detail records in the input file. 
        /// If any of the records produce an error, that record will be skipped, but the file processing will continue.
        /// </summary>
        private void ProcessDetails()
        {
            XDocument xDoc = XDocument.Load(inputFileName);

            //Each transaction element in the xml file must have 7 child elements.
            IEnumerable<XElement> one = xDoc.Descendants().Elements("transaction");
            IEnumerable<XElement> two = one.Where(x => x.Elements().Nodes().Count() == 7);
            ProcessErrors(one, two, "Node count is not 7.");

            //The program element must match the program attribute of the root element.
            IEnumerable<XElement> three = two.Where(x => x.Element("program").Value == xDoc.Root.Attribute("program").Value);
            ProcessErrors(two, three, "Transaction program does not match root program.");

            //The type element within each transaction element must be numeric.
            IEnumerable<XElement> four = three.Where(x => Numeric.IsNumeric(x.Element("type").Value, System.Globalization.NumberStyles.Any));
            ProcessErrors(three, four, "Transaction type element is not numeric.");

            //The grade element within each transaction element must be numeric or have the value of ‘*’.
            IEnumerable<XElement> five = four.
                Where(x => Numeric.IsNumeric(x.Element("grade").Value, System.Globalization.NumberStyles.Any) || x.Element("grade").Value == "*");
            ProcessErrors(four, five, "Transaction grade element is not numeric or '*'.");

            //The type element within each transaction element must have a value of 1 or 2
            IEnumerable<XElement> six = five.Where(x => x.Element("type").Value == "1" || x.Element("type").Value == "2");
            ProcessErrors(five, six, "Transaction type element is not 1 or 2.");

            // type=1 && grade=* || type=2 && grade>=1 && grade<=100
            IEnumerable<XElement> seven = six.
                Where(x => (x.Element("type").Value == "1" && x.Element("grade").Value == "*") ||
                           (x.Element("type").Value == "2" && double.Parse(x.Element("grade").Value) >= 1 && double.Parse(x.Element("grade").Value) <= 100));
            ProcessErrors(six, seven, "Invalid grade value for transaction type.");

            //The student_no element within each transaction element must exist in the database.
            IEnumerable<long> allStudentNumbers = db.Students.Select(x => x.StudentNumber).ToList();
            IEnumerable<XElement> eight = seven.Where(x => allStudentNumbers.Contains(long.Parse(x.Element("student_no").Value)));
            ProcessErrors(seven, eight, "Invalid student number.");

            //courseNo type=1 && exist in db || type=2 && courseNo=*
            IEnumerable<string> allCourseNumbers = db.Courses.Select(x => x.CourseNumber).ToList();
            IEnumerable<XElement> nine = eight.
                Where(x => (x.Element("type").Value == "1" && allCourseNumbers.Contains(x.Element("course_no").Value)) || 
                           (x.Element("type").Value == "2" && x.Element("course_no").Value == "*"));
            ProcessErrors(eight, nine, "Invalid course number.");

            // type=1 && regNo=* || type=2 && exist in db.
            IEnumerable<long> allRegistrationNumbers = db.Registrations.Select(x => x.RegistrationNumber).ToList();
            IEnumerable<XElement> ten = nine.
                Where(x => (x.Element("type").Value == "1" && x.Element("registration_no").Value == "*") || 
                           (x.Element("type").Value == "2" && allRegistrationNumbers.Contains(long.Parse(x.Element("registration_no").Value))));
            ProcessErrors(nine, ten, "Invalid registration number.");

            ProcessTransactions(ten);
        }

        /// <summary>
        /// This method is used to process all valid transaction records.
        /// </summary>
        /// <param name="transactionRecords">Represents the list of error-free records from the input file.</param>
        private void ProcessTransactions(IEnumerable<XElement> transactionRecords)
        {
            foreach (XElement transaction in transactionRecords)
            {
                string type = transaction.Element("type").Value;

                if (type == "1")
                {
                    int studentNumber = int.Parse(transaction.Element("student_no").Value);
                    string courseNumber = transaction.Element("course_no").Value;
                    string notes = transaction.Element("notes").Value;

                    int studentId = db.Students.Where(x => x.StudentNumber == studentNumber).Select(x => x.StudentId).FirstOrDefault();
                    int courseId = db.Courses.Where(x => x.CourseNumber == courseNumber).Select(x => x.CourseId).FirstOrDefault();

                    try
                    {
                        int registrationResult = service.RegisterCourse(studentId, courseId, notes);

                        if (registrationResult == 0)
                        {
                            logData += $"Student: {studentNumber} has successfully registered for Course: {courseNumber}\n";
                        }
                        else
                        {
                            logData += $"REGISTRATION ERROR: {BusinessRules.RegisterError(registrationResult)}\n";
                        }
                    }
                    catch (Exception ex) 
                    {
                        logData += $"REGISTRATION EXCEPTION: {ex.Message}\n";
                    }
                }
                else if (type == "2")
                {
                    double grade = double.Parse(transaction.Element("grade").Value);
                    int registrationNumber = int.Parse(transaction.Element("registration_no").Value);
                    string notes = transaction.Element("notes").Value;

                    double parsedGrade = Convert.ToDouble(grade) / 100;
                    int registrationId = db.Registrations.Where(x => x.RegistrationNumber == registrationNumber).Select(x => x.RegistrationId).FirstOrDefault();

                    if (parsedGrade >= 0 && parsedGrade <= 1)
                    {
                        try
                        {
                            double? gradeAchieved = service.UpdateGrade(parsedGrade, registrationId, notes);

                            if (gradeAchieved != null)
                            {
                                logData += $"A grade of: {gradeAchieved} has been successfully applied to registration: {registrationId}\n";
                            }
                            else
                            {
                                logData += $"GRADE UPDATE ERROR: Grade update unsuccessful for Registration Number: {registrationNumber}\n";
                            }
                        }
                        catch (Exception ex)
                        {
                            logData += $"GRADE UPDATE EXCEPTION: {ex.Message}\n";
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Writes the Log data to the log file.
        /// </summary>
        /// <returns> captured log data for a particular input file.</returns>
        public String WriteLogData()
        {
            using (StreamWriter writer = new StreamWriter(logFileName, true))
            {
                writer.WriteLine(logData);
                writer.Close();
            }

            string capturedLogData = logData;
            logData = "";
            logFileName = "";
            
            return capturedLogData;
        }

        /// <summary>
        /// This method will initiate the batch process by determining the appropriate filename and 
        /// then proceeding with the header and detail processing.
        /// </summary>
        /// <param name="programAcronym">Represents the program acronym from the file to be processed.</param>
        public void ProcessTransmission(String programAcronym)
        {
            inputFileName = DateTime.Now.Year + "-" + DateTime.Now.DayOfYear + "-" + programAcronym + ".xml";
            logFileName = "LOG " + inputFileName.Replace("xml", "txt");

            try
            {
                if(File.Exists(inputFileName))
                {
                    try
                    {
                        ProcessHeader();
                        ProcessDetails();
                    }
                    catch (Exception ex)
                    {
                        logData += $"Exception occurred while processing header or details: {ex.Message}\n";
                    }
                }
                else
                {
                    logData += $"File does not exist in the system: {inputFileName}\n";
                }
            }
            catch (Exception ex)
            {
                logData += $"Exception occurred: {ex.Message}\n";
            }
        }
    }
}
