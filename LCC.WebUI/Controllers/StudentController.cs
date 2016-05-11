using LCC.Domain.Abstract;
using LCC.Domain.Entities;
using LCC.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using QRCoder;
using System.Text.RegularExpressions;
using System.Web.Security;
using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;
using System.Threading.Tasks;
using System.Globalization;
using System.Threading;

namespace LCC.WebUI.Controllers
{
       [Authorize(Roles = "Admin,Accountant")]
    public class StudentController : Controller
    {
        //
        // GET: /StudentPayment/

        private readonly IStudentRepository stuRepository;
        private readonly IAdminRepository adminReository;
        private readonly IUserRepository userReository;
        public StudentController(IStudentRepository stuRepo, IAdminRepository ad, IUserRepository us)
        {
            var badCulture = new CultureInfo("");
            badCulture.DateTimeFormat.ShortDatePattern = "dd-MM-yyyy";
            Thread.CurrentThread.CurrentCulture = badCulture;
            stuRepository = stuRepo;
            adminReository = ad;
            userReository = us;


        }
        public ActionResult Index()
        {
            return View();
        }
          [Authorize(Roles = "Admin")]
        public ViewResult AdjustFeeAmount()
        {
            StudentModel model = new StudentModel();
            ViewBag.Class = new SelectList(stuRepository.Classes, "ClassId", "Name");
            ViewBag.StudentCategory = new SelectList(stuRepository.StudentCategorys, "Id", "Name");
            model.listOfFee = stuRepository.ListOfFees.Select(x => new ListOfFee { Id = x.Id, Name = x.Name, }).DefaultIfEmpty().ToList();

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult AdjustFeeAmount(StudentModel modeel)
        {
            modeel.listOfFee = stuRepository.ListOfFees.Select(x => new ListOfFee { Id = x.Id, Name = x.Name, }).DefaultIfEmpty().ToList();

            ViewBag.Class = new SelectList(stuRepository.Classes, "ClassId", "Name");
            ViewBag.StudentCategory = new SelectList(stuRepository.StudentCategorys, "Id", "Name");


            if (ModelState.IsValid)
            {

                var fee = stuRepository.FeeAmounts.Where(m => m.ClassId == modeel.classe.ClassId)
                                        .Where(dm => dm.StudentCategoryId == modeel.studentCategory.Id).ToList();
                string ClassName = stuRepository.Classes.Where(m => m.ClassId == modeel.classe.ClassId).Select(s => s.Name).FirstOrDefault();
                string StudentCategory = stuRepository.StudentCategorys.Where(m => m.Id == modeel.studentCategory.Id).Select(s => s.Name).FirstOrDefault();

                for (int i = 0; i < modeel.listOfFee.Count; i++)
                {
                    int d = 0;

                    foreach (var f in fee)
                    {

                        if (f.FeeId == modeel.listOfFee[i].Id)
                        {
                            d = 1;
                        }

                    }
                    if (d == 0)
                    {
                        modeel.feeAmountList[i].FeeId = modeel.listOfFee[i].Id;
                        modeel.feeAmountList[i].ClassId = modeel.classe.ClassId;
                        modeel.feeAmountList[i].StudentCategoryId = modeel.studentCategory.Id;

                        TempData["SuccessMessage"] = string.Format("Class {0} {1} student fee has been successfully adjusted for the year  ", ClassName, StudentCategory);
                        stuRepository.UpdateFeeAmount(modeel.feeAmountList[i]);

               
                    }
                    else
                    {

                        TempData["UnsuccessMessage"] = string.Format("Class {0} {1} student fee has been already adjusted, Now you can only edit", ClassName, StudentCategory);


                    }

                }





            }

            return View(modeel);
        }
               [Authorize(Roles = "Admin")]
        public ViewResult AddListOfFee()
        {


            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult AddListOfFee(ListOfFee listOfFee)
        {



            stuRepository.UpdateListOfFees(listOfFee);
            return View();
        }

        public ActionResult StudentInfo(string userId, string name)
        {
            StudentModel model = new StudentModel();

            ViewBag.Classe = new SelectList(stuRepository.Classes, "ClassId", "Name");
            ViewBag.StudentCatego = new SelectList(stuRepository.StudentCategorys, "Id", "Name");
            ViewBag.Section = new SelectList(stuRepository.Sections, "Id", "Name");
            ViewBag.AcademicYears = new SelectList(adminReository.AcademicYears.Select(p => p.Year).Distinct(), "Year");

            ViewBag.Name = name;

            model.userId = userId;


            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult StudentInfo(StudentModel model)
        {

            model.student.UserId = model.userId;

            ViewBag.Classe = new SelectList(stuRepository.Classes, "ClassId", "Name");
            ViewBag.StudentCatego = new SelectList(stuRepository.StudentCategorys, "Id", "Name");
            ViewBag.Section = new SelectList(stuRepository.Sections, "Id", "Name");
            ViewBag.AcademicYears = new SelectList(adminReository.AcademicYears.Select(p => p.Year).Distinct(), "Year");
           model.student.AcademicYearId = adminReository.AcademicYears.Where(m=>m.Year == model.student.AcademicYearId)
                                                                       .Where(m=>m.ClassId== model.student.ClassId)
                                                                       .Select(s=>s.Id)
                                                                       .FirstOrDefault();

           
            if (ModelState.IsValid)
            {



                var femonths = stuRepository.FeeMonths.Where(f => f.ClassId == model.student.ClassId)
                                                          .Where(f => f.StudentCategoryId == model.student.StudentCategoryId)
                                                          .Where(f=>f.February==true)
                                                          .Where(f=>f.January==true)
                                                          .Where(f=>f.March==true)
                                                          .Where(f=>f.April==true)
                                                          .Where(f=>f.May==true)
                                                          .Where(f=>f.June==true)
                                                          .Where(f=>f.July==true)
                                                          .Where(f=>f.Augest==true)
                                                          .Where(f=>f.September==true)
                                                          .Where(f=>f.October==true)
                                                          .Where(f=>f.November==true)
                                                          .Where(f=>f.December==true)
                                                          .ToArray();
                decimal amount = 0;
               foreach(var i in femonths)
                {
                    amount += stuRepository.FeeAmounts.Where(d => d.FeeId == i.FeeId)
                                                      .Where(d => d.ClassId == i.ClassId)
                                                      .Where(d => d.StudentCategoryId == i.StudentCategoryId)
                                                       .Select(s => s.Amount).FirstOrDefault();

                }

               model.scholarship.AcademicYearId = model.student.AcademicYearId;
               model.scholarship.Amount = amount - model.scholarship.Amount;
               model.scholarship.StudentId = model.student.UserId;



               adminReository.UpdateScholarship(model.scholarship);
                stuRepository.UpdateStudentInfo(model.student);

                int month = model.student.FirstDayOfClass.Value.Month;

                int mo = month;
                int monthNow = DateTime.Now.Date.Month;
                for (int j = month; j <= monthNow; j++)
                {
                    month = j;
                    int ttpl = stuRepository.TotalPayables.Where(p => p.Month == month)
                                                           .Where(p => p.AcademicYearId == model.student.AcademicYearId)
                                                           .Where(s => s.StudentId == model.userId)
                                                           .Select(s => s.Id)
                                                           .FirstOrDefault();
                   
                    if (ttpl == 0)
                    {

                        TotalPayable totalPay = new TotalPayable();
                        totalPay.Month = month;
                        totalPay.StudentId = model.userId;
                        totalPay.AcademicYearId = model.student.AcademicYearId;
                        ttpl = stuRepository.UpdateTotalPayable(totalPay);

                        totalPay.Id = ttpl;

                        totalPay.PrevBalance = 0;
                        totalPay.Amount = 0;
                        totalPay.Balance = 0;
                        totalPay.NetTotal = 0;
                        totalPay.AmountPaid = 0;
                        if (month - 1 < 1)
                        {
                            var stuYearList = stuRepository.Students.Where(s => s.UserId == model.userId)
                                                           .Select(S => S.AcademicYearId).ToList();
                            if (stuYearList.Count() > 1)
                            {
                                totalPay.PrevBalance = stuRepository.TotalPayables.Where(t => t.StudentId == model.userId)
                                                                         .Where(t => t.AcademicYearId == stuYearList[stuYearList.Count() - 2])
                                                                         .Where(s => s.Month == 12)
                                                                         .Select(s => s.Balance)
                                                                         .FirstOrDefault();
                            }
                        }
                        else
                        {
                            totalPay.PrevBalance = stuRepository.TotalPayables.Where(t => t.StudentId == model.userId)
                                                                         .Where(t => t.AcademicYearId == model.student.AcademicYearId)
                                                                         .Where(s => s.Month == (month - 1))
                                                                         .Select(s => s.Balance)
                                                                         .FirstOrDefault();

                        }
                        totalPay.Deduction = adminReository.Scholarships.Where(s => s.AcademicYearId == model.student.AcademicYearId)
                                                                         .Where(s => s.StudentId == model.userId)
                                                                         .Select(s => s.Amount).FirstOrDefault();
                        totalPay.NetTotal += totalPay.PrevBalance;
                        totalPay.NetTotal -= totalPay.Deduction;
                        var femonth = stuRepository.FeeMonths.Where(f => f.ClassId == model.student.ClassId)
                                                             .Where(f => f.StudentCategoryId == model.student.StudentCategoryId)
                                                             .ToArray();




                        for (int i = 0; i < femonth.Count(); i++)
                        {

                            bool dd = false;
                            if (femonth[i].January == true && month == 1)
                            {
                                dd = true;
                            }
                            else if (femonth[i].February == true && month == 2)
                            {
                                dd = true;
                            }
                            else if (femonth[i].March == true && month == 3)
                            {
                                dd = true;
                            }
                            else if (femonth[i].April == true && month == 4)
                            {
                                dd = true;

                            }
                            else if (femonth[i].May == true && month == 5)
                            {
                                dd = true;
                            }
                            else if (femonth[i].June == true && month == 6)
                            {
                                dd = true;

                            }
                            else if (femonth[i].July == true && month == 7)
                            {
                                dd = true;
                            }
                            else if (femonth[i].Augest == true && month == 8)
                            {
                                dd = true;
                            }

                            else if (femonth[i].September == true && month == 9)
                            {
                                dd = true;
                            }
                            else if (femonth[i].October == true && month == 10)
                            {
                                dd = true;
                            }
                            else if (femonth[i].November == true && month == 11)
                            {
                                dd = true;
                            }
                            else if (femonth[i].December == true && month == 12)
                            {
                                dd = true;
                            }
                            else if (femonth[i].yearly == true && j==mo )
                            {
                                dd = true;
                            }
                            if (dd == true)
                            {
                                FeeAmount feeAmount = stuRepository.FeeAmounts.Where(f => f.FeeId == femonth[i].FeeId)
                                                                              .Where(f => f.ClassId == model.student.ClassId)
                                                                              .Where(f => f.StudentCategoryId == model.student.StudentCategoryId)
                                                                              .FirstOrDefault();
                                if (feeAmount != null)
                                {
                                    PayableInDetail payDeta = new PayableInDetail();
                                    payDeta.FeeId = feeAmount.FeeId;
                                    payDeta.TotalPayableId = ttpl;
                                    payDeta.Amount = feeAmount.Amount;
                                    totalPay.Amount += feeAmount.Amount;
                                    stuRepository.UpdatePayableDeatails(payDeta);
                                }

                            }
                        }
                        totalPay.NetTotal += totalPay.Amount;
                        totalPay.Balance = totalPay.NetTotal;
                        stuRepository.UpdateTotalPayable(totalPay);
                        TempData["SuccessMessage"] = string.Format("Student information added successful");


                    }
                    else
                        TempData["UnsuccessMessage"] = string.Format("This student information already added");

                }


            }
            return RedirectToAction("Register", "User");
        }

         [Authorize(Roles = "Admin")]
        public ViewResult SelectMonthForFee()
        {

            ViewBag.Class = new SelectList(stuRepository.Classes, "ClassId", "Name");
            ViewBag.StudentCategory = new SelectList(stuRepository.StudentCategorys, "Id", "Name");
            ViewBag.Fee = new SelectList(stuRepository.ListOfFees, "Id", "Name");

            return View();

        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult SelectMonthForFee(StudentModel model)
        {

            ViewBag.Class = new SelectList(stuRepository.Classes, "ClassId", "Name");
            ViewBag.StudentCategory = new SelectList(stuRepository.StudentCategorys, "Id", "Name");
            ViewBag.Fee = new SelectList(stuRepository.ListOfFees, "Id", "Name");
            model.feeMonth.ClassId = model.classe.ClassId;
            model.feeMonth.StudentCategoryId = model.studentCategory.Id;
            model.feeMonth.FeeId = model.listFee.Id;
            var f = stuRepository.FeeMonths.Where(M => M.ClassId == model.feeMonth.ClassId)
                                            .Where(M => M.FeeId == model.feeMonth.FeeId)
                                            .Where(M => M.StudentCategoryId == model.studentCategory.Id)
                                            .FirstOrDefault();
            string ClassName = stuRepository.Classes.Where(m => m.ClassId == model.classe.ClassId).Select(s => s.Name).FirstOrDefault();
            string StudentCategory = stuRepository.StudentCategorys.Where(m => m.Id == model.studentCategory.Id).Select(s => s.Name).FirstOrDefault();


            if (ModelState.IsValid)
            {

                if (f == null)
                {
                    TempData["SuccessMessage"] = string.Format("successfully ");
                    stuRepository.UpdateFeeMonth(model.feeMonth);

                }
                else
                {

                    TempData["UnsuccessMessage"] = string.Format("Class {0} {1}  fee month has been already adjusted, Now you can only edit", ClassName, StudentCategory);


                }



            }
            return View();

        }


        public ActionResult CheakName(string categoryId, string classId, string yearId)
        {
        

            int cId = Int32.Parse(categoryId);
            int clId = Int32.Parse(classId);
            int yId = Int32.Parse(yearId);
            yId = adminReository.AcademicYears.Where(s => s.Year == yId)
                                                .Where(s => s.ClassId == clId)
                                                .Select(s => s.Id)
                                                 .FirstOrDefault();
        
            var tpay = stuRepository.TotalPayables.Where(m => m.Month == DateTime.Now.Month).ToList();

            var query =
                       stuRepository.Students.Where(s => s.StudentCategoryId == cId)
                                              .Where(s => s.ClassId == clId)
                                             .Where(s => s.AcademicYearId == yId)
                                            .Join(stuRepository.Classes, s => s.ClassId, c => c.ClassId, (s, c) => new { s, c })
                                           .Join(adminReository.AcademicYears, y => y.s.AcademicYearId, ye => ye.Id, (y, ye) => new { y, ye })
                                           .Join(stuRepository.StudentCategorys, sat => sat.y.s.StudentCategoryId, cat => cat.Id, (sat, cat) => new { sat, cat })
                                           .Join(userReository.Users, u => u.sat.y.s.UserId, us => us.UserId, (u, us) => new { u, us })
                                           .Join(tpay, t => t.us.UserId, tp => tp.StudentId, (t, tp) => new { t, tp })
                                           .Select(m => new
                                           {
                                               StId = m.tp.StudentId,
                                               Name = m.t.us.Name,
                                               FatherName = m.t.us.FatherName,
                                               GuardianPhone = m.t.us.GuardianPhone,
                                               Year = m.t.u.sat.ye.Year,
                                               Category = m.t.u.cat.Name,
                                               Classe = m.t.u.cat.Name,
                                               Due = m.tp.Balance,
                                               YID = m.t.u.sat.ye.Id
                                           });

            return Json(query, JsonRequestBehavior.DenyGet);
        }
         
        public ViewResult StudentPayment()
        {
            var Classe = stuRepository.Classes.ToList();
             var StudentCategory = stuRepository.StudentCategorys.ToList();
            ViewBag.Class = new SelectList(Classe, "ClassId", "Name");
            ViewBag.StudentCategory = new SelectList(StudentCategory, "Id", "Name");
            ViewBag.Year = new SelectList(adminReository.AcademicYears.Select(p => p.Year).Distinct(), "Year");        
    
            return View();
        }
        [HttpPost]
        public ActionResult StudentPayment(StudentModel model)
        {
            var Classe = stuRepository.Classes.ToList();
            var StudentCategory = stuRepository.StudentCategorys.ToList();


            ViewBag.Class = new SelectList(Classe, "ClassId", "Name");
            ViewBag.StudentCategory = new SelectList(StudentCategory, "Id", "Name");
            ViewBag.Year = new SelectList(adminReository.AcademicYears.Select(p => p.Year).Distinct(), "Year");
            return View();
        }
        public ActionResult RowClick(string StudnetId, int Acid, int Months)
        {

            return RedirectToAction("AStudentPayment", "Student", new { StudnetId = StudnetId, Acid = Acid, Months = Months });
        }

     
        public JsonResult GetFee(string totalId)
        {
            int d = Int32.Parse(totalId);
            StudentModel model = new StudentModel();
            model.listOfFee = stuRepository.ListOfFees.ToList();
            model.payableInDetailList = stuRepository.PayableInDetails.Where(s => s.Id == d).ToList();
            var query = from l in model.listOfFee
                        from r in model.payableInDetailList

                        select new
                        {
                            fee = l.Name,
                            amount = r.Amount.ToString()
                        };
       
            return Json(query, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SetQRCode(int totalId, decimal AmountPaid, string QrCode,decimal UnableToPay)
        {
            PaymentConfirmation pc = new PaymentConfirmation();
            pc.TotalPayableId = totalId;
            pc.ConfirmBy = "nu";
            pc.AmountPaid = AmountPaid;
            pc.PrintOutDate = DateTime.Now;
            pc.QRCode = Session["QrCode"].ToString();
            pc.UnableToPay = UnableToPay;
            pc.UnableToPayConfirm = 1;// 1 means pending
            int PayId = stuRepository.UpdatePaymentConfirmations(pc);
            return Json(PayId, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetQRCode(int totalId, decimal AmountPaid)
        {

            string data = Membership.GeneratePassword(15, 0);
            data = Regex.Replace(data, @"[^A-Z0-9]", m => "9");
           Session["QrCode"] = data;
            QrEncoder qrEncoder = new QrEncoder(ErrorCorrectionLevel.H);
            QrCode qrCode = new QrCode();
            qrEncoder.TryEncode(data, out qrCode);
            GraphicsRenderer renderer = new GraphicsRenderer(new FixedModuleSize(4, QuietZoneModules.Four), Brushes.Black, Brushes.White);

            Stream memoryStream = new MemoryStream();
            renderer.WriteToStream(qrCode.Matrix, ImageFormat.Png, memoryStream);

            // very important to reset memory stream to a starting position, otherwise you would get 0 bytes returned
            memoryStream.Position = 0;

            var resultStream = new FileStreamResult(memoryStream, "image/png");
            resultStream.FileDownloadName = String.Format("{0}.png", data);

            return resultStream;

        }

        [HttpPost]
        public ActionResult GetMonthlyFee(string StudnetId, string Acid, string Months)
        {
            return RedirectToAction("AStudentPayment", "Student", new { StudnetId = StudnetId, Acid = Acid, Months = Months });
        }
   
        public ViewResult AStudentPayment(string StudnetId, string Acid, string Months)
        {            
            
            int month = Convert.ToInt32(Months);
            int ACID = Convert.ToInt32(Acid);
            Session["PayId"] = 1;

            if (month > 12)
                month = DateTime.Now.Date.Month;
            int yid = ACID;

            if (ACID > 2013)
            {
                yid = adminReository.AcademicYears.Where(a => a.Year ==  ACID)
                                                                          
                    .Select(s => s.Id).FirstOrDefault();
            }

            StudentModel model = new StudentModel();
          

            model.totalPayable = stuRepository.TotalPayables
                                                .Where(t => t.StudentId == StudnetId)
                                                .Where(s => s.AcademicYearId == yid)
                                                .Where(t => t.Month == month)
                                                .FirstOrDefault();
            model.student = stuRepository.Students.Where(s => s.UserId == StudnetId)
                                                 .Where(s => s.AcademicYearId == yid).FirstOrDefault();
            model.classe = stuRepository.Classes.Where(c => c.ClassId == model.student.ClassId)
                                                .FirstOrDefault();
            model.studentCategory = stuRepository.StudentCategorys.Where(s => s.Id == model.student.StudentCategoryId).FirstOrDefault();

            model.user = userReository.Users.Where(s => s.UserId == StudnetId).FirstOrDefault();
            if (model.totalPayable != null)
            {
                model.payableInDetailList = stuRepository.PayableInDetails.Where(s => s.TotalPayableId == model.totalPayable.Id)
                                                                           .Where(m => m.Amount != 0).ToList();
                model.totalPayList = stuRepository.TotalPayables.Where(a => a.AcademicYearId == yid)
                                                .Where(s => s.StudentId == StudnetId)
                                                .ToList();
            }
            model.monthList = stuRepository.Months.ToList();
            var dueMonth = from ml in model.monthList
                           from tp in model.totalPayList
                           where (ml.Id == tp.Month)
                           select new
                           {
                               Id = ml.Id,
                               Name = ml.Name
                           };

            model.listOfFee = stuRepository.ListOfFees.ToList();

            model.month = stuRepository.Months.Where(m => m.Id == month).FirstOrDefault();
            ViewBag.Months = new SelectList(dueMonth, "Id", "Name");
            ViewBag.std = StudnetId;

            ViewBag.acid = ACID;
            foreach (var d in dueMonth)
            {
                ViewBag.LastMonth = d.Name;
            }
            return View(model);
        }
   
           [AllowAnonymous]
           [HttpPost]
        public ActionResult UpdateFee()
        {
            var dateTimeNow = DateTime.Now;
            var date = dateTimeNow.Date;
            var month = dateTimeNow.Month;
            var year = adminReository.AcademicYears.Where(y => y.FirstDay <= date)
                                                      .Where(y => y.LastDay >= date)                                                      
                                                      .ToList();
            foreach (var ye in year)
            {
                int yearId = ye.Id;
                IEnumerable<Student> student = stuRepository.Students.Where(s => s.AcademicYearId == yearId)
                                                      .ToList();

                var d = month;
                foreach (var stu in student)
                {
                    int mo = stu.FirstDayOfClass.Value.Month;
                    int ttpl = stuRepository.TotalPayables.Where(p => p.Month == month)
                                                           .Where(p => p.AcademicYearId == yearId)
                                                           .Where(s => s.StudentId == stu.UserId)
                                                           .Select(s => s.Id)
                                                           .FirstOrDefault();
                    if (ttpl == 0)
                    {

                        TotalPayable totalPay = new TotalPayable();
                        totalPay.Month = month;
                        totalPay.StudentId = stu.UserId;
                        totalPay.AcademicYearId = yearId;
                        ttpl = stuRepository.UpdateTotalPayable(totalPay);

                        totalPay.Id = ttpl;

                        totalPay.PrevBalance = 0;
                        totalPay.Amount = 0;
                        totalPay.Balance = 0;
                        totalPay.NetTotal = 0;
                        totalPay.AmountPaid = 0;
                        if (month - 1 < 1)
                        {
                            var stuYearList = stuRepository.Students.Where(s => s.UserId == stu.UserId)
                                                           .Select(S => S.AcademicYearId).ToList();
                            if (stuYearList.Count() > 1)
                            {
                                totalPay.PrevBalance = stuRepository.TotalPayables.Where(t => t.StudentId == stu.UserId)
                                                                         .Where(t => t.AcademicYearId == stuYearList[stuYearList.Count() - 2])
                                                                         .Where(s => s.Month == 12)
                                                                         .Select(s => s.Balance)
                                                                         .FirstOrDefault();
                            }
                        }
                        else
                        {
                            totalPay.PrevBalance = stuRepository.TotalPayables.Where(t => t.StudentId == stu.UserId)
                                                                         .Where(t => t.AcademicYearId == yearId)
                                                                         .Where(s => s.Month == (month - 1))
                                                                         .Select(s => s.Balance)
                                                                         .FirstOrDefault();

                        }
                        totalPay.Deduction = adminReository.Scholarships.Where(s => s.AcademicYearId == yearId)
                                                                         .Where(s => s.StudentId == stu.UserId)
                                                                         .Select(s => s.Amount).FirstOrDefault();
                        totalPay.NetTotal += totalPay.PrevBalance;
                        totalPay.NetTotal -= totalPay.Deduction;
                        var femonth = stuRepository.FeeMonths.Where(f => f.ClassId == stu.ClassId)
                                                             .Where(f => f.StudentCategoryId == stu.StudentCategoryId)
                                                             .ToArray();

                        for (int i = 0; i < femonth.Count(); i++)
                        {

                            bool dd = false;
                            if (femonth[i].January == true && month == 1)
                            {
                                dd = true;
                            }
                            else if (femonth[i].February == true && month == 2)
                            {
                                dd = true;
                            }
                            else if (femonth[i].March == true && month == 3)
                            {
                                dd = true;
                            }
                            else if (femonth[i].April == true && month == 4)
                            {
                                dd = true;

                            }
                            else if (femonth[i].May == true && month == 5)
                            {
                                dd = true;
                            }
                            else if (femonth[i].June == true && month == 6)
                            {
                                dd = true;

                            }
                            else if (femonth[i].July == true && month == 7)
                            {
                                dd = true;
                            }
                            else if (femonth[i].Augest == true && month == 8)
                            {
                                dd = true;
                            }

                            else if (femonth[i].September == true && month == 9)
                            {
                                dd = true;
                            }
                            else if (femonth[i].October == true && month == 10)
                            {
                                dd = true;
                            }
                            else if (femonth[i].November == true && month == 11)
                            {
                                dd = true;
                            }
                            else if (femonth[i].December == true && month == 12)
                            {
                                dd = true;
                            }
                            else if (femonth[i].yearly == true && DateTime.Now.Date.Month == mo)
                            {
                                dd = true;
                            }
                            if (dd == true)
                            {
                                FeeAmount feeAmount = stuRepository.FeeAmounts.Where(f => f.FeeId == femonth[i].FeeId)
                                                                              .Where(f => f.ClassId == stu.ClassId)
                                                                              .Where(f => f.StudentCategoryId == stu.StudentCategoryId)
                                                                              .FirstOrDefault();
                                if (feeAmount != null)
                                {
                                    PayableInDetail payDeta = new PayableInDetail();
                                    payDeta.FeeId = feeAmount.FeeId;
                                    payDeta.TotalPayableId = ttpl;
                                    payDeta.Amount = feeAmount.Amount;
                                    totalPay.Amount += feeAmount.Amount;
                                    stuRepository.UpdatePayableDeatails(payDeta);
                                }

                            }
                        }
                        totalPay.NetTotal += totalPay.Amount;

                        totalPay.Balance = totalPay.NetTotal;
                        stuRepository.UpdateTotalPayable(totalPay);

                    }

                }
            }
            TempData["SuccessMessage"] = string.Format("All students fee update successfully");
            return RedirectToAction("StudentPayment");
        }

               [Authorize(Roles = "Admin")]
        public ViewResult UpdateMonthFee()
        {
            StudentModel model = new StudentModel();
            ViewBag.Class = new SelectList(stuRepository.Classes, "ClassId", "Name");
            ViewBag.StudentCategory = new SelectList(stuRepository.StudentCategorys, "Id", "Name");

            model.feeMonthList = stuRepository.FeeMonths.ToList();
            model.listOfFee = stuRepository.ListOfFees.ToList();
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult UpdateMonthFee(StudentModel model)
        {

            return View(model);
        }

               [Authorize(Roles = "Admin")]
        public JsonResult GetFeeMonth(string categoryId, string classId)
        {


            var Fee = stuRepository.ListOfFees.ToList();

            int cId = Int32.Parse(categoryId);
            int clId = Int32.Parse(classId);

            var month = stuRepository.FeeMonths
                          .Where(s => s.ClassId == clId)
                          .Where(s => s.StudentCategoryId == cId)
                          .ToList();

            var query =
                         from m in month
                         from f in Fee
                         where (m.FeeId == f.Id)
                         select new
                         {

                             Name = f.Name,
                             Yea = m.yearly,
                             Jan = m.January,
                             Feb = m.February,
                             Mar = m.March,
                             Apr = m.April,
                             May = m.May,
                             Jun = m.June,
                             Jul = m.July,
                             Aug = m.Augest,
                             Sep = m.September,
                             Oct = m.October,
                             Nov = m.November,
                             Dec = m.December


                         };


            return Json(query, JsonRequestBehavior.AllowGet);
        }
               public ViewResult PaymentConfirmation1()
               {


                   return View();

               }
           [HttpPost]
               public ActionResult PaymentConfirmation1(PaymentConfirmation model)
               {


                  return RedirectToAction("PaymentConfirmation", new { id = model.Id });
              
               }



               public ActionResult PaymentConfirmation(int id )
               {
                   StudentModel model = new StudentModel();
                 
                       model.paymentConfirmation = stuRepository.PaymentConfirmations.Where(m => m.Id == id).FirstOrDefault();
                       if (model.paymentConfirmation==null)
                       {
                           TempData["UnsuccessMessage"] = string.Format(" Invalid Payment Code");

                           return  RedirectToAction("PaymentConfirmation1");
                       }
                      else if (model.paymentConfirmation.ConfirmBy != "nu")
                       {
                           TempData["UnsuccessMessage"] = string.Format("Payment code {0} already confirmed", id);

                           return RedirectToAction("PaymentConfirmation1");
                       }
                       

                       model.totalPayable = stuRepository.TotalPayables.Where(m => m.Id == model.paymentConfirmation.TotalPayableId).FirstOrDefault();

                       model.user = userReository.Users.Where(m => m.UserId == model.totalPayable.StudentId).FirstOrDefault();
                       model.totalPayable.Balance = model.totalPayable.Balance - model.paymentConfirmation.AmountPaid;
       
                   return View(model);

               }

           [HttpPost]
           public ActionResult PaymentConfirmation(StudentModel model)
           {
             model.paymentConfirmation.ConfirmBy = Session["MyId"].ToString();
             model.paymentConfirmation.PaymentDate = DateTime.Now.Date;
             stuRepository.UpdatePaymentConfirmations(model.paymentConfirmation);
             model.totalPayable.AmountPaid += model.paymentConfirmation.AmountPaid;
             stuRepository.UpdateTotalPayable(model.totalPayable);
              TempData["SuccessMessage"] = string.Format(" Payment code confirm successfully");
              return RedirectToAction("PaymentConfirmation1");

           }

           public ViewResult StudentList(StudentModel model ,string Year)
           {
               var Classe = stuRepository.Classes.ToList();
               var StudentCategory = stuRepository.StudentCategorys.ToList();
               ViewBag.Class = new SelectList(Classe, "ClassId", "Name");
               ViewBag.StudentCategory = new SelectList(StudentCategory, "Id", "Name");
               ViewBag.Year = new SelectList(adminReository.AcademicYears.Select(p => p.Year).Distinct(), "Year");
               if (model.userList != null)
               {
                   model.userList.Clear();
                   model.studentCategoryList.Clear();
                   model.userList.Clear();
                   model.classeList.Clear();
               }
               model.userList = userReository.Users.ToList();
               model.studentCategoryList = stuRepository.StudentCategorys.ToList();
               model.classeList = stuRepository.Classes.ToList();
               try
               {
                   if (model.userList != null)
                   {
                       model.userList.Clear();
                       model.studentCategoryList.Clear();
                       model.userList.Clear();
                       model.classeList.Clear();
                   }

                   model.academicYear = adminReository.AcademicYears.Where(m => m.Year == Convert.ToInt32( Year))
                                                                    .Where(m=>m.ClassId==model.classe.ClassId)                                                                   
                       .FirstOrDefault();
                   model.studentList = stuRepository.Students.Where(m => m.ClassId == model.classe.ClassId)
                                                              .Where(m => m.StudentCategoryId == model.studentCategory.Id)
                                                              .Where(m => m.AcademicYearId == model.academicYear.Id).ToList();



                   var qu = userReository.Users.Join(model.studentList, s => s.UserId, u => u.UserId, (s, u) => new { s, u })
                                                       .Join(stuRepository.Classes, c => c.u.ClassId, cl => cl.ClassId, (c, cl) => new { c, cl })
                                                       .Join(stuRepository.StudentCategorys, s => s.c.u.StudentCategoryId, sc => sc.Id, (s, sc) => new { s, sc })
                                                     .Select(m => new
                                                     {
                                                         StudentId = m.s.c.s.UserId,
                                                         FathersName = m.s.c.s.FatherName,
                                                         Name = m.s.c.s.Name,
                                                         MothersName = m.s.c.s.MotherName,
                                                         Guardianphoneno = m.s.c.s.GuardianPhone,
                                                         Class = m.s.cl.Name,
                                                         StudentCategory = m.sc.Name,
                                                         dob = m.s.c.s.FatherName,
                                                         uid =m.s.c.s.Id,
                                                         img = m.s.c.s.ImageUrl,
                                                         pas = m.s.c.s.Password,
                                                         par = m.s.c.s.PermanentAddress,
                                                         pre = m.s.c.s.PresentAddress,
                                                         up = m.s.c.s.UserPhone,
                                                         gen = m.s.c.u.Id,  // students table id
                                                         ust = m.s.c.s.UserStatusId,

                                                     });

               
                   foreach (var i in qu)
                   {
                       User u = new User();
                       u.FatherName = i.FathersName;
                       u.MotherName = i.MothersName;
                       u.Name = i.Name;
                       u.GuardianPhone = i.Guardianphoneno;
                       u.UserId = i.StudentId;
                       u.Activity = false;
                       u.DOB = i.dob;
                       u.ImageUrl = i.img;
                       u.Password = i.pas;
                       u.PermanentAddress = i.par;
                       u.PresentAddress = i.pre;
                       u.Id = i.uid;
                       u.GenderId = i.gen; //students table id
                       u.UserPhone = i.up;
                       u.UserStatusId = i.ust;

                       model.userList.Add(u);
                       StudentCategory sc = new StudentCategory();
                       sc.Name = i.StudentCategory;
                       sc.Id = 1;
                       model.studentCategoryList.Add(sc);
                       Classe c = new Classe();
                       
                       c.Name = i.Class;
                       c.ClassId = 1;
                     
                
                       model.classeList.Add(c);
                   
                     

                   }
                  
               }
               catch
               {
                  
               }     
             
               return View(model);
           }

        
           public ViewResult EditStudent(string id , string sid)
           {
               StudentModel model = new StudentModel();
               var Classee = stuRepository.Classes.ToList();
               var StudentCategorye = stuRepository.StudentCategorys.ToList();
               ViewBag.Gender = new SelectList( userReository.Genders, "GenderId", "Name");
               ViewBag.Class = new SelectList(Classee, "ClassId", "Name");
               ViewBag.StudentCategory = new SelectList(StudentCategorye, "Id", "Name");
               ViewBag.Section = new SelectList(stuRepository.Sections, "Id", "Name");
               ViewBag.Year = new SelectList(adminReository.AcademicYears.Select(p => p.Year).Distinct(), "Year");
               model.user = userReository.Users.Where(u => u.UserId == id).FirstOrDefault();
               model.student = stuRepository.Students.Where(u => u.Id == Convert.ToInt32( sid)).FirstOrDefault();
               model.genderDic = userReository.Genders.ToDictionary(k => k.GenderId, k => k.Name);
               return View(model);
           }
           [HttpPost]
           public ActionResult EditStudent(StudentModel model)
           {

           
               var Classee = stuRepository.Classes.ToList();
               var StudentCategorye = stuRepository.StudentCategorys.ToList();
               ViewBag.Gender = new SelectList(userReository.Genders, "GenderId", "Name");
               ViewBag.Class = new SelectList(Classee, "ClassId", "Name");
               ViewBag.StudentCategory = new SelectList(StudentCategorye, "Id", "Name");
               ViewBag.Section = new SelectList(stuRepository.Sections, "Id", "Name");
               ViewBag.Year = new SelectList(adminReository.AcademicYears.Select(p => p.Year).Distinct(), "Year");
            
                   model.student.FirstDayOfClass = model.student.FirstDayOfClass.Value.Date;
                   userReository.UpdateUser(model.user);                 
                 Student student=  stuRepository.Students.Where(m => m.ClassId == model.student.ClassId)                                      
                                         .Where(m => m.UserId == model.student.UserId)
                                         .Where(m=>m.Id==model.student.Id)
                                         .Where(m => m.FirstDayOfClass == model.student.FirstDayOfClass)
                                         .Where(m => m.AcademicYearId == model.student.AcademicYearId)
                                         .Where(m => m.StudentCategoryId == model.student.StudentCategoryId).FirstOrDefault();
                  if(student==null)
                  { 
                        student = stuRepository.Students
                                         .Where(m => m.Id == model.student.Id)
                                        .FirstOrDefault();
                     Scholarship sc=  adminReository.Scholarships.Where(s => s.AcademicYearId ==student.AcademicYearId)
                                                                                      .Where(s => s.StudentId == model.student.UserId)
                                                                                      .FirstOrDefault();
                    
                        model.student.AcademicYearId = adminReository.AcademicYears.Where(m => m.Year == model.student.AcademicYearId)
                                                                    .Where(m => m.ClassId == model.student.ClassId)
                                                                    .Select(d => d.Id).FirstOrDefault();
                        sc.AcademicYearId = model.student.AcademicYearId;
                        adminReository.UpdateScholarship(sc);
                       if (ModelState.IsValid)
                       {
                           model.student.FirstDayOfClass = model.student.FirstDayOfClass.Value.Date;
                      
                           int month = model.student.FirstDayOfClass.Value.Month;
                           int mo = month;
                           int monthNow = DateTime.Now.Date.Month;
                           List<TotalPayable> totalPayable = stuRepository.TotalPayables.Where(p => p.AcademicYearId == student.AcademicYearId)
                                                                       .Where(s=>s.StudentId== student.UserId).ToList();
                           foreach (var tpf in totalPayable)
                           {
                               if(tpf.AmountPaid>0)
                               {
                                   TempData["UnsuccessMessage"] = string.Format("This student information  update successfully");
                                   return View();

                               }
                           }
                           foreach(var tp in totalPayable)
                           {
                               List<PayableInDetail> payableD = stuRepository.PayableInDetails.Where(p => p.TotalPayableId == tp.Id).ToList();
                               foreach (var pd in payableD)
                               {
                                   stuRepository.RemovePayableDetails(pd.Id);
                               }
                               stuRepository.RemoveTotalPayable(tp.Id);
                           }

                           for (int j = month; j <= monthNow; j++)
                           {
                               month = j;
                               int ttpl = stuRepository.TotalPayables.Where(p => p.Month == month)
                                                                      .Where(p => p.AcademicYearId == model.student.AcademicYearId)
                                                                      .Where(s => s.StudentId == model.student.UserId)
                                                                      .Select(s => s.Id)
                                                                      .FirstOrDefault();


                               if (ttpl == 0)
                               {

                                   TotalPayable totalPay = new TotalPayable();
                                   totalPay.Month = month;
                                   totalPay.StudentId = model.student.UserId;
                                   totalPay.AcademicYearId = model.student.AcademicYearId;
                                   ttpl = stuRepository.UpdateTotalPayable(totalPay);

                                   totalPay.Id = ttpl;

                                   totalPay.PrevBalance = 0;
                                   totalPay.Amount = 0;
                                   totalPay.Balance = 0;
                                   totalPay.NetTotal = 0;
                                   totalPay.AmountPaid = 0;
                                   if (month - 1 < 1)
                                   {
                                       var stuYearList = stuRepository.Students.Where(s => s.UserId == model.student.UserId)
                                                                      .Select(S => S.AcademicYearId).ToList();
                                       if (stuYearList.Count() > 1)
                                       {
                                           totalPay.PrevBalance = stuRepository.TotalPayables.Where(t => t.StudentId == model.student.UserId)
                                                                                    .Where(t => t.AcademicYearId == stuYearList[stuYearList.Count() - 2])
                                                                                    .Where(s => s.Month == 12)
                                                                                    .Select(s => s.Balance)
                                                                                    .FirstOrDefault();
                                       }
                                   }
                                   else
                                   {
                                       totalPay.PrevBalance = stuRepository.TotalPayables.Where(t => t.StudentId == model.student.UserId)
                                                                                    .Where(t => t.AcademicYearId == model.student.AcademicYearId)
                                                                                    .Where(s => s.Month == (month - 1))
                                                                                    .Select(s => s.Balance)
                                                                                    .FirstOrDefault();

                                   }
                                   totalPay.Deduction = adminReository.Scholarships.Where(s => s.AcademicYearId == model.student.AcademicYearId)
                                                                                    .Where(s => s.StudentId == model.student.UserId)
                                                                                    .Select(s => s.Amount).FirstOrDefault();
                                   totalPay.NetTotal += totalPay.PrevBalance;
                                   totalPay.NetTotal -= totalPay.Deduction;
                                   var femonth = stuRepository.FeeMonths.Where(f => f.ClassId == model.student.ClassId)
                                                                        .Where(f => f.StudentCategoryId == model.student.StudentCategoryId)
                                                                        .ToArray();


                                   bool dd = false;

                                   for (int i = 0; i < femonth.Count(); i++)
                                   {
                                       dd = false;

                                       if (femonth[i].January == true && month == 1)
                                       {
                                           dd = true;
                                       }
                                       else if (femonth[i].February == true && month == 2)
                                       {
                                           dd = true;
                                       }
                                       else if (femonth[i].March == true && month == 3)
                                       {
                                           dd = true;
                                       }
                                       else if (femonth[i].April == true && month == 4)
                                       {
                                           dd = true;

                                       }
                                       else if (femonth[i].May == true && month == 5)
                                       {
                                           dd = true;
                                       }
                                       else if (femonth[i].June == true && month == 6)
                                       {
                                           dd = true;

                                       }
                                       else if (femonth[i].July == true && month == 7)
                                       {
                                           dd = true;
                                       }
                                       else if (femonth[i].Augest == true && month == 8)
                                       {
                                           dd = true;
                                       }

                                       else if (femonth[i].September == true && month == 9)
                                       {
                                           dd = true;
                                       }
                                       else if (femonth[i].October == true && month == 10)
                                       {
                                           dd = true;
                                       }
                                       else if (femonth[i].November == true && month == 11)
                                       {
                                           dd = true;
                                       }
                                       else if (femonth[i].December == true && month == 12)
                                       {
                                           dd = true;
                                       }
                                       else if (femonth[i].yearly == true && j == mo)
                                       {
                                           dd = true;
                                       }
                                       if (dd == true)
                                       {
                                           FeeAmount feeAmount = stuRepository.FeeAmounts.Where(f => f.FeeId == femonth[i].FeeId)
                                                                                         .Where(f => f.ClassId == model.student.ClassId)
                                                                                         .Where(f => f.StudentCategoryId == model.student.StudentCategoryId)
                                                                                         .FirstOrDefault();
                                           if (feeAmount != null)
                                           {
                                               PayableInDetail payDeta = new PayableInDetail();
                                               payDeta.FeeId = feeAmount.FeeId;
                                               payDeta.TotalPayableId = ttpl;
                                               payDeta.Amount = feeAmount.Amount;
                                               totalPay.Amount += feeAmount.Amount;
                                               stuRepository.UpdatePayableDeatails(payDeta);
                                           }

                                       }
                                   }
                                   totalPay.NetTotal += totalPay.Amount;
                                   totalPay.Balance = totalPay.NetTotal;
                                   stuRepository.UpdateTotalPayable(totalPay);
                                   TempData["SuccessMessage"] = string.Format("Student information added successful");


                               }



                           }


                           stuRepository.UpdateStudentInfo(model.student);
                   }
               }
               return View();
           }
    }
}