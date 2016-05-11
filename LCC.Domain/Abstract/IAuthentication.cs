using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCC.Domain.Abstract
{
  public  interface IAuthentication
    {
      int  Authenticate(string userId, string password);
      bool Logout();

    }
}
