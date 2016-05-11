using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCC.Domain.Entities
{
   
   public class PurchaseList
    {
        public int Id { get; set; }
        public int ItemId { get; set; }

        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
     
       [DataType(DataType.Date)]
        public DateTime? PurchaseDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime? EntryDate { get; set; }
    }
}
