using System.ComponentModel.DataAnnotations;

namespace webapiCourse.models.DTOs
{
    public class RegisterDTO
    {

        public string UserName { get; set; }
        public string Password { get; set; }
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        public string Email { get; set; }
    }
}
