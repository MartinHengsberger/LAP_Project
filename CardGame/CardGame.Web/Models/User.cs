using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CardGame.Web.Models
{
    public class User
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "required!", AllowEmptyStrings = false)]
        [RegularExpression(@"^[a-zA-Z--]+$", ErrorMessage = "only upper and lowercase letters allowed")]
        public string Firstname { get; set; }

        [Required(ErrorMessage = "required!", AllowEmptyStrings = false)]
        [RegularExpression(@"^[a-zA-Z--]+$", ErrorMessage = "only upper and lowercase letters allowed")]
        public string Lastname { get; set; }

        [Required(ErrorMessage = "required!", AllowEmptyStrings = false)]
        [RegularExpression(@"^[a-zA-Z0-9--]+$", ErrorMessage = "no special characters allowed")]
        public string Gamertag { get; set; }

        [Required(ErrorMessage = "required!", AllowEmptyStrings = false)]
        [DataType(DataType.EmailAddress, ErrorMessage = "invalid ermailadress")]
        public string Email { get; set; }

        [Required(ErrorMessage = "required!", AllowEmptyStrings = false)]
        [DataType(DataType.EmailAddress, ErrorMessage = "invalid ermailadress")]
        [Compare("Email", ErrorMessage = "email not identical!")]
        [DisplayName("confirmEmail")]
        public string EmailValidation { get; set; }

        [StringLength(maximumLength: 20, MinimumLength = 4, ErrorMessage = "password requirements not met (4-20 digits)")]
        [Required(ErrorMessage = "required!", AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "required!", AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "passwords not identical!")]
        [DisplayName("confirmPassword")]
        public string PasswordValidation { get; set; }

        public string Salt { get; set; }

        public string Role { get; set; }

        public bool isActive { get; set; }

        [Required(ErrorMessage = "required!", AllowEmptyStrings = false)]
        [RegularExpression(@"^[a-zA-Z --]+$", ErrorMessage = "only upper and lowercase letters allowed")]
        public string Street { get; set; }

        [Required(ErrorMessage = "required!", AllowEmptyStrings = false)]
        public string Additions { get; set; }

        [Required(ErrorMessage = "required!", AllowEmptyStrings = false)]
        [RegularExpression(@"^[a-zA-Z0-9 --]+$", ErrorMessage = "no special characters allowed")]
        public string Zipcode { get; set; }

        [Required(ErrorMessage = "required!", AllowEmptyStrings = false)]
        [RegularExpression(@"^[a-zA-Z --]+$", ErrorMessage = "only upper and lowercase letters allowed")]
        public string City { get; set; }

        [Required(ErrorMessage = "required!", AllowEmptyStrings = false)]
        [RegularExpression(@"^[a-zA-Z_äÄöÖüÜß --]+$", ErrorMessage = "only upper and lowercase letters allowed")]
        public string Country { get; set; }
    }
}