using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MortyShop_MVC.Models
{
    public class Manager
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Bu alan boş bırakılamaz")]
        [StringLength(maximumLength: 20, ErrorMessage = "Bu alan en fazla 20 karakter olabilir")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Bu alan boş bırakılamaz")]
        [StringLength(maximumLength: 20, ErrorMessage = "Bu alan en fazla 20 karakter olabilir")]
        public string Surname { get; set; }

        [Index(IsUnique = true)]
        [Required(ErrorMessage = "Bu alan boş bırakılamaz")]
        [StringLength(maximumLength: 20, ErrorMessage = "Bu alan en fazla 20 karakter olabilir")]
        public string Username { get; set; }

        [Index(IsUnique = true)]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Bu alan boş bırakılamaz")]
        [StringLength(maximumLength: 20, ErrorMessage = "Bu alan en fazla 40 karakter olabilir")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Bu alan boş bırakılamaz")]
        [StringLength(maximumLength: 20, ErrorMessage = "Bu alan en fazla 20 karakter olabilir")]
        public string Password { get; set; }
        public DateTime LastEntry { get; set; }
        public bool IsActive { get; set; }
        public int RoleID { get; set; }
        [ForeignKey("RoleID")]
        public virtual ManagerRole managerRole { get; set; }
    }
}