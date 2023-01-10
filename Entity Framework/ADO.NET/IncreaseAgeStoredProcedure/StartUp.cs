using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncreaseAgeStoredProcedure
{
    internal class StartUp
    {
        static void Main()
        {
            string stringConnection = "Server=.;Integrated Security=true;encrypt=false;Database=MinionsDB";

            SqlConnection connection = new SqlConnection(stringConnection);
            connection.Open();

            int[] ids = Console.ReadLine().Split().Select(int.Parse).ToArray();
            using (connection)
            {
                string query = "EXEC usp_GetOlder @idMinion";
                using (var command = new SqlCommand(query, connection))
                {
                    for (int i = 0; i < ids.Length; i++)
                    {
                        command.Parameters.AddWithValue("@idMinion", ids[i]);
                        command.ExecuteNonQuery();
                    }
                    
                }
                using (var selectCommand = new SqlCommand("SELECT * FROM Minions", connection))
                {
                    SqlDataReader reader = selectCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        Console.WriteLine($"{reader["Name"]} - {reader["Age"]} old years old");
                    }
                }

            }
        }
    }
}
