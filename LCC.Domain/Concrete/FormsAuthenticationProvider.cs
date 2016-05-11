using LCC.Domain.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LCC.Domain.Abstract;


namespace LCC.Domain.Concrete
{
   public class FormsAuthenticationProvider : IAuthentication
    {
       private readonly EFDbContext context = new EFDbContext();
       public int Authenticate(string userId, string password)
        {

            var result = context.Users.Where(u => u.UserId == userId && u.Activity == true  )
                                        .Where(u=>u.Password == password)
                                      .FirstOrDefault();
           
           
            if (result == null)
            {
                return 10;
            }
            else
            {

                return result.UserStatusId;
            }
        }

        public bool Logout()
        {
            return true;
        }
    }
}
