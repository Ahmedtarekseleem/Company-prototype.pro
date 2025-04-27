using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Company.pro.PL.Dtos
{
    public class CreateEmployeeDto
    {
        [Required(ErrorMessage ="Name Is Required !!")]
        public string Name { get; set; }
        [Range(22,60)]
        public int? Age { get; set; }
        [DataType(DataType.EmailAddress,ErrorMessage ="Email Is Not Valid !!")]
        public string Email { get; set; }
        [RegularExpression(@"[0-9]{1,3}-[a-zA-Z]{5,10}-[a-zA-Z]{4,10}-[a-zA-Z]{5,10}$",
                               ErrorMessage = "Address Must Be Like 123-street-city-country")]
        public string Address { get; set; }
        [Phone]
        public string Phone { get; set; }
        [DataType(DataType.Currency)]
        public decimal Salary { get; set; }

        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        [DisplayName("Date of Creation")]
        public DateTime HiringDate { get; set; }
        public DateTime CreateAt { get; set; }
        public int? DepartmentId { get; set; }
        public string? DepartmentName { get; set; }
        public string? ImageName { get; set; }
        public IFormFile? Image { get; set; }
    }
}
