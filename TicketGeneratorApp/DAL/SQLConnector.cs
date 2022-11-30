using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketGeneratorApp.Models;

namespace TicketGeneratorApp.DAL
{
    public static class SQLConnector
    {
        static string _selectProcName= ConfigurationManager.AppSettings["SelectProcName"];
        public static List<Employee> GetEmployees()
        {
            try
            {
                List<Employee> dataList = new List<Employee>();
                string conString = ConfigurationManager.ConnectionStrings["connectSQL"].ConnectionString;
                using (var _sqlCon = new SqlConnection(conString))
                {
                    _sqlCon.Open();
                    SqlCommand sql_cmnd = new SqlCommand(_selectProcName, _sqlCon);
                    sql_cmnd.CommandType = CommandType.StoredProcedure;

                    //sql_cmnd.Parameters.AddWithValue("@Status", SqlDbType.VarChar).Value = selectStatus;

                    SqlDataReader reader = sql_cmnd.ExecuteReader();
                    Employee data = null;

                    while (reader.Read())
                    {
                        data= new Employee() {
                            ID = Convert.ToInt32(reader["ID"]),
                            EmpCode = Convert.ToString(reader["Employee_Code"]),
                            FirstName = Convert.ToString(reader["Full_Name"]),
                            //LastName = Convert.ToString(reader["LastName"]),
                            NoOfAttendiees = Convert.ToString(reader["Total_Family_Members_Count"]),
                        };
                       
                        dataList.Add(data);
                    }
                    _sqlCon.Close();
                }
                
                return dataList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
