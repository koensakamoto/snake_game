using CS3500.Networking;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Net;
using System.Net.Sockets;

namespace WebServer
{
    public static class WebServer
    {

        private const string httpOkHeader =
            "HTTP/1.1 200 OK\r\n" +
            "Connection: close\r\n" +
            "Content-Type: text/html; charset=UTF-8\r\n" +
            "\r\n";
        private const string httpBadHeader =
             "HTTP/1.1 404 Not Found\r\n" +
            "Connection: close\r\n" +
            "Content-Type: text/html; charset=UTF-8\r\n" +
            "\r\n";
        /// <summary>
        /// The connection string.
        /// Your uID login name serves as both your database name and your uid
        /// </summary>
        public const string connectionString = "server=atr.eng.utah.edu;" +
      "database=u1466090;" +
      "uid=u1466090;" +
      "password=hey";



        static void Main(string[] args)
        {
            Server.StartServer(HTTPRequest, 10000);
            Console.Read();

        }

        static void HTTPRequest(NetworkConnection connection)
        {

            string request = connection.ReadLine();
            if (request.Contains("GET / "))//home page
            {
                connection.Send(httpOkHeader + QueryDatabase(false,0));

            } else if (request.Contains("GET /games"))//games page
            {
                connection.Send(httpOkHeader);
            }
            else
            {
                connection.Send(httpBadHeader);
                connection.Send("Not Found :<");
            }
            connection.Disconnect();
        }

        static string QueryDatabase(bool specificGame, int gameID)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    // Open a connection
                    conn.Open();
                    MySqlCommand command = conn.CreateCommand();

                    if (!specificGame) 
                    { 
                        command.CommandText += "SELECT * FROM Games;";
                        string response = string.Empty;
                        // Execute the command and cycle through the DataReader object
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {

                            while (reader.Read())
                            {
                                response += (reader["GameID"] + "\n" + reader["StartTime"] + "\n" + reader["EndTime"]);
                            }
                        }
                        return response;

                    }
                    else { return string.Empty; }
                    

                    
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                return string.Empty;
            }
        }
    }
    }

