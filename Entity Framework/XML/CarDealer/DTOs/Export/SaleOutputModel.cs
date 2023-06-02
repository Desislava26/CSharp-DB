using CarDealer.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace CarDealer.DTOs.Export
{
    [XmlType("sale")]
    public class SaleOutputModel
    {
        [XmlElement("car")]

        public CarSaleOutputModel Car { get; set; }

        [XmlElement("discount")]
        public decimal Discount { get; set; }
        [XmlElement("customer-name")]
        public string CustomerName { get; set; }
        [XmlElement("price")]
        public decimal Price { get; set; }
        [XmlElement("price-with-discount")]
        public decimal PriceWithDiscount { get; set; }

    }
    [XmlType("car")]
    public class CarSaleOutputModel
    {
        [XmlElement("make")]
        public string Make { get; set; }
        [XmlElement("model")]
        public string Model { get; set; }
        [XmlElement("travelled-distance")]
        public long TravelledDistance { get; set; }
    }
}

  //< sale >
  //  < car make = "Opel" model = "Omega" traveled - distance = "109910837" />
  //  < discount > 30.00 </ discount >
  //  < customer - name > Zada Attwoood </ customer - name >
  //  < price > 330.97 </ price >
  //  < price - with - discount > 231.68 </ price - with - discount >
  //</ sale >
