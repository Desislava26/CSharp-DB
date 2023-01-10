using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace IncreaseMinionAge
{
    internal class StartUp
    {
        static void Main()
        {
            string stringConnection = "Server=.;Integrated Security=true;encrypt=false;Database=MinionsDB";

            SqlConnection connection = new SqlConnection(stringConnection);
            connection.Open();

            string[] names = Console.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries);
           
            using (connection)
            {
                string query = @"SELECT
                               Id,
                               Name,
                               Age
                               From Minions";
                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string name = reader["Name"].ToString();
                            int age = (int)reader["Age"];
                            UpdateNameAndAge(name,age);
                            
                        }
                    }
                }
            }
        }

        private static void UpdateNameAndAge(string name, int age)
        {
            string stringConnection = "Server=.;Integrated Security=true;encrypt=false;Database=MinionsDB";

            SqlConnection connection = new SqlConnection(stringConnection);
            connection.Open();
            string value = "";

            using (connection)
            {
                string query = @"UPDATE Minions SET Name = @value WHERE Name = @name";
                using (var command = new SqlCommand(query, connection))
                {
                    if (Char.IsUpper(name[0]))
                    {
                        value = name.ToLower();
                    }
                    else
                    {
                        TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;

                        value = textInfo.ToTitleCase(name);
                    }
                   

                    command.Parameters.AddWithValue("@value", value);
                    command.ExecuteNonQuery();
                }

                string queryAge = @"UPDATE Minions SET Age = Age+1 WHERE Name = @name";
                using (var commandAge = new SqlCommand(queryAge, connection))
                {
                    commandAge.Parameters.AddWithValue("@name", value);
                    commandAge.ExecuteNonQuery();
                }

            }
        }
    }
}
