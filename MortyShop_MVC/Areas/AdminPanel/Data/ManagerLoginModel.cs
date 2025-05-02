using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MortyShop_MVC.Areas.AdminPanel.Data
{
    public class ManagerLoginModel
    {
        [Required(ErrorMessage = "Kullanıcı adı boş olamaz")]
        [StringLength(maximumLength: 30, MinimumLength = 5, ErrorMessage = "Bu alan 5 - 30 karakter arasında olabilir")]
        public string Username { get; set; }

        [Display(Name = "Şifre")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Lütfen bir şifre girin")]
        [StringLength(maximumLength: 30, MinimumLength = 4, ErrorMessage = "Bu alan 4 - 16 karakter arasında olabilir")]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}