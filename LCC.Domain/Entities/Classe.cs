﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCC.Domain.Entities
{
  public  class Classe
    {
       [Key]
        public int ClassId { get; set; }
        public string  Name { get; set; }

       
    }
}
