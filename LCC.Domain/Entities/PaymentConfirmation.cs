using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCC.Domain.Entities
{
   public class PaymentConfirmation
    {
        [Key]
        public int Id { get; set; }
        public DateTime? PrintOutDate { get; set; }
       public int TotalPayableId { get; set; }
        public String QRCode { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string ConfirmBy { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal UnableToPay { get;set; }
        public int UnableToPayConfirm { get; set; }
    }
}
