using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBStatistik
{
    internal class Mall
    {
        public string driversLicense { get; set; }
        public string ownCar { get; set; }
        public int modelYear { get; set; }
        public double dieselPrice { get; set; }

        public Mall(string license, string car, int year, double price)
        {
            driversLicense = license;
            ownCar = car;
            modelYear = year;
            dieselPrice = price;
        }

    }
}
