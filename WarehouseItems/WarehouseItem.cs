using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopholy.WarehouseItems
{
    internal abstract class WarehouseItem
    {
        public string Id;
        public float Width;
        public float Height;
        public float Depth;

        public abstract float Weight { get; }
        public abstract float Volume { get; }
        public abstract DateTime ExpirationDate { get;}
    }
}
