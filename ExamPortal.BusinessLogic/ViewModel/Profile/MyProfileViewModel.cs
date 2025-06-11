using System.ComponentModel.DataAnnotations;

namespace ExamPortal.BusinessLogic.ViewModel.Profile
{
    public class MyProfileViewModel
    {
        [RegularExpression(@"^(?![\s\-\(\)\[\]&'/\+,]*$)(?=.*[A-Za-zÀ-ÿ])([A-Za-zÀ-ÿ\s\-\(\)\[\]&'/\+,.]+)$", ErrorMessage = "First Name is not valid.")]
        [StringLength(256)]
        public string FirstName { get; set; } = null!;

        [StringLength(256)]
        [RegularExpression(@"^(?![\s\-\(\)\[\]&'/\+,]*$)(?=.*[A-Za-zÀ-ÿ])([A-Za-zÀ-ÿ\s\-\(\)\[\]&'/\+,.]+)$", ErrorMessage = "Last Name is not valid.")]
        public string LastName { get; set; } = null!;

        [RegularExpression(@"^(\+\d{1,3}[- ]?)?\d{10}$", ErrorMessage = "Invalid phone number format.")]
        public string? Phonenumber { get; set; }
        public string? Address { get; set; }

        [StringLength(10)]
        [RegularExpression(@"^\d{6}(-\d{4})?$", ErrorMessage = "Invalid zipcode format.")]
        public string? Zipcode { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }
        public string? ProfileImage { get; set; }

    }
}