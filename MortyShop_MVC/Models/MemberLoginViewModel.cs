using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MortyShop_MVC.Models
{
    public class MemberLoginViewModel
    {
        [Required(ErrorMessage = "Mail alanı zorunludur.")]
        public string Mail { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Mail alanı zorunludur.")]
        public string Password { get; set; }
    }
}