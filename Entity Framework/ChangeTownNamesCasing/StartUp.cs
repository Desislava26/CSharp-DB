using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChangeTownNamesCasing
{
    internal class StartUp
    {
        static void Main()
        {
            string stringConnection = "Server=.;Integrated Security=true;encrypt=false;Database=MinionsDB";

            SqlConnection connection = new SqlConnection(stringConnection);

            var townList = new List<string>();
            string name = Console.ReadLine();
            CheckingTown(connection, name,townList);

            
        }

        private static void CheckingTown(SqlConnection connection, string name,List<string>townList)
        {
            string query = @"Select 
            t.Name
            FROM Towns as t
            JOIN Countries AS c ON c.Id = t.CountryCode
            WHERE c.Name = @country"; 
            using (connection)
            {
                connection.Open();
                using (var command = new SqlCommand(query,connection))
                {
                    command.Parameters.AddWithValue("@country", name);
                    using (var reader = command.ExecuteReader())
                    {
                            if (!reader.HasRows)
                            {
                            Console.WriteLine("No town names were affected.");
                            return;
                            }
                            var listingUpper = new List<string>();
                            while (reader.Read())
                            {
                            var town = (string)reader["Name"];
                            townList.Add(town);
                            listingUpper.Add(town.ToUpper());
                            

                            }
                        Console.WriteLine($"{listingUpper.Count()} town names were affected. ");
                        Console.WriteLine($"[{string.Join(", ", listingUpper)}]");
                        
                    }

                }
                UpdatingNames(townList);

               
            }
            
        }

        private static void UpdatingNames(List<string>townList)
        {
            string stringConnection = "Server=.;Integrated Security=true;encrypt=false;Database=MinionsDB";

            SqlConnection connection = new SqlConnection(stringConnection);
            using (connection)
            {
                connection.Open();
                var queryTwo = "UPDATE Towns SET Name = @newName WHERE Name = @country";
                using (var commandTwo = new SqlCommand(queryTwo, connection))
                {
                    for (int i = 0; i < townList.Count; i++)
                    {
                        commandTwo.Parameters.AddWithValue("@newName", townList[i].ToUpper());
                        commandTwo.Parameters.AddWithValue("@country", townList[i]);
                        commandTwo.ExecuteNonQuery();
                    }
                
                }
            }
        }

        

    }
}
