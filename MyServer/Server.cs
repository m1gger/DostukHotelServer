







using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MyServer
{

    public static class Server
    {
        public static void StartServer()
        {
            TcpListener ?server = null;
            try
            {
                int port = 1488;
                IPAddress localAddr = IPAddress.Parse("192.168.137.1");

                server = new TcpListener(localAddr, port);

                server.Start();
                Console.WriteLine($"Server is running on {localAddr}:{port}");

                while (true)
                {
                    Console.WriteLine("Waiting for connection...");

                    TcpClient client = server.AcceptTcpClient();

                    // Use Task.Run to create a new thread for each client
                    Task.Run(() => HandleClient(client));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }
            finally
            {
                server?.Stop();
            }
        }

        private static void HandleClient(TcpClient client)
        {
            try
            {
                NetworkStream stream = client.GetStream();

                byte[] data = new byte[1024];
                int bytesRead = stream.Read(data, 0, data.Length);
                StringBuilder jsonString = new();
                jsonString.Append(Encoding.UTF8.GetString(data, 0, bytesRead));
                string[] ?stringArray = JsonSerializer.Deserialize<string[]>(jsonString.ToString());

                string answer = "0";
                if (stringArray.Length > 0)
                {
                    switch (stringArray[0])
                    {
                        case "1":
                            answer = RegistrationLogin.Login(stringArray[1], stringArray[2]);
                            break;
                        case "2":
                            answer = RegistrationLogin.Register(stringArray[1], stringArray[2], stringArray[3], stringArray[4]);
                            break;
                        case "3":
                            answer = UserName.GetUserName(stringArray[1]);
                            break;
                        case "4":
                            answer = UserName.GetUserSurname(stringArray[1]);
                            break;
                        case "5":
                            answer = UserName.isGuestRoom(stringArray[1]);
                            break;
                        case "6":
                            DateTime checkIn = DateTime.Parse(stringArray[1]);
                            DateTime checkOut = DateTime.Parse(stringArray[2]);
                            List<Room> rooms = Room.ReturnListOfAvailableRooms(checkIn, checkOut);
                            answer = Room.WriteList(rooms);
                            break;
                        case "7":
                            string ass = "";
                            answer = ass + Room.BookRoom(stringArray[1], stringArray[2], DateTime.Parse(stringArray[3]), DateTime.Parse(stringArray[4]));
                            break;
                    }
                }


                foreach (string str in stringArray)
                {
                    Console.WriteLine($"{str}");
                }

                byte[] responseBytes = Encoding.UTF8.GetBytes(answer);
                stream.Write(responseBytes, 0, responseBytes.Length);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error handling client: {e.Message}");
            }
            finally
            {
                client.Close();
            }
        }
    }
}
