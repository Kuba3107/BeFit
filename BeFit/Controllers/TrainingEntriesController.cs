using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BeFit.Data;
using BeFit.Models;
using BeFit.DTO;
using Microsoft.AspNetCore.Identity;
using Humanizer;
using Microsoft.AspNetCore.Authorization;

namespace BeFit.Controllers
{
    [Authorize]
    public class TrainingEntriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private Task<ApplicationUser?> GetCurrentUserAsync() => _userManager.GetUserAsync(User);
        private string GetUserId() => _userManager.GetUserId(User);

        public TrainingEntriesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: TrainingEntries
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var exercises = await _context.TrainingEntries
                                 .Where(e => e.UserId == userId)
                                 .Include(e => e.TrainingSession)
                                 .Include(e => e.ExerciseType)
                                 .ToListAsync();
            return View(exercises);
        }

        // GET: TrainingEntries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainingEntry = await _context.TrainingEntries
                .Include(t => t.ExerciseType)
                .Include(t => t.TrainingSession)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (trainingEntry == null)
            {
                return NotFound();
            }

            return View(trainingEntry);
        }

        // GET: TrainingEntries/Create
        public IActionResult Create()
        {
            ViewData["ExerciseTypeId"] = new SelectList(_context.ExerciseTypes, "Id", "Name");
            var userId = GetUserId();

            var sessions = _context.TrainingSessions
                .Where(t => t.UserId == userId)
                .ToList();

            ViewData["TrainingSessionId"] = new SelectList(sessions, "Id", "Start");
            return View();
        }

        // POST: TrainingEntries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TraningEntriesCreateDto dto)
        {
            var entity = new TrainingEntry
            {
                TrainingSessionId = dto.TrainingSessionId,
                ExerciseTypeId = dto.ExerciseTypeId,
                Reps = dto.Reps,
                Sets = dto.Sets,
                Load = dto.Load,
                UserId =  GetUserId()
                
            };
            var userId = GetUserId();

            var sessions = _context.TrainingSessions
                .Where(t => t.UserId == userId)
                .ToList();

            ViewData["TrainingSessionId"] = new SelectList(sessions, "Id", "Start");

            _context.Add(entity);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: TrainingEntries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainingEntry = await _context.TrainingEntries.FindAsync(id);
            if (trainingEntry == null)
            {
                return NotFound();
            }
            ViewData["ExerciseTypeId"] = new SelectList(_context.ExerciseTypes, "Id", "Name", trainingEntry.ExerciseTypeId);

            var userId = GetUserId();

            var sessions = _context.TrainingSessions
                .Where(t => t.UserId == userId)
                .ToList();

            ViewData["TrainingSessionId"] = new SelectList(sessions, "Id", "Start", trainingEntry.TrainingSessionId);

            return View(trainingEntry);
        }

        // POST: TrainingEntries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TrainingEntry entryFromForm)
        {
            if (id != entryFromForm.Id)
                return NotFound();

            ModelState.Remove("UserId");

            var entry = await _context.TrainingEntries.FindAsync(id);
            if (entry == null)
                return NotFound();

            entry.TrainingSessionId = entryFromForm.TrainingSessionId;
            entry.ExerciseTypeId = entryFromForm.ExerciseTypeId;
            entry.Load = entryFromForm.Load;
            entry.Sets = entryFromForm.Sets;
            entry.Reps = entryFromForm.Reps;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        // GET: TrainingEntries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainingEntry = await _context.TrainingEntries
                .Include(t => t.ExerciseType)
                .Include(t => t.TrainingSession)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (trainingEntry == null)
            {
                return NotFound();
            }

            return View(trainingEntry);
        }

        // POST: TrainingEntries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var trainingEntry = await _context.TrainingEntries.FindAsync(id);
            if (trainingEntry != null)
            {
                _context.TrainingEntries.Remove(trainingEntry);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TrainingEntryExists(int id)
        {
            return _context.TrainingEntries.Any(e => e.Id == id);
        }
    }
}
