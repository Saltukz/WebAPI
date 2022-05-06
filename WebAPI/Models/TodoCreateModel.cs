using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    public class TodoCreateModel
    {
        [Required]
        [StringLength(250)]
        public string Text { get; set; }
        [StringLength(500)]
        public string Description { get; set; }

    }
}
