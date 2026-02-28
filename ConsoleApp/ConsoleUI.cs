using System;
using System.Collections.Generic;
using System.Text;
using Services;

namespace ConsoleApp
{
    internal class ConsoleUI
    {
        internal static StorageService _storageService = new StorageService();
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            Console.Clear();
            Console.WriteLine("МЕНЕДЖЕР ПРОДУКТІВ");
            Console.WriteLine("1. Показати усі склади");
            Console.WriteLine("2. Вихід з програми");
            Console.WriteLine("\nВведіть номер вашого вибору: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    ShowWarehouseMenu();
                    break;
                case "2":
                    break;
                default:
                    Console.WriteLine("Неправильний вибір. Спробуйте ще раз: ");
                    Console.ReadKey();
                    break;
            }
        }

        static void ShowWarehouseMenu()
        {
            Console.Clear();
            Console.WriteLine("СПИСОК СКЛАДІВ");

            var warehouses = _storageService.GetWarehouses();

            foreach (var warehouse in warehouses)
            {
                warehouse.Display();
            }

            Console.WriteLine("\nДля детальної інформації про склад введіть його 'ID'" +
                "\nДля повернення натисність 'q': ");

            string choice = Console.ReadLine();

            if (choice? == "q") return;

            if (int.TryParse(choice, out int Id))
            {
                
            }
            else
            {
                Console.WriteLine("Неправильно введений 'ID'");
                Console.WriteLine("Спробуйте ще раз: ");
                Console.ReadLine();
            }
        }

        static void ShowDetailsOnWarehouse(int id)
        {
            Console.Clear();
            var warehouse = _storageService.GetWarehouseById(id);

            warehouse.DisplayWithProducts
        }
    }
}
