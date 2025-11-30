using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeFit.Models
{
    public class TrainingSession
    {
        public int Id { get; set; }
        public string UserId { get; set; } = null!;

        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }

        [Required]
        [Display(Name = "Początek sesji")]
        public DateTime Start { get; set; }

        [Required]
        [Display(Name = "Koniec sesji")]
        public DateTime End { get; set; }
 
    }
}
