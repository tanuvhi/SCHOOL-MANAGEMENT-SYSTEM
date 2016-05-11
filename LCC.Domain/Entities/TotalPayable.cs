using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCC.Domain.Entities
{
  public  class TotalPayable
    {
      [Key]
       public int  Id { get; set; }
      public string StudentId { get; set; }
      public int  Month { get; set; }

      public decimal Amount { get; set; }

      public decimal Deduction { get; set; }

      public decimal PrevBalance { get; set; }
      public decimal NetTotal { get; set; }
      public decimal AmountPaid { get; set; }
      public decimal Balance { get; set; }
      public int AcademicYearId { get; set; }



    }
}
