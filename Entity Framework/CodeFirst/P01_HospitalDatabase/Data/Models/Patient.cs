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
    public class Patient
    {
        [Key]
        public int PatientId { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string FirstName { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string LastName { get; set; }
        [Column(TypeName = "nvarchar(250)")]
        public string Address { get; set; }

        [EmailAddressAttribute]
        [Column(TypeName = "varchar(80)")]
        public string Email { get; set; }
        public bool HasInsurance { get; set; }

        public ICollection<Visitation> Visitations { get; set; } = new HashSet<Visitation>();
        public ICollection<Diagnose> Diagnoses { get; set; } = new HashSet<Diagnose>();
        public ICollection<PatientMedicament> Prescriptions { get; set; } = new HashSet<PatientMedicament>();
    }
}
