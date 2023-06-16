using Castle.Components.DictionaryAdapter;
using Invoices.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeyAttribute = System.ComponentModel.DataAnnotations.KeyAttribute;

namespace Invoices.Data.Models
{
    public class Invoice
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Range(1000000000, 1500000000)]
        public long Number { get; set; }
        [Required]
        public DateTime IssueDate  { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        [Required]
        public decimal Amount { get; set; }

        public CurrencyType CurrencyType { get; set; }

        [Required]
        public int ClientId { get; set; }
        [ForeignKey(nameof(ClientId))]
        public Client? Client { get; set; }
    }
}
