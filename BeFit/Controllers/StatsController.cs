using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BeFit.Data;
using BeFit.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace BeFit.Controllers
{
    [Authorize]
    public class StatsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public StatsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var fourWeeksAgo = DateTime.Now.AddDays(-28);

            var stats = await _context.ExerciseTypes
                .Select(exerciseType => new StatsViewModel
                {
                    ExerciseName = exerciseType.Name,
                    ExerciseCounter = _context.TrainingEntries
                        .Count(entry => entry.ExerciseTypeId == exerciseType.Id
                            && entry.TrainingSession.Start >= fourWeeksAgo
                            && entry.UserId == userId),
                    TotalReps = _context.TrainingEntries
                        .Where(entry => entry.ExerciseTypeId == exerciseType.Id
                            && entry.TrainingSession.Start >= fourWeeksAgo
                            && entry.UserId == userId)
                        .Sum(entry => entry.Sets * entry.Reps),
                    AverageLoad = _context.TrainingEntries
                        .Where(entry => entry.ExerciseTypeId == exerciseType.Id
                            && entry.TrainingSession.Start >= fourWeeksAgo
                            && entry.UserId == userId)
                        .Average(entry => (double?)entry.Load) ?? 0,
                    MaxLoad = _context.TrainingEntries
                        .Where(entry => entry.ExerciseTypeId == exerciseType.Id
                            && entry.TrainingSession.Start >= fourWeeksAgo
                            && entry.UserId == userId)
                        .Max(entry => (double?)entry.Load) ?? 0
                }).ToListAsync();

            return View(stats);
        }
    }
}
