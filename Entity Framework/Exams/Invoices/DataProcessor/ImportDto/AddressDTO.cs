using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Invoices.DataProcessor.ImportDto
{
    [XmlType("Address")]
    public class AddressDTO
    {
        [XmlElement("StreetName")]
        [Required]
        [MaxLength(10)]
        [MinLength(20)]
        public string? StreetName { get; set; }
        [XmlElement("StreetNumber")]
        public int StreetNumber { get; set; }
        [XmlElement("PostCode")]
        public string? PostCode { get; set; }
        [XmlElement("City")]
        [Required]
        [MinLength(5)]
        [MaxLength(15)]
        public string? City { get; set; }
        [XmlElement("Country")]
        [Required]
        [MinLength(5)]
        [MaxLength(15)]
        public string? Country { get; set; }
    }
}
        //< StreetName > Gewerbestrasse </ StreetName >
        //< StreetNumber > 12 </ StreetNumber >
        //< PostCode > 5165 </ PostCode >
        //< City > Berndorf bei Salzburg</City>
        //<Country>Austria</Country>