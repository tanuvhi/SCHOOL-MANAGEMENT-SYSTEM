using LCC.Domain.Abstract;
using LCC.Domain.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCC.Domain.Concrete
{
    public class EFStudenRepository : IStudentRepository
    {
        private readonly EFDbContext context = new EFDbContext();

        public IEnumerable<Student> Students
        {
            get { return context.Students; }
        }
        public IEnumerable<ListOfFee> ListOfFees
        {
            get { return context.ListOfFees; }
        }
        public IEnumerable<FeeAmount> FeeAmounts
        {
            get { return context.FeeAmounts; }
        }
        public IEnumerable<Month> Months
        {
            get { return context.Months; }
        }

        public IEnumerable<PaymentConfirmation> PaymentConfirmations
        {
            get { return context.PaymentConfirmations; }
        }

        public void UpdateListOfFees(ListOfFee listoffee)
        {

            if (listoffee.Id == 0)
            {

                context.ListOfFees.Add(listoffee);
                context.SaveChanges();


            }
            else
            {
                ListOfFee fees = context.ListOfFees.Find(listoffee.Id);
                fees.Name = listoffee.Name;
                context.SaveChanges();

            }

        }

        public void UpdateFeeAmount(FeeAmount feeamount)
        {
            if (feeamount.Id == 0)
            {
                context.FeeAmounts.Add(feeamount);


            }
            else
            {
                FeeAmount fee = context.FeeAmounts.Find(feeamount.Id);

            }
            context.SaveChanges();

        }
        public IEnumerable<StudentCategory> StudentCategorys
        {
            get { return context.StudentCategorys; }
        }
        public IEnumerable<TotalPayable> TotalPayables
        {
            get { return context.TotalPayables; }
        }


        public IEnumerable<Classe> Classes
        {
            get { return context.Classes; }
        }

        public IEnumerable<Section> Sections
        {
            get { return context.Sections; }
        }

        public IEnumerable<PayableInDetail> PayableInDetails
        {
            get { return context.PayableInDetails; }
        }
        public IEnumerable<FeeMonth> FeeMonths
        {
            get { return context.FeeMonths; }
        }
        public void UpdateStudentInfo(Student student)
        {
            if (student.Id == 0)
            {
                context.Students.Add(student);
                context.SaveChanges();
            }
            else
            {
                Student stu = context.Students.Find(student.Id);
                stu.AcademicYearId = student.AcademicYearId;
                stu.ClassId = student.ClassId;
                stu.FirstDayOfClass = student.FirstDayOfClass;
                stu.StudentCategoryId = student.StudentCategoryId;
                stu.UserId = student.UserId;
                stu.SectionId = student.SectionId;
                context.SaveChanges();

            }
        }

        public void UpdateFeeMonth(FeeMonth fm)
        {
            if (fm.Id == 0)
            {
                context.FeeMonths.Add(fm);
                context.SaveChanges();
            }
            else
            {
                FeeMonth feeM = context.FeeMonths.Find(fm.Id);
                feeM.Id = fm.Id;
                feeM.StudentCategoryId = fm.StudentCategoryId;
                feeM.ClassId = fm.ClassId;
                feeM.January = fm.January;
                feeM.February = fm.February;
                feeM.March = fm.March;
                feeM.April = fm.April;
                feeM.May = fm.May;
                feeM.June = fm.June;
                feeM.July = fm.July;
                feeM.Augest = fm.Augest;
                feeM.September = fm.September;
                feeM.October = fm.October;
                feeM.November = fm.November;
                feeM.December = fm.December;
                context.SaveChanges();
            }
        }
        public int UpdateTotalPayable(TotalPayable tp)
        {
            if (tp.Id == 0)
            {
                context.TotalPayables.Add(tp);
                context.SaveChanges();
                return tp.Id;
            }
            else
            {
                TotalPayable ttp = new TotalPayable();
                ttp = context.TotalPayables.Find(tp.Id);
                ttp.Month = tp.Month;
                ttp.NetTotal = tp.NetTotal;
                ttp.PrevBalance = tp.PrevBalance;
                ttp.StudentId = tp.StudentId;
                ttp.AcademicYearId = tp.AcademicYearId;
                ttp.Amount = tp.Amount;
                ttp.Balance = tp.Balance;
                ttp.AmountPaid = tp.AmountPaid;
                ttp.Deduction = tp.Deduction;
                context.SaveChanges();
                return ttp.Id;
            }
        }
        public int UpdatePayableDeatails(PayableInDetail pay)
        {
            if (pay.Id == 0)
            {
                context.PayableInDetails.Add(pay);
                context.SaveChanges();
                return pay.Id;
            }
            else
            {
                PayableInDetail paya = new PayableInDetail();
                paya = context.PayableInDetails.Find(paya.Id);
                context.SaveChanges();
                return paya.Id;
            }

        }
        public int UpdatePaymentConfirmations(PaymentConfirmation pc)
        {
            if (pc.Id == 0)
            {
                context.PaymentConfirmations.Add(pc);
                context.SaveChanges();
                return pc.Id;
            }
            else
            {

                PaymentConfirmation pyc = new PaymentConfirmation();
                pyc = context.PaymentConfirmations.Find(pc.Id);
                pyc.PaymentDate = pc.PaymentDate;
                pyc.PrintOutDate = pc.PrintOutDate;
                pyc.QRCode = pc.QRCode;
                pyc.TotalPayableId = pc.TotalPayableId;
                pyc.ConfirmBy = pc.ConfirmBy;
                pyc.AmountPaid = pc.AmountPaid;
                context.SaveChanges();
                return pyc.Id;
            }

        }

        public void RemoveTotalPayable(int Id)
        {

            TotalPayable t = context.TotalPayables.Find(Id);
            if (t != null)
            {
                context.TotalPayables.Remove(t);
                context.SaveChanges();
            }

        }


        public void RemovePayableDetails(int Id)
       {

           PayableInDetail t = context.PayableInDetails.Find(Id);
           if (t != null)
           {
               context.PayableInDetails.Remove(t);
               context.SaveChanges();
           }

       }
    }
}