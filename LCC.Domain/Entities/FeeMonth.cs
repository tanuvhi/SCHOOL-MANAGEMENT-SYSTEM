using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCC.Domain.Entities
{
   public class FeeMonth
    {
       [Key]
       public int Id { get; set; }

       public int ClassId { get; set; }
       public int StudentCategoryId { get; set; }
       public int FeeId { get; set; }
       public bool January { get; set; }

        public bool February { get; set; }
        public bool March { get; set; }
        public bool April { get; set; }
        public bool May { get; set; }
        public bool June { get; set; }
        public bool July { get; set; }
        public bool Augest { get; set; }

        public bool September { get; set; }
        public bool October { get; set; }
        public bool November { get; set; }
        public bool December { get; set; }
        public bool yearly { get; set; }
    }
}
