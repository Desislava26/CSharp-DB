using MusicHub.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicHub.Data.Models
{
    public class Song
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Name { get; set; }
        [Required]
        public TimeSpan Duration { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }
        
        public Genre Genre { get; set; }

        public int? AlbumId { get; set; }

        public Album? Album { get; set; }

        public int WriterId { get; set; }

        public Writer Writer { get; set; }

        [Required]
        public decimal Price { get; set; }

        public ICollection<SongPerformer> SongPerformers { get; set; }   = new List<SongPerformer>();
        public ICollection<Performer> Performers { get; set; } = new List<Performer>();

    }
}
