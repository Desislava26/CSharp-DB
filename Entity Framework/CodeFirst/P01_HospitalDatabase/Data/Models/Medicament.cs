using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P01_HospitalDatabase.Data.Models
{
    public class Medicament
    {
        [Key]
        public int MedicamentId { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string Name { get; set; }

        public ICollection<PatientMedicament> Prescriptions { get; set; } = new HashSet<PatientMedicament>();
    }
}
