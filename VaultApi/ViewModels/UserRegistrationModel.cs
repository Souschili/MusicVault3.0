using System.ComponentModel.DataAnnotations;

namespace VaultApi.ViewModels
{
    public class UserRegistrationModel
    {
        [Required]
        public string Login { get; set; }

        [Required,DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required,DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
