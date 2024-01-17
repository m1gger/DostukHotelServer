using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MyServer
{
    public static class RegistrationLogin
    {
        public static string Register(string name, string surname, string login, string password)
        {
            string error = "1";
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(surname) || string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                error = "2"; // Пустое поле
                return error; 
            }
            
            var connectionString = "Data Source=DESKTOP-Q9593L8;Initial Catalog=Hotel;User ID=sa;Password=1234";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand("SELECT * FROM Users WHERE Логин = @Логин", connection))
                {
                    command.Parameters.AddWithValue("@Логин", login);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            return "4"; // Логин существует
                        }
                        else
                        {
                            error = "1";
                        }
                    }
                }

                using (var command = new SqlCommand("INSERT INTO Users (Имя, Фамилия, Логин, Пароль, Дата_регистрации) VALUES (@Имя, @Фамилия, @Логин, @Пароль, @Дата_регистрации)", connection))
                {
                    command.Parameters.AddWithValue("@Имя", name);
                    command.Parameters.AddWithValue("@Фамилия", surname);
                    command.Parameters.AddWithValue("@Логин", login);
                    command.Parameters.AddWithValue("@Пароль", password);
                    command.Parameters.AddWithValue("@Дата_регистрации", DateTime.Now);

                    command.ExecuteNonQuery();
                }

                

                connection.Close();
            }

            return error;
        }

        public static string Login(string login, string password)
        {
            string error = "1";
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                error = "2"; // Пустое поле
                return error; 
            }
            var connectionString = "Data Source=DESKTOP-Q9593L8;Initial Catalog=Hotel;User ID=sa;Password=1234";         
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand("SELECT * FROM Users WHERE Логин = @Логин AND Пароль = @Пароль", connection))
                {
                    command.Parameters.AddWithValue("@Логин", login);
                    command.Parameters.AddWithValue("@Пароль", password);

                    using (var reader = command.ExecuteReader())
                    {
                        if(reader.HasRows)
                        {
                            error = "1";
                        }
                        else
                        {
                            error = "3"; // Неверный логин или пароль
                        }
                    }
                }

                connection.Close();
            }

            return error;
        }
    }
}
