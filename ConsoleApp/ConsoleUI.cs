using System;
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

            RunMainMenu();
        }

        static void RunMainMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("   МЕНЕДЖЕР ПРОДУКТІВ\n");
                Console.WriteLine("1. Показати усі склади");
                Console.WriteLine("2. Вихід з програми");
                Console.Write("\nВведіть ваш вибір: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ShowWarehouseMenu();
                        break;
                    case "2":
                        return;
                    default:
                        Console.WriteLine("Неправильний вибір,");
                        Console.WriteLine("Спробуйте ще раз: ");
                        Console.ReadKey();
                        break;
                }
            }
        }

        static void ShowWarehouseMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("   СПИСОК СКЛАДІВ\n");

                var warehouses = _storageService.GetWarehouses();

                foreach (var warehouse in warehouses)
                {
                    warehouse.Display();
                }

                Console.WriteLine("\nДля детальної інформації про склад введіть його 'ID'");
                Console.WriteLine("Для повернення натисність 'q':");
                Console.Write("\nВведіть вибір: ");

                string choice = Console.ReadLine();

                if (choice?.ToLower() == "q")
                    return;

                if (int.TryParse(choice, out int id))
                {
                    ShowDetailsOnWarehouse(id);
                }
                else
                {
                    Console.WriteLine("Невірний 'ID'");
                    Console.ReadKey();
                }
            }
        }

        static void ShowDetailsOnWarehouse(int id)
        {
            while (true)
            {
                Console.Clear();

                var warehouse = _storageService.GetWarehouseById(id);

                if (warehouse == null)
                {
                    Console.WriteLine("Склад не знайдено.");
                    Console.ReadKey();
                    return;
                }

                Console.WriteLine("   ДЕТАЛІ СКЛАДУ\n");
                warehouse.DisplayDetailed();

                var products = _storageService.GetProductsByWarehouse(warehouse.Guid);

                Console.WriteLine("\n   ПРОДУКТИ\n");

                foreach (var product in warehouse.Products)
                {
                    product.DisplayShort();
                }

                Console.WriteLine("\nВведіть ID продукту для деталей");
                Console.WriteLine("Для повернення натисність 'q'");
                Console.Write("\nВведіть вибір: ");

                string choice = Console.ReadLine();

                if (choice?.ToLower() == "q")
                    return;

                if (int.TryParse(choice, out int productId))
                {
                    ShowProductDetails(productId);
                }
                else
                {
                    Console.WriteLine("Невірний 'ID'");
                    Console.ReadLine();
                }
            }
        }

        static void ShowProductDetails(int id)
        {
            Console.Clear();

            var product = _storageService.GetProductById(id);

            if (product == null)
            {
                Console.WriteLine("Продукт не знайдено.");
            }
            else
            {
                Console.WriteLine("   ДЕТАЛІ ПРОДУКТУ\n");
                product.DisplayFull();
            }

            Console.WriteLine("\nНатисніть будь-яку клавішу для повернення.");
            Console.ReadKey();
        }
    }
}