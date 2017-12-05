using System.ComponentModel.DataAnnotations;

namespace Presentation.Models
{
    public class LectureCreateModel
    {
        [Required(ErrorMessage = "A title is required.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "A description is required.")]
        [StringLength(2000, ErrorMessage = "Maximum number of characters is 2000!")]
        public string Description { get; set; }
    }
}
