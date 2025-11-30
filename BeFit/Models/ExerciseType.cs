using System.ComponentModel.DataAnnotations;

namespace BeFit.Models
{
    public class ExerciseType
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Nazwa powinna mieć od 2 do 100 znaków.")]
        [Display(Name ="Nazwa ćwiczenia")]
        public string Name { get; set; } = null!;
    }
}
