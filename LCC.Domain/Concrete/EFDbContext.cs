
using LCC.Domain.Entities;

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCC.Domain.Concrete
{
    public class EFDbContext : DbContext
    {
       public DbSet<User> Users{get; set;}
       public DbSet<Gender> Genders { get; set; }

       public DbSet<Student> Students { get; set; }
       public DbSet<Teacher> Teachers { get; set; }
       public DbSet<ListOfFee> ListOfFees { get; set; }
       public DbSet<UserStatus> UserStatus { get; set; }
       public DbSet<FeeAmount> FeeAmounts { get; set; }
       public DbSet<StudentCategory> StudentCategorys { get; set; }
       public DbSet<Classe> Classes { get; set; }
       public DbSet<Section> Sections { get; set; }
       public DbSet<PayableInDetail> PayableInDetails { get; set; }
       public DbSet<FeeMonth> FeeMonths { get; set; }
       public DbSet<AcademicYear> AcademicYears { get; set; }
       public DbSet<Scholarship> Scholarships { get; set; }
       public DbSet<TotalPayable> TotalPayables { get; set; }
       public DbSet<Month> Months { get; set; }
       public DbSet<PaymentConfirmation> PaymentConfirmations { get; set; }

       public DbSet<ItemList> ItemLists { get; set; }
       public DbSet<PurchaseList> PurchaseLists { get; set; }
       public DbSet<QuantityType> QuantityTypes { get; set; }

        }
}

