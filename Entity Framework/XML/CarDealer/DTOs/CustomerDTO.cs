using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace CarDealer.DTOs
{
    [XmlType("Customer")]
    public class CustomerDTO
    {
        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("birthDate")]
        public string BirthDate { get; set; }
        
        [XmlElement("IsYoungDriver")]
        public bool IsYoungDriver { get; set; }
    }
}
        //< name > Natalie Poli </ name >
        //< birthDate > 1990 - 10 - 04T00: 00:00 </ birthDate >
        //< isYoungDriver > false </ isYoungDriver >