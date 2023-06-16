using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;


namespace Invoices.DataProcessor.ImportDto
{
    [XmlType("Client")]
    public class CLientDTO
    {
        [Required]
        [XmlElement("Name")]
        [MinLength(10)]
        [MaxLength(25)]
        public string Name { get; set; } = null!;

        [Required]
        [XmlElement("NumberVat")]
        [MinLength(10)]
        [MaxLength(25)]
        public string NumberVat { get; set; } = null!;

        [XmlArray("Addresses")]
        public AddressDTO[] Addresses { get; set; } = null!;


    }
}
