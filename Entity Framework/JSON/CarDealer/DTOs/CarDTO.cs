using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDealer.DTOs
{
    public class CarDTO
    {
        public string Make { get; set; }
        public string Model { get; set; }
        public int TraveledDistance { get; set; }
        public IEnumerable<int> PartsId { get; set; }
    }
}
