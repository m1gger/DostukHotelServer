using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Collections.Generic;

namespace MyServer
{
    // Operation:
    // 1 - login
    // 2 - register
    // 3 - получить имя
    // 4 - получить фамилию
    // 5 - является ли гостем
    // Errors:
    // 1 - Успех
    // 2 - Заполните поля
    // 3 - Неверный логин или пароль
    // 4 - Логин уже занят
    // 5 - номер комнаты
     // операции с комнатами
    // 6 получить список комнат с свободных в указанные дни
    // 7 заселиться в выбранную комнату
    class Program
    {
        static void Main()
        {
            string str = UserName.isGuestRoom("dima1408");
            //Console.WriteLine(str);
           List <Room> rooms = Room.ReturnListOfAvailableRooms(DateTime.Parse("17-01-2024"), DateTime.Parse("20-01-2024"));
            //  Console.WriteLine();


             Console.WriteLine(Room.WriteList(rooms));
           

         //   Console.WriteLine("Все комнаты");


            //  Console.WriteLine(Room.ReturnRoomsList());
            Server.StartServer();
            
        }

    }

}
