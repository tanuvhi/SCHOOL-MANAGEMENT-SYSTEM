using LCC.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LCC.Domain.Abstract
{
  public  interface IAdminRepository
    {
      IEnumerable<AcademicYear> AcademicYears { get; }
      IEnumerable<Scholarship> Scholarships { get; }

      void UpdateAcademicYear(AcademicYear Ac);
      void UpdateScholarship(Scholarship sc);
    }
}
