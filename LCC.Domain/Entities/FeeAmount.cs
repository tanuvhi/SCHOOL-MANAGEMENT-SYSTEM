using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCC.Domain.Entities
{
   public class FeeAmount
    {

       [Key]
       public int Id { get; set; }
        [Required]
       public int ClassId { get; set; }

       [Required]
       [Display(Name = "Student Category")]
       public int  StudentCategoryId {get;set;}

       public int FeeId { get; set; }
       
    
       public decimal Amount { get; set; }

      

    }
}
