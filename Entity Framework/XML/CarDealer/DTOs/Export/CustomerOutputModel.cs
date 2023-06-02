using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace CarDealer.DTOs.Export
{
    [XmlType("customer")]
    public class CustomerOutputModel
    {
        [XmlAttribute("name")]
        public string FullName { get; set; }

        [XmlAttribute("bought-cars")]
        public int BoughtCars { get; set; }
        [XmlAttribute("spend-money")]
        public decimal SpendMoney { get; set; }

        //[XmlArray("prop")]
        //public CustomerInfoModel MyProperty[] { get; set; }
    }

    public class CustomerInfoModel
    {

    }
}
        //< name > Marcelle Griego </ name >
        //< birthDate > 1990 - 10 - 04T00: 00:00 </ birthDate >
        //< isYoungDriver > true </ isYoungDriver >