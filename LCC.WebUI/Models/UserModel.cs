
using LCC.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LCC.WebUI.Models
{
    public class UserModel
    {
        public IEnumerable<HttpPostedFileBase> File { get; set; }
        public User user { get; set; }

    }
}