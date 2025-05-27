using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopholy.WarehouseItems
{
    internal class Box : WarehouseItem
    {
        public DateTime? ProductionDate { get; private set; }
        private DateTime? ExpirationDateExplicit { get; set; }

        public override DateTime ExpirationDate 
        { 
            get 
            {
                if (ExpirationDateExplicit.HasValue)
                    return ExpirationDateExplicit.Value;

                return ProductionDate.Value.AddDays(100);
            }
        }

        private readonly float weight;
        public override float Weight => weight;

        public override float Volume => Width * Height * Depth;

        private static Random random = new();

        public Box() : this(GenerateRandomId()) { }

        public Box(string id)
        {
            Id = id;
            Width = random.Next(20, 120);
            Height = random.Next(10, 50);
            Depth = random.Next(20, 80);
            weight = (float)Math.Round(random.Next(1, 25) + (float)random.NextDouble(), 2);

            if (random.Next(2) == 0)
            {
                ProductionDate = DateTime.Today.AddDays(random.Next(-15, 16)).Date;
                ExpirationDateExplicit = null;
            }
            else
            {
                ProductionDate = null;
                ExpirationDateExplicit = DateTime.Today.AddDays(random.Next(-15, 16)).Date;
            }
        }

        private static string GenerateRandomId() => $"BOX-{random.Next(1000, 9999)}";
    }
}
