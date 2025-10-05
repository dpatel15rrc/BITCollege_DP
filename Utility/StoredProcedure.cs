using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    /// <summary>
    /// Executes the SQL Server stored procedure.
    /// </summary>
    public static class StoredProcedures
    {
        /// <summary>
        /// Retrieves the next available number
        /// </summary>
        /// <param name="discriminator"> table to increment the ID</param>
        /// <returns> the next available number</returns>
        public static long? NextNumber(string discriminator)
        {
            try
            {
                long? returnValue = 0;
                SqlConnection connection = new SqlConnection("Data Source=ASUS\\EARTH; " +
                "Initial Catalog=BITCollege_DPContext;Integrated Security=True");
                SqlCommand storedProcedure = new SqlCommand("next_number", connection);
                storedProcedure.CommandType = CommandType.StoredProcedure;
                storedProcedure.Parameters.AddWithValue("@Discriminator", discriminator);
                SqlParameter outputParameter = new SqlParameter("@NewVal", SqlDbType.BigInt)
                {
                    Direction = ParameterDirection.Output
                };
                storedProcedure.Parameters.Add(outputParameter);
                connection.Open();
                storedProcedure.ExecuteNonQuery();
                connection.Close();
                returnValue = (long?)outputParameter.Value;
                return returnValue;
            }
            catch (Exception ex)
            {
                // Return null in case of any exception
                return null;
            }
        }
    }
}