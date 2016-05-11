using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCC.Domain.Entities
{
   public class AcademicYear
    {
       [Key]
        public int Id { get; set; }
        public int Year { get; set; }
       [Required]
       [Display(Name = "Class")]
        public int ClassId { get; set; }
       [Required]
       [Display(Name = "Class Start Date")]
       public DateTime? FirstDay { get; set; }
       [Required]
       [Display(Name = "Class End Date ")]
       
       [DataType(DataType.Date)]
       [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
       public DateTime? LastDay { get; set; }
   }
}
