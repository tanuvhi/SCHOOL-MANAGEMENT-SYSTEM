using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCC.Domain.Entities
{
   public class User
    {     
        public string UserId { get; set; }
       [Required]
        public string Name { get; set; }
       [Display(Name="Father Name")]
       [Required]
        public string FatherName { get; set; }
        [Display(Name = "Mother Name")]
        [Required]
        public string MotherName { get; set; }
        [Display(Name = "Phone No")]
        [Required]
        public string UserPhone { get; set; }
        [Display(Name = "Guardian Phone No")]
        [Required]
        public string GuardianPhone { get; set; }
        [Display(Name = "Present Address")]
        [Required]
        public string PresentAddress { get; set; }
         [Display(Name = "Permanent Address")]
          [Required]
        public string PermanentAddress { get; set; }
        [Display(Name = "Image")]
        [Required]
        public string ImageUrl { get; set; }
       [Required]
        public string Password { get; set; }
       [Display(Name = "User Status")]
       [Required]
        public int  UserStatusId { get; set; }
       [Required]
        public string DOB { get; set; }
       [Display(Name = "Gender")]
       [Required]
        public int GenderId { get; set; }

       [Key] 
        public int Id { get; set; }

       public bool Activity { get; set; }
 
    }
}
