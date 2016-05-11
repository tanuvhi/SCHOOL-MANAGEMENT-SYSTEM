using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCC.Domain.Entities
{
   public class Scholarship
    {
       [Key]
        public int Id { get; set; }
       public string StudentId { get; set; }

       public decimal Amount { get; set; }
       public int AcademicYearId { get; set; }

    }
}
