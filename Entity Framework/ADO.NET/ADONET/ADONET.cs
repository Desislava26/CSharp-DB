using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;

namespace ADONET
{
     class Class1
    {
        static void Main()
        {
            string stringConnection = "Server=.;Integrated Security=true;encrypt=false;Database=MinionsDB";
            var connection = new SqlConnection(stringConnection);
            //CreatingDB(connection);
            //InsertingInformation(connection);
            //VillainNames(connection);
            MinionNames(connection);


        }

        public static void CreatingDB(SqlConnection connection)
        {
            using (connection)
            {
                connection.Open();
                string query = @"
                    USE MinionsDB

                    CREATE TABLE Countries
                    (Id INT PRIMARY KEY, Name VARCHAR(50))

                    CREATE TABLE Towns
                    (Id INT PRIMARY KEY, Name VARCHAR(50), 
                    CountryCode INT REFERENCES Countries(Id))

                    CREATE TABLE Minions
                    (Id INT PRIMARY KEY, Name VARCHAR(50), 
                    Age INT, 
                    TownId INT REFERENCES Towns(Id))

                    CREATE TABLE EvilnessFactors
                    (Id INT PRIMARY KEY, Name VARCHAR(50))

                    CREATE TABLE Villains
                    (Id INT PRIMARY KEY, Name VARCHAR(50), 
                    EvilnessFactorId INT REFERENCES EvilnessFactors(Id))

                    CREATE TABLE MinionsVillains
                    (MinionId INT REFERENCES Minions(Id), 
                    VillainId INT REFERENCES Villains(Id), 
                    PRIMARY KEY(MinionId, VillainId))";
                SqlCommand sqlCommand = new SqlCommand(query, connection);
                sqlCommand.ExecuteNonQuery();


            }
        }

        public static void InsertingInformation(SqlConnection connection)
        {
            using (connection)
            {
                connection.Open();
                string query = @"
                    INSERT INTO Countries(Id, Name) 
                    VALUES (1, 'Bulgaria'), (2, 'Norway'), (3, 'Cyprus'), (4, 'Greece'), (5, 'UK')
                    INSERT INTO Towns(Id, Name, CountryCode) 
                    VALUES (1, 'Plovdiv', 1), (2, 'Oslo', 2), (3, 'Larnaka', 3), (4, 'Athens', 4), (5, 'London', 5)
                    INSERT INTO Minions(Id, Name, Age, TownId) 
                    VALUES (1, 'Stoyan', 12, 1), (2, 'George', 22, 2), (3, 'Ivan', 25, 3), (4, 'Kiro', 35, 4), (5, 'Niki', 25, 5)
                    INSERT INTO EvilnessFactors(Id, Name) 
                    VALUES (1, 'super good'), (2, 'good'), (3, 'bad'), (4, 'evil'),(5, 'super evil')
                    INSERT INTO Villains(Id, Name, EvilnessFactorId) 
                    VALUES (1, 'Gru', 1), (2, 'Ivo', 2), (3, 'Teo', 3), (4, 'Sto', 4), (5, 'Pro', 5)
                    INSERT INTO MinionsVillains 
                    VALUES (1,1), (2,2), (3,3), (4,4), (5,5)
                    INSERT INTO MinionsVillains 
                    VALUES (1, 2), (3, 1), (1, 3), (4, 1), (3, 4), (1, 4), (1, 5), (5, 1)";
                SqlCommand sqlCommand = new SqlCommand(query, connection);
                sqlCommand.ExecuteNonQuery();


            }

        }

        public static void VillainNames(SqlConnection connection)
        {
            string query = @"SELECT
                            v.Name,
                            COUNT(mv.MinionId) as Counting
                            FROM Villains AS v
                            JOIN MinionsVillains AS mv ON mv.VillainId = v.Id
                            GROUP BY v.Id, v.Name
                            HAVING COUNT(mv.MinionId) > 3";
            using (connection)
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"{reader[0]} - {reader[1]}");
                        }
                    }
                }
            }

        }


        public static void MinionNames(SqlConnection connection)
        {
            
            int villiandId = int.Parse(Console.ReadLine());
            string query = @"SELECT Name FROM Villains WHERE Id = @villianId";

            using (connection)
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@villianId", villiandId);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (reader.HasRows)
                            {
                                Console.WriteLine($"Villain: {reader["Name"]}");
                                reader.Close();

                                string queryTwo = @"SELECT 
                                                m.Name, 
                                                m.Age
                                                FROM Villains AS v
                                                JOIN MinionsVillains AS mv
                                                ON mv.VillainId = v.Id
                                                JOIN Minions AS m
                                                ON m.Id = mv.MinionId
                                                WHERE v.Id = @villainId
                                                ORDER BY m.Name";
                                var sqlcommand = new SqlCommand(queryTwo, connection);
                             
                                sqlcommand.Parameters.AddWithValue("@villainId", villiandId);
                                using(SqlDataReader sqlDataReader = sqlcommand.ExecuteReader())
                                {
                                    if (sqlDataReader.HasRows)
                                    {
                                        int index = 1;
                                        while (sqlDataReader.Read())
                                        {
                                            Console.WriteLine($"{index}. {sqlDataReader["Name"]} {sqlDataReader["Age"]}");
                                            index++;
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine($"(no minions)");
                                    }
                                }
                            }
                            
                            break;
                        }
                        Console.WriteLine($"No villain with ID {villiandId} exists in the database.");
                    }
                }
            }

        }



    }
}

