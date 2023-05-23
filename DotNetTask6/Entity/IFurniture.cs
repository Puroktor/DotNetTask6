using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetTask6.Entity
{
    public interface IFurniture
    {
        public double Price { get; set; }

        public string Buy();

        public string Sell(double discount);
    }
}
