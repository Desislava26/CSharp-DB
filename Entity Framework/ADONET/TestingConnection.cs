using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace Entity_Framework_Introduction
{
     class TestingConnection
    {
        static void Main()
        {
            string stringConnection = "Server=.;Integrated Security=true;encrypt=false;Database=Softuni";
            var connection = new SqlConnection(stringConnection);
            using (connection)
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM Employees";
                SqlCommand sqlCommand = new SqlCommand(query, connection);
                sqlCommand.ExecuteNonQuery();
                Console.WriteLine(sqlCommand.ExecuteScalar);
            }
                
        }
    }
}
