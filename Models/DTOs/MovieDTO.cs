namespace RESTJwt.Models.DTOs
{
    using System.ComponentModel.DataAnnotations;

    public class MovieDTO
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public double Rating { get; set; }
    }
}
