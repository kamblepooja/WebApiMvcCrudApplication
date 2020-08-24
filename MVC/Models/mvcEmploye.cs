using System;
using System.ComponentModel.DataAnnotations;

namespace MVC.Models
{
    public class mvcEmploye
    {
        public int EmployeId { get; set; }

        [RegularExpression("^([a-zA-Z0-9 .&'-]+)$", ErrorMessage = "Invalid Name")]
        [Required]
        public string Name { get; set; }

        [Required]
        public string Position { get; set; }

        [Range(18, 60, ErrorMessage = "Age Should be min 18 and max 60")]
        [Required]
        public Nullable<int> Age { get; set; }

        [Required]
        public Nullable<int> Salary { get; set; }

        [Required]
        public byte[] image { get; set; }

        public string imageString { get; set; }
    }
}