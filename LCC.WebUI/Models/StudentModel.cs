using LCC.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LCC.WebUI.Models
{
    public class StudentModel
    {
        public IList< ListOfFee> listOfFee { get; set; }
        public ListOfFee listFee { get; set; }
        public FeeAmount feeAmount { get; set; }
        public Month month { get; set; }
        public IList<Month> monthList { get; set; }
        public IList<FeeAmount> feeAmountList { get; set; }
        public AcademicYear academicYear { get; set; }
        public FeeMonth feeMonth { get; set; }
        public IList<FeeMonth> feeMonthList { get; set; }

        public Scholarship scholarship { get; set; }
        public Dictionary<int ,string> genderDic { get; set; }
        public Gender gender { get; set; }
        public List<Student> studentList { get; set; }
        public IList<User> userList { get; set; }
        public User user { get; set; }
        public IList<PayableInDetail> payableInDetailList { get; set; }
        public PayableInDetail payableInDetail { get; set; }
        public IList<TotalPayable> totalPayList { get; set; }
        public TotalPayable totalPayable { get; set; }
        public Student student { get; set; }
        public Classe classe { get; set; }
        public List< Classe> classeList { get; set; }
        public StudentCategory studentCategory { get; set; }

        public List<StudentCategory> studentCategoryList { get; set; }
        public PaymentConfirmation paymentConfirmation { get; set; }
        public string printS { get; set; }
        public string paymentMonth { get; set; }
        public string userId { get; set; }
        public int feeListCount{get;set; }
       public string[,] st = new string[200, 10];


    }
}