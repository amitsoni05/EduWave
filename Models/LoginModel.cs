﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudStudentProject.Models
{
    public class LoginModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string UserPassword { get; set; }
       
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        
    }
}
