using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Uyelik.ViewModels
{
    public class PasswordChangeViewModel
    {
        [Display(Name = "Eski şifre")]
        [Required(ErrorMessage = "Eski şifreniz gereklidir")]
        [DataType(DataType.Password)]
        [MinLength(4,ErrorMessage = "Şifreniz en az 4 karakterli olmak zorundadır.")]
        public string PasswordOld { get; set; }

        [Display(Name = "Yeni şifre")]
        [Required(ErrorMessage = "Yeni şifre gereklidir")]
        [DataType(DataType.Password)]
        [MinLength(4, ErrorMessage = "Şifreniz en az 4 karakterli olmak zorundadır.")]
        public string PasswordNew { get; set; }

        [Display(Name = "Yeni şifre (tekrar)")]
        [Required(ErrorMessage = "Yeni şifre (tekrar) gereklidir")]
        [DataType(DataType.Password)]
        [MinLength(4, ErrorMessage = "Şifreniz en az 4 karakterli olmak zorundadır.")]
        [Compare("PasswordNew",ErrorMessage = "Yeni şifre ve yeni şifre (tekrar) uyuşmamaktadır.")]
        public string PasswordConfirm { get; set; }


    }
}
