using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCC.Domain.Entities
{
  public   class Student
  {
      public string UserId { get; set; }


      [Display(Name = "Class")]
      [Required]

      public int ClassId { get; set; }
      [Display(Name = "Section")]
      [Required]
      public int SectionId { get; set; }
      [Display(Name = "Student Category")]
      [Required]
      public int StudentCategoryId { get; set; }

      [Required]
      public int AcademicYearId { get; set; }
      [Required]
      [Display(Name = "First Day Of Class")]
      public DateTime? FirstDayOfClass { get; set; }      
      [Key]
      public int Id { get; set; }


    }
}
