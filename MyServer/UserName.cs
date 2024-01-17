using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Data.SqlClient;

namespace MyServer
{
    public static class UserName
    {
        public static string GetUserName(string login)
        {
            string userName = "";
            var connectionString = "Data Source=DESKTOP-Q9593L8;Initial Catalog=Hotel;User ID=sa;Password=1234";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand("SELECT Имя FROM Users WHERE Логин = @Логин", connection))
                {
                    command.Parameters.AddWithValue("@Логин", login);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            userName = reader["Имя"].ToString();
                        }
                    }
                }
                connection.Close();
            }
            return userName;
        }

        public static string GetUserSurname(string login)
        {
            string userSurname = "";
            var connectionString = "Data Source=DESKTOP-Q9593L8;Initial Catalog=Hotel;User ID=sa;Password=1234";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand("SELECT Фамилия FROM Users WHERE Логин = @Логин", connection))
                {
                    command.Parameters.AddWithValue("@Логин", login);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            userSurname = reader["Фамилия"].ToString();
                        }
                    }
                }
                connection.Close();
            }
            return userSurname;
        }

        public static string isGuestRoom(string login)
        {
            string answer = "-1";
            int userId = -1;
            int roomNumber = -1;
            DateTime checkInDate = DateTime.MinValue;
            DateTime checkOutDate = DateTime.MinValue;

            var connectionString = "Data Source=DESKTOP-Q9593L8;Initial Catalog=Hotel;User ID=sa;Password=1234";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Выбираем RoomNumber, CheckInDate и CheckOutDate по логину пользователя
                using (var command = new SqlCommand("SELECT G.RoomNumber, G.CheckInDate, G.CheckOutDate FROM Users U JOIN Guests G ON U.UserID = G.UserID WHERE U.Логин = @Логин", connection))
                {
                    command.Parameters.AddWithValue("@Логин", login);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            roomNumber = reader.GetInt32(reader.GetOrdinal("RoomNumber"));
                            checkInDate = reader.GetDateTime(reader.GetOrdinal("CheckInDate"));
                            checkOutDate = reader.GetDateTime(reader.GetOrdinal("CheckOutDate"));
                        }
                        else
                        {
                            return answer;
                        }
                    }
                }

                // Получаем тип комнаты по номеру комнаты
                using (var command = new SqlCommand("SELECT Тип_комнаты FROM Rooms WHERE RoomNumber = @RoomNumber", connection))
                {
                    command.Parameters.AddWithValue("@RoomNumber", roomNumber);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            answer = $"{reader["Тип_комнаты"]} {checkInDate.ToString("yyyy-MM-dd")} {checkOutDate.ToString("yyyy-MM-dd")}";
                        }
                    }
                }

                connection.Close();
            }

            return answer;
        }
        public static int GetUserIdByLogin(string login)
        {
            int userId = -1;

            try
            {
                var connectionString = "Data Source=DESKTOP-Q9593L8;Initial Catalog=Hotel;User ID=sa;Password=1234";
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (var command = new SqlCommand("SELECT UserID FROM Users WHERE Логин = @Login", connection))
                    {
                        command.Parameters.AddWithValue("@Login", login);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                userId = reader.GetInt32(reader.GetOrdinal("UserID"));
                            }
                        }
                    }

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting UserID by login: {ex.Message}");
            }

            return userId;
        }

    }
}
