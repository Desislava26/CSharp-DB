using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.IdentityModel.Tokens;

namespace PrintAllMinionNames
{
    internal class StartUp
    {
        static void Main()
        {

            string stringConnection = "Server=.;Integrated Security=true;encrypt=false;Database=MinionsDB";

            SqlConnection connection = new SqlConnection(stringConnection);
            connection.Open();
            List<string> listing = new List<string>();
            using (connection)
            {
                string query = "SELECT Name FROM Minions";
                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            listing.Add((string)reader["Name"]);
                        }
                    }
                }
            }
            
            for (int i = 0; i < listing.Count; i++)
            {
                Console.WriteLine(listing[i]);
                
                for (int j = listing.Count-1; listing.Count-1 > 0; j--)
                {
                    Console.WriteLine(listing[j]);
                    listing.RemoveAt(i);
                    listing.RemoveAt(j-1);
                    
                    i--;
                    break;
                    
                }
            }
        }
    }
}
