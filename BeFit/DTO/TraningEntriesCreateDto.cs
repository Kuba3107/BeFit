

namespace BeFit.DTO
{
    public class TraningEntriesCreateDto
    {
        public int Id { get; set; }
        public int TrainingSessionId { get; set; }
        public int ExerciseTypeId { get; set; }
        public int Reps { get; set; }
        public int Sets { get; set; }
        public int Load { get; set; }
    }
}
