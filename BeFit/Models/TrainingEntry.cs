using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeFit.Models
{
    public class TrainingEntry
    {
        
        public int Id { get; set; }

        public string UserId { get; set; } = null!;

        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }

        [Required]
        [Display(Name = "Sesja treningowa")]
        public int TrainingSessionId { get; set; }
        [Display(Name = "Sesja treningowa")]
        public TrainingSession? TrainingSession { get; set; }

        [Required]
        [Display(Name = "Nazwa ćwiczenia")]
        public int ExerciseTypeId { get; set; }
        [Display(Name = "Nazwa ćwiczenia")]
        public ExerciseType? ExerciseType { get; set; }

        
        [Range(0, 1000)]
        [Display(Name = "Obciążenie [kg]")]
        public double Load { get; set; }

        [Range(1, 100)]
        [Display(Name = "Serie")]
        public int Sets { get; set; }

        [Range(1, 1000)]
        [Display(Name = "Powtórzenia w serii")]
        public int Reps { get; set; }
    }
}
