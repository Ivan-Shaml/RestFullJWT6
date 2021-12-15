namespace RESTJwt.Models
{
    using System.ComponentModel.DataAnnotations;
    public class User
    {
        [Key]
        public Guid Uuid { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string EmailAddress { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Role { get; set; }

    }
}
