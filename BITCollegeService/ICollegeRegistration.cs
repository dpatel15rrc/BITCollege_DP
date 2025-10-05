using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace BITCollegeService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ICollegeRegistration" in both code and config file together.
    [ServiceContract]
    public interface ICollegeRegistration
    {
        [OperationContract]
        void DoWork();

        /// <summary>
        /// Implementation will retrieve the Registration record from database, remove it and persist the change.
        /// </summary>
        /// <param name="registrationId">Represents the registration  ID of the course. </param>
        /// <returns></returns>
        [OperationContract]
        bool DropCourse(int registrationId);

        /// <summary>
        /// Implementation will register a student for a course.
        /// In the event that the course registration fails, the return code will indicate the reason
        /// </summary>
        /// <param name="studentId"> Represents the student ID. </param>
        /// <param name="courseId"> Represents the course ID. </param>
        /// <param name="notes"> Represents the notes. </param>
        /// <returns></returns>
        [OperationContract]
        int RegisterCourse(int studentId, int courseId, String notes);

        /// <summary>
        /// Implementation will set the grade for a student record.
        /// </summary>
        /// <param name="grade"> Represents the grade achieved by the student. </param>
        /// <param name="registrationId"> Represents the registration id of the student. </param>
        /// <param name="notes">Represents the notes. </param>
        /// <returns></returns>
        [OperationContract]
        double? UpdateGrade(double grade, int registrationId, String notes);
    }
}
