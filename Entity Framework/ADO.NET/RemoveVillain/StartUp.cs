using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Tokens;

namespace RemoveVillain
{
    internal class StartUp
    {
        static void Main()
        {
            string stringConnection = @"Server=.;Integrated Security=true;encrypt=false;Database=MinionsDB";
            SqlConnection connection = new SqlConnection(stringConnection);
            connection.Open();

            int id = int.Parse(Console.ReadLine());
            string nameVillian = "";
            if(CheckId(id) == 0)
            {
                Console.WriteLine("No such villain was found.");
                return;
            }

            using (connection)
            {
                string query = @"SELECT 
                                v.Name,
                                COUNT(m.Name) AS Minions
                                FROM Villains AS v
                                JOIN MinionsVillains AS mv
                                ON mv.VillainId = v.Id
                                JOIN Minions AS m
                                ON m.Id = mv.MinionId
                                WHERE v.Id = 1
                                GROUP BY v.Name";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string name = (string)reader["Name"];
                            
                            Console.WriteLine($"{reader["Name"]} was deleted.");
                            Console.WriteLine($"{reader["Minions"]} minions were released.");
                            DeleteVillain(name);
                        }
                    }


                }
            }
        }

        private static int CheckId(int id)
        {
            string stringConnection = @"Server=.;Integrated Security=true;encrypt=false;Database=MinionsDB";
            SqlConnection connection = new SqlConnection(stringConnection);
            connection.Open();
            int c = 0;
            using (connection)
            {
                string query = @"SELECT * FROM Villains WHERE Id = @numId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@numId", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (!reader.HasRows)
                        {
                            return 0;
                        }
                    }


                }
            }
            return id;


        }

        private static void DeleteVillain(string name)
        {
            string stringConnection = @"Server=.;Integrated Security=true;encrypt=false;Database=MinionsDB";
            SqlConnection connection = new SqlConnection(stringConnection);
            connection.Open();
            int c = 0;
            using (connection)
            {
                string query = @"DELETE FROM Villians WHERE Id = @villainId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@villainId", command);
                    command.ExecuteNonQuery();


                }
            }

        }

    }
}
