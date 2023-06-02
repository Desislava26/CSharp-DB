using CarDealer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CarDealer.DTOs
{
    [XmlType("Car")]
    public class CarDTO
    {
        [XmlElement("make")]
        public string Make { get; set; }

        [XmlElement("model")]
        public string Model { get; set; }

        [XmlElement("traveledDistance")]
        public long TraveledDistance { get; set; }
        [XmlArray("parts")]
        public PartCarDTO[] Parts { get; set; }
    }
}
    //< make > Opel </ make >
    //< model > Omega </ model >
    //< traveledDistance > 176664996 </ traveledDistance >
    //< parts >
    //  < partId id = "38" />