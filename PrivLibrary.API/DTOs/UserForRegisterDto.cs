using System.ComponentModel.DataAnnotations;

namespace PrivLibrary.API.DTOs
{
    public class UserForRegisterDto
    {
        [Required]
        public string  Username { get; set; }


        [Required]
        [StringLength(8, MinimumLength=4, ErrorMessage="Password must have 4 characters at least.")]
        public string Password { get; set; }
    }
}