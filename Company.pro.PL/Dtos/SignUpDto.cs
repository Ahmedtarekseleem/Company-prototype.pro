using System.ComponentModel.DataAnnotations;

namespace Company.pro.PL.Dtos
{
	public class SignUpDto
	{
        [Required(ErrorMessage ="UserName Is Required !!")]
        public string UserName { get; set; }
		[Required(ErrorMessage = "FirstName Is Required !!")]
		public string FirstName { get; set; }
		[Required(ErrorMessage = "LastName Is Required !!")]
		public string LastName { get; set; }
		[Required(ErrorMessage = "Email Is Required !!")]
		[EmailAddress]
		public string Email { get; set; }
		[Required(ErrorMessage = "Password Is Required !!")]
		[DataType(DataType.Password)]
		public string Password { get; set; }
		[Required(ErrorMessage = "Confirm Password Is Required !!")]
		[DataType(DataType.Password)]
		[Compare(nameof(Password),ErrorMessage ="Confirm password dose not match Password")]
		public string ConfirmPassword { get; set; }
        public bool IsAgree { get; set; }

		



		 



		     





    }
}
