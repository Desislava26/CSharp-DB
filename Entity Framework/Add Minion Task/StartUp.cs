using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Tokens;

namespace Add_Minion_Task
{
    public class StartUp
    {
        static void Main()
        {
            string stringConnection = "Server=.;Integrated Security=true;encrypt=false;Database=MinionsDB";
          
            SqlConnection connection = new SqlConnection(stringConnection);
            connection.Open();
            string[] infoMinion = Console.ReadLine().Split(' ');

            string nameMini = infoMinion[1];
            int ageMini = int.Parse(infoMinion[2]);
            string townMini = (infoMinion[3]);

            string[] infoVillain = Console.ReadLine().Split(' ');
            string villName = infoVillain[1];
            
            using (connection)
            {
                int townId = FindTown(connection, townMini);
                int villainId = FindVillain(connection, villName);
                if (townId == 0)
                {
                    InsertTown(connection, townMini);
                    townId = FindTown(connection, townMini);
                }

                if (villainId == 0)
                {
                    InsertVillain(connection, villName);
                    villainId = FindVillain(connection, villName);
                }

                InsertMinion(connection, townId, nameMini, ageMini);
                int minionId = FindMinionId(connection, nameMini);
                CreateConnectionBetweenVillainAndMinion(connection, minionId, villainId, nameMini, villName);
            }
        }

        private static int FindMinionId(SqlConnection connection,string nameMini)
        {
            string query = @"SELECT * FROM Minions WHERE Name = @nameMini";

            using (connection)
            {
               
                using (var command = new SqlCommand(query, connection))
                {
                        command.Parameters.AddWithValue("@newMinionName", nameMini);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        var minionId = 0;
                        while (reader.Read())
                            minionId = (int)reader["Id"];
                        return minionId;
                    }
                }
            }

        }

        private static void InsertMinion(SqlConnection connection, int townId, string nameMini, int ageMini)
        {
            using (connection)
            {
               
                string query = @"INSERT INTO Minions (Name, Age, TownId) VALUES (@newMinionName, @newMinionAge, @townId)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@newMinionName", nameMini);
                    command.Parameters.AddWithValue("@newMinionAge", ageMini);
                    command.Parameters.AddWithValue("@townId", townId);

                    command.ExecuteNonQuery();
                }
            }

        }

        private static void InsertVillain(SqlConnection connection, string villName)
        {
            using (connection)
            {
              
                string query = @"INSERT INTO Villains (Name, EvilnessFactorId) VALUES (@villainName, 4)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@villainName", villName);

                    command.ExecuteNonQuery();
                }
            }
            Console.WriteLine($"Villain {villName} was added to the database.");
        }


        private static void InsertTown(SqlConnection connection, string townName)
        {
            using (connection)
            {
                
                string query = @"INSERT INTO Towns (Name, CountryId) VALUES (@newMinionTown, 5)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@newMinionTown", townName);

                    command.ExecuteNonQuery();
                }
            }
            Console.WriteLine($"Town {townName} was added to the database.");
        }

        private static int FindVillain(SqlConnection connection, string villname)
        {
            using (connection)
            {
                
                string query = "SELECT * FROM Villains WHERE Name = @villainName";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@villainName", villname);
                    var reader = command.ExecuteReader();
                    using (reader)
                    {
                        if (reader.HasRows)
                        {
                            var Id = 0;
                            while (reader.Read())
                            {
                                Id = (int)reader["Id"];
                            }
                            return Id;

                        }
                        return 0;
                    }
                }
            }
        }

        private static int FindTown(SqlConnection connection, string townMini)
        {
            using (connection)
            {
              
                string query = "SELECT * FROM Towns WHERE Name = @newMinionTown";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@newMinionTown", townMini);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            var townId = 0;
                            while (reader.Read())
                            {
                                townId = (int)reader["Id"];
                            }
                            return townId;

                        }
                        return 0;
                    }
                }
            }
        }

        private static void CreateConnectionBetweenVillainAndMinion(SqlConnection connection, int minionId,
           int villainId, string newMinionName, string villainName)
        {
           
            var command =
                new SqlCommand("INSERT INTO MinionsVillains (MinionId, VillainId) VALUES (@minionId, @villainId)",
                    connection);
            command.Parameters.AddWithValue("@minionId", minionId);
            command.Parameters.AddWithValue("@villainId", villainId);
            command.ExecuteNonQuery();
            Console.WriteLine($"Successfully added {newMinionName} to be minion of {villainName}.");
        }







    }
}
