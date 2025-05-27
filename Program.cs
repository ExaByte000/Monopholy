using Monopholy.WarehouseItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopholy
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== СИСТЕМА УПРАВЛЕНИЯ СКЛАДОМ ===\n");

            List<Palette> palettes = GenerateTestData();

            Console.WriteLine($"Сгенерировано {palettes.Count} паллет\n");

            Console.WriteLine("=== ГРУППИРОВКА ПАЛЛЕТ ПО СРОКУ ГОДНОСТИ ===");
            DisplayGroupedPalettes(palettes);

            Console.WriteLine("\n" + new string('=', 60) + "\n");

            Console.WriteLine("=== ТОП-3 ПАЛЛЕТЫ С НАИБОЛЬШИМ СРОКОМ ГОДНОСТИ КОРОБОК ===");
            DisplayTopPalettes(palettes);

            Console.WriteLine("\nНажмите любую клавишу для выхода...");
            Console.ReadKey();
        }

        private static List<Palette> GenerateTestData()
        {
            List<Palette> palettes = new();

            for (int i = 0; i < 15; i++)
            {
                palettes.Add(new Palette());
            }

            return palettes;
        }

        private static void DisplayGroupedPalettes(List<Palette> palettes)
        {
            var groupedPalettes = palettes
                .GroupBy(p => p.ExpirationDate.Date)
                .OrderBy(g => g.Key)
                .ToList();

            foreach (var group in groupedPalettes)
            {
                Console.WriteLine($"Срок годности: {group.Key:dd.MM.yyyy}");
                Console.WriteLine(new string('-', 40));

                var sortedPalettes = group.OrderByDescending(p => p.Weight).ToList();

                foreach (var palette in sortedPalettes)
                {
                    palette.Boxes = palette.Boxes.OrderBy(p => p.ExpirationDate.Date).ToList();

                    Console.WriteLine($"    Паллета: {palette.Id}");
                    Console.WriteLine($"    Размеры: {palette.Width}x{palette.Depth} см");
                    Console.WriteLine($"    Вес: {palette.Weight:F2} кг");
                    Console.WriteLine($"    Объем: {palette.Volume/100000:F2} м^3");
                    Console.WriteLine($"    Коробок: {palette.Boxes.Count}");


                    foreach (var box in palette.Boxes)
                    {
                        Console.WriteLine($"      └─ {box.Id}: Годен до: {box.ExpirationDate:dd.MM.yyyy}, Произв: {box.ProductionDate:dd.MM.yyyy}, {box.Weight:F1}кг, {box.Volume/100000:F2} м^3, {box.Width}x{box.Height}x{box.Depth} см");
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
            }
        }

        private static void DisplayTopPalettes(List<Palette> palettes)
        {
            var maxExpirationDate = palettes
                .SelectMany(p => p.Boxes)
                .Max(b => b.ExpirationDate);

            Console.WriteLine($"Максимальный срок годности коробок: {maxExpirationDate:dd.MM.yyyy}\n");

            var palettesWithMaxExpiration = palettes
                .Where(p => p.Boxes.Any(b => b.ExpirationDate.Date == maxExpirationDate.Date))
                .OrderBy(p => p.Volume)
                .Take(3)
                .ToList();

            if (!palettesWithMaxExpiration.Any())
            {
                Console.WriteLine("Паллеты с коробками максимального срока годности не найдены.");
                return;
            }

            for (int i = 0; i < palettesWithMaxExpiration.Count; i++)
            {
                var palette = palettesWithMaxExpiration[i];
                var maxBoxes = palette.Boxes.Where(b => b.ExpirationDate.Date == maxExpirationDate.Date).ToList();

                Console.WriteLine($"{i+1}.  Паллета: {palette.Id}");
                Console.WriteLine($"    Размеры: {palette.Width}x{palette.Depth} см");
                Console.WriteLine($"    Вес: {palette.Weight:F2} кг");
                Console.WriteLine($"    Объем: {palette.Volume / 100000:F2} м^3");
                Console.WriteLine($"    Коробок: {palette.Boxes.Count}");

                foreach (var box in maxBoxes)
                {
                    Console.WriteLine($"      └─ {box.Id}: Годен до: {box.ExpirationDate:dd.MM.yyyy}, Произв: {box.ProductionDate:dd.MM.yyyy}, {box.Weight:F1}кг, {box.Volume / 100000:F2} м^3, {box.Width}x{box.Height}x{box.Depth} см");
                }
                Console.WriteLine();
            }
        }
    }
}