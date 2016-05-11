using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCC.Domain.Entities
{
   public class Teacher
    {
       [Key]
        public string UserId { get; set; }
        public decimal Salary { get; set; }
    }
}
