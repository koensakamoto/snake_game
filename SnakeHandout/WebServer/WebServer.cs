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

        public const string errorString = "<h1>Not Found :<</h1>";



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
                string response = string.Empty;
                response += httpOkHeader;
                response += "<h1>Welcome to Dom's and Koen's Games</h1>";
                response += "<a href = \"games\"/games>Games</a>";

                connection.Send(response);

            }
            else if (request.Contains("GET /games"))//games page
            {
                string response = string.Empty;
                if (request.Contains("?gid="))
                {
                    int index = request.IndexOf("=") + 1;

                    if (int.TryParse(request.Substring(index, request.IndexOf(" ")), out int gid))// game request follows format
                    {
                        using (MySqlConnection conn = new MySqlConnection(connectionString))
                        {
                            try
                            {
                                // Open a connection
                                conn.Open();
                                MySqlCommand command = conn.CreateCommand();


                                command.CommandText += "SELECT PlayerID, Name, MaxScore, EnterTime, LeaveTime FROM Players" +
                                    $" where GameID = {gid};";

                                response += httpOkHeader + "<html>" +
                                    "<table border = \"1\">" +
                                    "<thead>" +
                                    "<tr>" +
                                    "<td>ID</td><td>Name</td><td>MaxScore</td><td>EnterTime</td><td>LeaveTime</td>" +
                                    "</tr>" +
                                    "</thead>";
                                response += "<tbody>";
                                using (MySqlDataReader reader = command.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        response += "<tr>" +
                                            $"<td>{reader["PlayerID"]}</td>" +
                                            $"<td>{reader["Name"]}</td>" +
                                            $"<td>{reader["MaxScore"]}</td>" +
                                            $"<td>{reader["EnterTime"]}</td>" +
                                            $"<td>{reader["LeaveTime"]}</td>";
                                       
                                    }
                                }
                                response += "</tbody>" +
                                    "</table>" +
                                    "</html>";


                            }

                            catch (Exception e)
                            {
                                response = httpBadHeader + errorString;//failed query
                            }
                        }
                    }
                    else
                    {
                        response = httpBadHeader + errorString;
                    }

                }
                else//looking at games home page
                {
                    using (MySqlConnection conn = new MySqlConnection(connectionString))
                    {
                        try
                        {
                            // Open a connection
                            conn.Open();
                            MySqlCommand command = conn.CreateCommand();


                            command.CommandText += "SELECT * from Games";

                            response += httpOkHeader + "<html>" +
                                "<table border = \"1\">" +
                                "<thead>" +
                                "<tr>" +
                                "<td>ID</td><td>StartTime</td><td>EndTime</td>" +
                                "</tr>" +
                                "</thead>";
                            response += "<tbody>";
                            using (MySqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    response += "<tr>" +
                                        $"<td><a href = \"/games?gid={reader["GameID"]}\">{reader["GameID"]}</a></td>" +
                                        $"<td>{reader["StartTime"]}</td>" +
                                        $"<td>{reader["EndTime"]}</td>";
                                       
                                }
                            }
                            response += "</tbody>" +
                                "</table>" +
                                "</html>";


                        }

                        catch (Exception e)
                        {
                            response = httpBadHeader + errorString;//failed query
                        }
                    }

                }
                connection.Send(response);
            }
            else
            {
                connection.Send(httpBadHeader + errorString);
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

