using CarDealer.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace CarDealer.DTOs
{
    [XmlType("Part")]
    public class PartDTO
    {
        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("price")]
        public decimal Price { get; set; }

        [XmlElement("quantity")]
        public int Quantity { get; set; }
        [XmlElement("supplierId")]
        public int SupplierId { get; set; } = new int();
    }
}

    //< Part >
    //    < name > Bonnet / hood </ name >
    //    < price > 1001.34 </ price >
    //    < quantity > 10 </ quantity >
    //    < supplierId > 17 </ supplierId >
    //</ Part >
