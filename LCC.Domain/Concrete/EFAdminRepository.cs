using LCC.Domain.Abstract;
using LCC.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LCC.Domain.Concrete
{
   public class EFAdminRepository : IAdminRepository
    {
       private readonly EFDbContext context =new EFDbContext();
       public IEnumerable<AcademicYear> AcademicYears
       {
           get { return context.AcademicYears; }
       }
       public IEnumerable<Scholarship> Scholarships
       {
           get { return context.Scholarships; }
       }
      
       public void UpdateAcademicYear(AcademicYear cls)
       {
           if(cls.Id==0)
           {
               context.AcademicYears.Add(cls);
               context.SaveChanges();
           }
           else
           {
               AcademicYear ucls = context.AcademicYears.Find(cls.Id);
              ucls.ClassId = cls.ClassId;
              ucls.Year = cls.Year;
              ucls.FirstDay = cls.FirstDay;
              ucls.LastDay = cls.LastDay;

           }
           
       }

       public void UpdateScholarship(Scholarship sc)
       {
           if (sc.Id == 0)
           {
               context.Scholarships.Add(sc);
               context.SaveChanges();
           }
           else
           {
               Scholarship sch = context.Scholarships.Find(sc.Id);
               sch.Amount = sc.Amount;
               context.SaveChanges();

           }

       }
    }
}
