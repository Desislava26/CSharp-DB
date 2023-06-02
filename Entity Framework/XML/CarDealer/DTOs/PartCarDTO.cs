using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace CarDealer.DTOs
{
    [XmlType("partId")]
    public class PartCarDTO
    {
        [XmlAttribute("id")]
        public int Id { get; set; }
    }
}
