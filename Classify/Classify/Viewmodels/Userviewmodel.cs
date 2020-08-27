using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Classify.Viewmodels
{
    public class Userviewmodel
    {
        public int Id { get; set; }
        [MinLength(3, ErrorMessage = "Atleast 3 char needed")]
        [MaxLength(50, ErrorMessage = "upto 50 char allowed")]
        [Required(ErrorMessage = "This Field is Required")]
        public string Firstname { get; set; }
        [MinLength(3, ErrorMessage = "Atleast 3 char needed")]
        [MaxLength(50, ErrorMessage = "upto 50 char allowed")]
        [Required(ErrorMessage = "This Field is Required")]
        public string Lastname { get; set; }
        [MinLength(20, ErrorMessage = "min 20 char needs")]
        [MaxLength(150, ErrorMessage = "max 150 char allowed")]
        [Required(ErrorMessage = "This Field is Required")]
        public string Address { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        public Nullable<int> City { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        public Nullable<int> State { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        public Nullable<int> Country { get; set; }
        [RegularExpression("^\\d{6}$", ErrorMessage = "Zip code should be 6 digits")]
        [Required(ErrorMessage = "This Field is Required")]
        public Nullable<int> Zipcode { get; set; }
    }
}