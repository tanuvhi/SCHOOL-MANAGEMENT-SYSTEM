using LCC.Domain.Concrete;
using LCC.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace LCC.WebUI.Controllers
{
       [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        //
        // GET: /Admin/
        private readonly EFAdminRepository adminReository;
        private readonly EFStudenRepository stuRepository;
        private readonly EFUserRepository userRepository;
        public AdminController(EFAdminRepository adRp,EFStudenRepository stu ,EFUserRepository usr)
        {
            var badCulture = new CultureInfo("");
            badCulture.DateTimeFormat.ShortDatePattern = "dd-MM-yyyy";
            Thread.CurrentThread.CurrentCulture = badCulture;
            adminReository = adRp;
            stuRepository = stu;
            userRepository = usr;
        }
        public ActionResult Index()
        {
            return View();
        }

            [Authorize(Roles = "Admin")]
        public ViewResult AcademicYear()
        {

            ViewBag.Class = new SelectList(stuRepository.Classes, "ClassId", "Name");

            return View();

        }
            [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult AcademicYear(AcademicYear model)
        {

            ViewBag.Class = new SelectList(stuRepository.Classes, "ClassId", "Name");

           

            if (ModelState.IsValid)
            {
                var f = adminReository.AcademicYears.Where(m => m.Year == model.Year)
                                               .Where(m => m.ClassId == model.ClassId).FirstOrDefault();
                string ClassName = stuRepository.Classes.Where(m => m.ClassId == model.ClassId).Select(s => s.Name).FirstOrDefault();
                if (f == null)
                {
                    TempData["SuccessMessage"] = string.Format("successfully ");
                    adminReository.UpdateAcademicYear(model);

                }
                else
                {

                    TempData["UnsuccessMessage"] = string.Format("Class {0} academic year {1} already adjusted, Now you can only edit", ClassName,model.Year);


                }



            }
            return View();

        }
            [Authorize(Roles = "Admin")]
        [AllowAnonymous]
        public JsonResult SNameAutoComplite(string term)
        {
           
          
            var stu = stuRepository.Students.ToList();
            var cls = stuRepository.Classes.ToList();
            var result = from u in userRepository.Users
                         from ss in stu
                         from cl in cls
                         where(u.UserId ==ss.UserId)
                         where(cl.ClassId==ss.ClassId)
                         where u.Name.ToLower().Contains(term.ToLower())
                         select new { u.Name, u.UserId, ClassName = cl.Name, u.FatherName };

            return Json(result, JsonRequestBehavior.AllowGet);
        }
       

        public ViewResult Schilarship()
        {
            

           

            return View();

        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Schilarship(Scholarship model ,string Name)
        {
            if(ModelState.IsValid)
            {
                int yId= stuRepository.Students.Where(s=>s.UserId==model.StudentId)
                                                .Select(s=>s.AcademicYearId)
                                                .LastOrDefault();
                var f = adminReository.Scholarships.Where(s => s.AcademicYearId == yId)
                                                    .Where(s => s.StudentId == model.StudentId).FirstOrDefault();
               

                if (f == null)
                {
                    model.AcademicYearId = yId;
                    TempData["SuccessMessage"] = string.Format("successfull ");
                    adminReository.UpdateScholarship(model);

                }
                else
                {

                    TempData["UnsuccessMessage"] = string.Format(" Schilarship has already been taken  before , Now you can only edit");


                }
            }

            return View();
        }
       public  ViewResult  UnableToPayList()
        {

          
            return View();
        }
       public ActionResult DisproveList()
       {
           //3 disporve
           var qurey = stuRepository.PaymentConfirmations 
                                               .Where(m=>m.UnableToPayConfirm==3)                                           
                                             .Join(stuRepository.TotalPayables, pc => pc.TotalPayableId, tp => tp.Id, (tp, pc) => new { tp, pc })
                                             .Join(userRepository.Users, tpp => tpp.pc.StudentId, us => us.UserId, (tpp, us) => new { tpp, us })
                                             .Select(m => new
                                             {
                                                 id =m.tpp.tp.Id,
                                                 PaymentDate = m.tpp.tp.PaymentDate.ToString(),
                                                 Name = m.us.Name,
                                               
                                                 Due = m.tpp.pc.Balance,
                                                 AmountPaid = m.tpp.pc.AmountPaid,
                                                 UnableToPay = m.tpp.tp.UnableToPay

                                             });

           return Json(qurey, JsonRequestBehavior.DenyGet);
       }

       public ActionResult PendingList()
       {
           var qurey = stuRepository.PaymentConfirmations
                                               .Where(m => m.UnableToPayConfirm == 1)
                                               .Where(m => m.ConfirmBy != "nu")
                                               .Where(m => m.UnableToPay > 0)
                                             .Join(stuRepository.TotalPayables, pc => pc.TotalPayableId, tp => tp.Id, (tp, pc) => new { tp, pc })
                                             .Join(userRepository.Users, tpp => tpp.pc.StudentId, us => us.UserId, (tpp, us) => new { tpp, us })
                                             .Select(m => new
                                             {
                                                 PaymentDate = m.tpp.tp.PaymentDate.ToString(),
                                                 Name = m.us.Name,
                                                  id =m.tpp.tp.Id,
                                                 Due = m.tpp.pc.Balance,
                                                 AmountPaid = m.tpp.pc.AmountPaid,
                                                 UnableToPay = m.tpp.tp.UnableToPay

                                             });

           return Json(qurey, JsonRequestBehavior.DenyGet);
       }
       public ActionResult ApproveList()
       {
           // 2 Approved
           var qurey = stuRepository.PaymentConfirmations
                                               .Where(m => m.UnableToPayConfirm == 2)                                         
                                             .Join(stuRepository.TotalPayables, pc => pc.TotalPayableId, tp => tp.Id, (tp, pc) => new { tp, pc })
                                             .Join(userRepository.Users, tpp => tpp.pc.StudentId, us => us.UserId, (tpp, us) => new { tpp, us })
                                             .Select(m => new
                                             {
                                                 PaymentDate = m.tpp.tp.PaymentDate.ToString(),
                                                 Name = m.us.Name,
                                                  id =m.tpp.tp.Id,
                                                 Due = m.tpp.pc.Balance,
                                                 AmountPaid = m.tpp.pc.AmountPaid,
                                                 UnableToPay = m.tpp.tp.UnableToPay
                                             });

           return Json(qurey, JsonRequestBehavior.DenyGet);
       }

       
   public ActionResult Approve(int id)
       {

         PaymentConfirmation pc=  stuRepository.PaymentConfirmations.Where(m => m.Id == id).FirstOrDefault();
         pc.UnableToPayConfirm = 2;

           stuRepository.UpdatePaymentConfirmations(pc);
           TotalPayable tp = stuRepository.TotalPayables.Where(m => m.Id == pc.TotalPayableId).FirstOrDefault();
           tp.Balance -= pc.UnableToPay;
           stuRepository.UpdateTotalPayable(tp);          
           return View();
       }
           public ActionResult Disprove(int id)
           {
               PaymentConfirmation pc = stuRepository.PaymentConfirmations.Where(m => m.Id == id).FirstOrDefault();
               pc.UnableToPayConfirm = 3;
               stuRepository.UpdatePaymentConfirmations(pc);
               return View();
           }
	}
}