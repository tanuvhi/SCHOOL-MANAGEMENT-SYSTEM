using LCC.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCC.Domain.Abstract
{
  public  interface IStudentRepository
    {
      IEnumerable<FeeAmount> FeeAmounts { get; }
      IEnumerable<Student> Students { get; }
      void UpdateListOfFees(ListOfFee feeamount);
      IEnumerable<ListOfFee> ListOfFees { get; }
      void UpdateFeeAmount(FeeAmount feeamount);
      void UpdateStudentInfo(Student student);
      IEnumerable<StudentCategory> StudentCategorys { get; }
      IEnumerable<Classe> Classes { get; }
      IEnumerable<Section> Sections { get; }
      IEnumerable<PayableInDetail> PayableInDetails { get; }
      IEnumerable<TotalPayable> TotalPayables { get; }
       IEnumerable<FeeMonth> FeeMonths { get; }
       IEnumerable<Month> Months { get; }
       IEnumerable<PaymentConfirmation> PaymentConfirmations { get; }
       void UpdateFeeMonth(FeeMonth fm);
       int UpdateTotalPayable(TotalPayable tp);
       int UpdatePayableDeatails(PayableInDetail pay);

       int UpdatePaymentConfirmations(PaymentConfirmation pc);
      void RemoveTotalPayable(int tp);
      void RemovePayableDetails(int tp);
      
    }
}
