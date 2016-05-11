using LCC.Domain.Abstract;
using LCC.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace LCC.WebUI.Controllers
{
    public class AccountController : Controller
    {
            private readonly IUserRepository  userRepository;
        private readonly IStudentRepository stuRepository;
        private readonly IAuthentication authentication;

        private string userId = "";
        public AccountController(IUserRepository UserRepo, IStudentRepository stuRepo, IAuthentication auth)
        {
          
            userRepository = UserRepo;
            stuRepository = stuRepo;
            authentication = auth;
        }
        //
        // GET: /Account/
        public ActionResult Index()
        {
            return View();
        }

        public ViewResult Login()
        {

            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(User model)
        {

            if (model.UserId != "" & model.Password != "")
            {
                int UserStatusId = authentication.Authenticate(model.UserId, model.Password);
                if (UserStatusId < 3)
                {
                    Session["MyKey"] = UserStatusId;
                    User user = userRepository.Users.FirstOrDefault(u => u.UserId == model.UserId);
                    Session["MyId"] = user.UserId;
                    Session["Name"] = user.Name;
                    userId = model.UserId;
                }
                if (UserStatusId == 1) //1 means admin
                {

                    FormsAuthentication.SetAuthCookie(model.UserId, false);

                    if (!Roles.RoleExists("Admin"))
                    {
                        Roles.CreateRole("Admin");
                    }

                    if (!Roles.IsUserInRole(model.UserId, "Admin"))
                    {
                       
                        Roles.AddUserToRole(model.UserId, "Admin");
                    }

                    return RedirectToAction("AcademicYear", "Admin");


                }
                else if (UserStatusId == 2) // 0 means customer
                {

                    if (!Roles.RoleExists("accountant"))
                    {
                        Roles.CreateRole("accountant");
                    }

                    if (!Roles.IsUserInRole(model.UserId, "accountant"))
                    {
                        Roles.AddUserToRole(model.UserId, "accountant");

                    }

                    FormsAuthentication.SetAuthCookie(model.UserId, false);
                    return Redirect(Url.Action("StudentPayment", "Student"));


                }
                else if (UserStatusId == 6)
                {
                    ModelState.AddModelError("", "Your account has been deactivated. Please contact Administrator ");
                    return View();
                }
                else
                {
                    ModelState.AddModelError("", "Incorect UserId or password ");
                    return View();
                }
            }
            else
            {
                return View();
            }

        }
        public ActionResult Logout()
        {
            if (Roles.IsUserInRole(userId, "accountant"))
                Roles.RemoveUserFromRole(userId, "accountant");
            if (Roles.IsUserInRole(userId, "accountant"))
                Roles.RemoveUserFromRole(userId, "accountant");
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Admin");
        }
       
	}
}