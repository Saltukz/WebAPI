using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    public class TodoUpdateModel
    {
        [Required]
        [StringLength(250)]
        public string Text { get; set; }
        [StringLength(500)]
        public string Description { get; set; }

        public bool IsCompleted { get; set; }

    }
}
