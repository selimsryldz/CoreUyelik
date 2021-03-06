﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Uyelik.ViewModels
{
    public class UserViewModel
    {


        [Required(ErrorMessage = "Kullanıcı adı gereklidir")]
        [Display(Name ="Kullanıcı Adı")]
        public string UserName { get; set; }


        [Display(Name = "Tel No:")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Email adresi gereklidir")]
        [Display(Name = "Email Adresiniz")]
        [EmailAddress(ErrorMessage ="Email adresiniz doğru formatta değil")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifreniz gereklidir")]
        [Display(Name = "Şifre")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        

       





    }
}
