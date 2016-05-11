using LCC.Domain.Abstract;
using LCC.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCC.Domain.Concrete
{
   public class EFUserRepository : IUserRepository
    {
       private readonly EFDbContext context = new EFDbContext();

       public IEnumerable<User> Users
       {
           get { return context.Users; }
       }
   
       public IEnumerable<Gender> Genders
       {
           get { return context.Genders; }
       }
       public IEnumerable<Teacher> Teachers
       {
           get { return context.Teachers; }
       }
      

       public IEnumerable<UserStatus> UserStatus
       {
           get { return context.UserStatus; }
       }

 
       public string UpdateUser(User aUser)
       {
           if (aUser.Id == 0)
           {
               aUser.UserId = "t";
               if(aUser.ImageUrl==null)
               aUser.ImageUrl = "dfd";
               context.Users.Add(aUser);
               context.SaveChanges();
               string userType;
               if (aUser.UserStatusId == 3)
                   userType = "st-";               
               else if (aUser.UserStatusId == 4)
                   userType = "te-";
               else if (aUser.UserStatusId == 1)
                   userType = "ad-";
               else if (aUser.UserStatusId == 2)
                   userType = "ac-";
               else
                   userType = "em-";

               if (aUser.Id < 10)
                   userType += "000";
               else if (aUser.Id < 100)
                   userType += "00";
               else if (aUser.Id < 1000)
                   userType += "0";
               string userId =userType+ aUser.Id.ToString();
               
              User user= context.Users.Find(aUser.Id);
              user.UserId = userId;

               
           }
           else
           {
             User user =  context.Users.Find(aUser.Id);
             user.UserId = aUser.UserId;
             user.Name = aUser.Name;
             user.MotherName = aUser.MotherName;
             user.FatherName = aUser.FatherName;
             user.DOB = aUser.DOB;
             user.UserPhone = aUser.UserPhone;
             user.GenderId = aUser.GenderId;
             user.PermanentAddress = aUser.PermanentAddress;
             user.PresentAddress = aUser.PresentAddress;
             user.GuardianPhone = aUser.GuardianPhone;
             user.ImageUrl = aUser.ImageUrl;
             user.Password = aUser.Password;
             user.UserStatusId = aUser.UserStatusId;
               
              
           }
           context.SaveChanges();
           return aUser.UserId;
       }

 

    }
}
