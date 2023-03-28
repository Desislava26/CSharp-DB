using MusicHub.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicHub.Data.Models
{
    public class Album
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(40)]
        public string Name { get; set; }
        [Required]
        public DateTime ReleaseDate { get; set; }

        //public decimal Price { get; set; }
        private decimal price;

        public decimal Price
        {
            get { return price; }
            set
            {
                foreach (var item in this.Songs)
                {
                    price+= item.Price;
                }
                price = value; 
            }
        }


        public int? ProducerId { get; set; }

        public Producer? Producer { get; set; }

        public ICollection<Song> Songs { get; set; } = new List<Song>();

    }
}
