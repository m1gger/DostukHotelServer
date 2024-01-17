using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyServer
{
    internal class Room
    {

        private int roomId;
        private string roomsType;
        private float roomsCena;
        private bool isFree;
        private int roomsVmest;
        private DateTime checkoutDate;
        private DateTime checkintDate;


      

        public int RoomId
        {
            get
            {
                return roomId;
            }

            set
            {
                roomId = value;
            }
        }

        public string RoomsType
        {
            get
            {
                return roomsType;
            }

            set
            {
                roomsType = value;
            }
        }
        public float RoomsCena
        {
            get
            {
                return roomsCena;
            }

            set
            {
                roomsCena = value;
            }
        }
        public bool IsFree
        {
            get
            {
                return isFree;
            }
            set
            {
                isFree = value;
            }
        }
        public int RoomVmest
        {
            get
            {
                return roomsVmest;
            }
            set
            {
                roomsVmest = value;
            }
        }
        public DateTime CheckoutDate 
        {
            get 
            { 
                return checkoutDate;
            }
            set 
            { 
                checkoutDate = value; 
            }
        
        }




        public Room(int roomId, string roomsType, float roomsCena, bool isFree, int roomsVmest, DateTime checkoutDate,DateTime checkinDate)
        {
            this.roomId = roomId;
            this.roomsType = roomsType;
            this.roomsCena = roomsCena;
            this.isFree = isFree;
            this.roomsVmest = roomsVmest;
            this.checkoutDate = checkoutDate;
            this.checkintDate= checkinDate;
        }
        public override string ToString()
        {

            return String.Format($"{roomId} {roomsType} {roomsCena} {roomsVmest}");
            // return String.Format($"{roomId} {roomsType} {roomsCena}  {roomsVmest} {checkoutDate.Date.ToString("yyyy-MM-dd")} {checkintDate.Date.ToString("yyyy-MM-dd")}");
        }





        // получаем список комнат свободных на данные даты
        public static List<Room> ReturnListOfAvailableRooms(DateTime checkIn, DateTime checkOut) 
        {

            {
                List<Room> rooms = new();
                var connectionString = "Data Source=DESKTOP-Q9593L8;Initial Catalog=Hotel;User ID=sa;Password=1234";
                using (SqlConnection connection = new(connectionString))
                {
                    connection.Open();

                    string query = "SELECT R.RoomNumber AS Id, R.Тип_комнаты, R.Цена, R.Состояние, R.Вместимость, Guests.CheckOutDate,Guests.CheckInDate\r\nFROM Rooms R\r\n full JOIN Guests ON R.RoomNumber = Guests.RoomNumber;\r\n";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DateTime tempOut;
                                if (!reader.IsDBNull(5))
                                {
                                    tempOut = reader.GetDateTime(5);
                                }
                                else
                                {
                                    tempOut = DateTime.MinValue;
                                }
                                DateTime tempIn;
                                if (!reader.IsDBNull(5))
                                {
                                    tempIn = reader.GetDateTime(6);
                                }
                                else
                                {
                                    tempIn = DateTime.MinValue;
                                }

                                Room room = new Room(reader.GetInt32(0), reader.GetString(1), Convert.ToSingle(reader.GetDecimal(2)), reader.GetBoolean(3), reader.GetInt32(4), tempOut, tempIn);
                                rooms.Add(room);
                            }
                        }
                    }
                    List<Room> avaibllerooms = new List<Room>();
     
                    List<int> notavids = new List<int>();
                    ;
                    foreach (Room room1 in rooms) 
                    {

                        if (room1.checkoutDate <= checkIn || room1.checkintDate >= checkOut || room1.IsFree)
                        {
                       //     ids.Add(room1.RoomId);
                          
                            avaibllerooms.Add(room1);
                        }
                        else 
                        {
                          
                            notavids.Add(room1.RoomId);

                        }
                        
                    
                    }
                    List<Room> tochtosvobodny = new List<Room>();
                    foreach (Room room in rooms) 
                    {
                        if (!notavids.Contains(room.RoomId)) 
                        {

                            tochtosvobodny.Add(room);
                        }
                    }
                    //  удаление повторяющихся записей при помощи linQ
                    avaibllerooms= tochtosvobodny
                    .Where(room => !notavids.Contains(room.RoomId))
                    .GroupBy(room => room.RoomId)
                    .Select(group => group.First())
                    .ToList();



                    return avaibllerooms;
                }



            }
        }

        // список румов в массив строк
        
         // Список румов в строку
        public static string WriteList(List<Room> rooms)

        {

            int availableLux = 0;
            int availableSuperLux = 0;
            int availableKingSize = 0;
            int availableTwin = 0;
            int availableSingle = 0;

            float LuxCost = 0;
            float SuperLuxCost = 0;
            float KingSizeCost = 0;
            float TwinCost = 0;
            float SingleCost = 0;

            int LuxCol = 0;
            int SuperCol = 0;
            int KingCol = 0;
            int TwinCol = 0;
            int SingleCol = 0;


            var str = string.Empty;
            foreach (Room room in rooms)
            {
                switch (room.RoomsType) 
                {
                    case "Lux":
                        availableLux++;
                        LuxCost = room.RoomsCena;
                        LuxCol = room.RoomVmest;
                        break;
                    case "Super Lux":
                        availableSuperLux++;
                        SuperLuxCost = room.RoomsCena;
                        SuperCol= room.RoomVmest;
                        break;
                        
                    case "King-Size":
                        availableKingSize++;
                        KingSizeCost = room.RoomsCena;
                        KingCol = room.RoomVmest;
                        break;
                    case "Twin":
                        availableTwin++;
                        TwinCost = room.RoomsCena;
                        TwinCol = room.RoomVmest;
                        break;
                    case "Single":
                        availableSingle++;
                        SingleCost = room.RoomsCena;
                        SingleCol = room.RoomVmest;
                        break;
                }
                
            }
            //str = $"Lux {availableLux} {LuxCol} {LuxCost}\nSuper-Lux {availableSuperLux} {SuperCol} {SuperLuxCost}\nKing-Size {availableKingSize} {KingCol} {KingSizeCost}\nTwin {availableTwin} {TwinCol} {TwinCost}\nSingle {availableSingle} {SingleCol} {SingleCost}";
            str = $"{availableLux} Lux {LuxCost} {LuxCol}\n{availableSuperLux} Super-Lux {SuperLuxCost} {SuperCol}\n{availableKingSize} King-Size {KingSizeCost} {KingCol}\n{availableTwin} Twin {TwinCost} {TwinCol}\n{availableSingle} Single {SingleCost} {SingleCol}";
            return str;
        }

        // метод для получения всех записей в качестве строки , использовался при отладке

        public static string ReturnRoomsList()

        {
            List<Room> rooms = new List<Room>();
            var connectionString = "Data Source=DESKTOP-Q9593L8;Initial Catalog=Hotel;User ID=sa;Password=1234";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT R.RoomNumber AS Id, R.Тип_комнаты, R.Цена, R.Состояние, R.Вместимость, Guests.CheckOutDate,Guests.CheckInDate\r\nFROM Rooms R\r\n full JOIN Guests ON R.RoomNumber = Guests.RoomNumber;\r\n";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DateTime tempOut;
                            if (!reader.IsDBNull(5))
                            {
                                tempOut = reader.GetDateTime(5);
                            }
                            else 
                            {
                                tempOut = DateTime.MinValue;
                            }
                            DateTime tempIn;
                            if (!reader.IsDBNull(5))
                            {
                                tempIn = reader.GetDateTime(6);
                            }
                            else
                            {
                                tempIn = DateTime.MinValue;
                            }

                            Room room = new Room(reader.GetInt32(0), reader.GetString(1), Convert.ToSingle(reader.GetDecimal(2)), reader.GetBoolean(3), reader.GetInt32(4),tempOut,tempIn);
                            rooms.Add(room);
                        }
                    }
                }
                string stringRoomsList = "";
                foreach (Room room in rooms)
                {
                    stringRoomsList += room.ToString() + "\n";
                }



                return stringRoomsList;
            }



        }
        public static int GetRoomIdFromType(string roomsType, DateTime checkIn, DateTime checkOut) 
        {
            int roomId = 0;
            List<Room> rooms = ReturnListOfAvailableRooms(checkIn, checkOut);
            foreach (Room room in rooms) 
            {
                if (room.RoomsType == roomsType) 
                {
                    return room.RoomId;
                }
            }
            return roomId;
        }
        public static int  BookRoom(string login,string roomsType, DateTime checkIn, DateTime checkOut )
        {
            int UserId= UserName.GetUserIdByLogin(login);
            int roomID = GetRoomIdFromType(roomsType, checkIn, checkOut);
            if (roomID > 0)
            {
                var connectionString = "Data Source=DESKTOP-Q9593L8;Initial Catalog=Hotel;User ID=sa;Password=1234";
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Создаем новую запись о бронировании
                    using (var command = new SqlCommand("INSERT INTO Guests (UserID, RoomNumber, CheckInDate, CheckOutDate) VALUES (@UserID, @RoomNumber, @CheckInDate, @CheckOutDate)", connection))
                    {
                        command.Parameters.AddWithValue("@UserID", UserId);
                        command.Parameters.AddWithValue("@RoomNumber", roomID);
                        command.Parameters.AddWithValue("@CheckInDate", checkIn);
                        command.Parameters.AddWithValue("@CheckOutDate", checkOut);

                        // Выполняем команду
                        command.ExecuteNonQuery();
                    }

                    connection.Close();
                }

            }
            else return -1;

            return 1;
        }
    }
}
