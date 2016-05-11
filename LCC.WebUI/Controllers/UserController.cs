using ImageResizer;
using LCC.Domain.Abstract;
using LCC.Domain.Entities;
using LCC.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace LCC.WebUI.Controllers
{
      [Authorize(Roles = "Admin,Accountant")]
    public class UserController : Controller
    {
        //
        // GET: /User/
        private readonly IUserRepository  userRepository;
        private readonly IStudentRepository stuRepository;
        private readonly IAuthentication authentication;


        public UserController(IUserRepository UserRepo, IStudentRepository stuRepo, IAuthentication auth)
        {
            var badCulture = new CultureInfo("");
            badCulture.DateTimeFormat.ShortDatePattern = "dd-MM-yyyy";
            Thread.CurrentThread.CurrentCulture = badCulture;
            userRepository = UserRepo;
            stuRepository = stuRepo;
            authentication = auth;
        }
        public ViewResult Register()
        {
            UserModel model =new UserModel();
            ViewBag.UserStatus = new SelectList(userRepository.UserStatus, "UserStatusId", "Name");
            ViewBag.Gender = new SelectList(userRepository.Genders, "GenderId", "Name");
           
            return View(model);
        }

        [HttpPost]
        public ActionResult Register(UserModel model)
        {
         model.user.Password= Membership.GeneratePassword(4, 0);
         model.user.Password = Regex.Replace(model.user.Password, @"[^A-Z0-9]", m => "9");
            ViewBag.UserStatus = new SelectList(userRepository.UserStatus, "UserStatusId", "Name");
            ViewBag.Gender = new SelectList(userRepository.Genders, "GenderId", "Name");
            //ModelState["user.Passeord"].Errors.Clear();
                 //   ModelState["user.ImageUrl"].Errors.Clear();
                    foreach (var key in ModelState.Keys)
                    {
                        if (key == "user.Password" || key == "user.ImageUrl")
                        ModelState[key].Errors.Clear();
                    }
            if (ModelState.IsValid)
            {
                var versions = new Dictionary<string, string>();

                var path = Server.MapPath("~/Content/Photo/");
                versions.Add(model.user.Name, "maxwidth=300&maxheight=300&format=jpg");

                foreach (var file in model.File)
                {

                    if (file != null)
                    {

                        //Declare a new dictionary to store the parameters for the image versions.
                        string fname = file.FileName + ".jpg";

                        //Define the versions to generate


                        model.user.ImageUrl = path + model.user.Name + fname;


                        //Generate each version
                        foreach (var suffix in versions.Keys)
                        {
                            file.InputStream.Seek(0, SeekOrigin.Begin);

                            //Let the image builder add the correct extension based on the output file type

                            ImageBuilder.Current.Build(
                           new ImageJob(
                         file.InputStream,
                         path + suffix + file.FileName,
                         new Instructions(versions[suffix]),
                         false,
                         true));
                            break;
                        }

                    }

                }

                model.user.Activity = false;

                model.user.UserId = userRepository.UpdateUser(model.user);
                if (model.user.UserStatusId == 3)
                {

                    return RedirectToAction("StudentInfo", "Student", new { userId = model.user.UserId, name = model.user.Name });
                }
            }
                return View();

        }

        public ViewResult Index()
        {
            return View();
        }
   
     
	}
}