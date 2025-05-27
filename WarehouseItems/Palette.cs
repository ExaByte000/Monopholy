using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopholy.WarehouseItems
{
    internal class Palette : WarehouseItem
    {
        private List<Box> boxList = new();

        public List<Box> Boxes { get { return boxList; } set { boxList = value; } }

        public override float Weight => boxList.Sum(box => box.Weight) + 30f;

        public override float Volume => boxList.Sum(box => box.Volume) + (Width * Height * Depth); 

        public override DateTime ExpirationDate => !boxList.Any() ? DateTime.MaxValue : boxList.Min(box => box.ExpirationDate);
        

        private static Random random = new Random();

        public Palette() : this(GenerateRandomId()) { }

        public Palette(string id)
        {
            Id = id;
            var sizes = new[] { (120f, 80f), (120f, 100f), (100f, 80f) };
            var size = sizes[random.Next(sizes.Length)];

            Width = size.Item1;
            Depth = size.Item2;
            Height = 15f;

            
            int boxCount = random.Next(3, 9);
            for (int i = 0; i < boxCount; i++)
            {
                Box box = new();
                if (box.Width > Width)
                {
                    i--;
                    continue;
                }
                else if (box.Depth > Depth)
                {
                    i--;
                    continue;
                }

                AddBox(box);
            }
        }

        private void AddBox(Box box)
        {
            boxList.Add(box);
        }

        private static string GenerateRandomId() => $"PAL-{random.Next(1000, 9999)}";


    }
}
