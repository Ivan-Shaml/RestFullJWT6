namespace RESTJwt.Models
{
    using System.ComponentModel.DataAnnotations;
    public class Movie
    {
        [Key]
        public virtual int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public double Rating { get; set; }
    }
}
