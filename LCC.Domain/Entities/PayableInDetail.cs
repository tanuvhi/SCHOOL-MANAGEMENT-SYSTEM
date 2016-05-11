using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCC.Domain.Entities
{
   public class PayableInDetail
    {
       [Key]
        public int Id { get; set; }
       public int TotalPayableId { get; set; }
       public int FeeId { get; set; }
       public decimal Amount { get; set; }


    }
}
