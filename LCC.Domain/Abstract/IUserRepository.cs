using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LCC.Domain.Entities;

namespace LCC.Domain.Abstract
{
   public interface IUserRepository 
    {

       IEnumerable<User> Users { get; }
       IEnumerable<Gender> Genders { get;}
       IEnumerable<UserStatus> UserStatus { get; }
       
        string UpdateUser(User aUser);
       
    }
}
