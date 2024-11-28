using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServer
{
    internal class DBQuerier
    {
        /// <summary>
        /// The connection string.
        /// Your uID login name serves as both your database name and your uid
        /// </summary>
        public const string connectionString = "server=atr.eng.utah.edu;" +
      "database=u1466090;" +
      "uid=u1466090;" +
      "password=hey";

        public static void db(string[] args)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    // Open a connection
                    conn.Open();



                    // Create a command
                    MySqlCommand command = conn.CreateCommand();
                    command.CommandText = "delete from Games;";
                    command.CommandText += "Insert into Games (StartTime, EndTime) Values ('2024-11-27 06:06:06', '2024-11-27 06:06:06');";
                    command.CommandText += "SELECT * FROM Games;";

                    // Execute the command and cycle through the DataReader object
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            Console.WriteLine(reader["GameID"] + "\n" + reader["StartTime"] + "\n" + reader["EndTime"]);
                        }
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    
}
}
