using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SurveyApp.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SurveyApp.Controllers
{
    public class SurveysController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly string _uploadFolderPath;

        public SurveysController(ApplicationDbContext context)
        {
            _context = context;
            _uploadFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
            if (!Directory.Exists(_uploadFolderPath))
            {
                Directory.CreateDirectory(_uploadFolderPath);
            }
        }

        // GET: Surveys
        public async Task<IActionResult> Index()
        {
            return View(await _context.Surveys.ToListAsync());
        }


        // GET: Surveys/Create
        public IActionResult Create()
        {
            ViewBag.FeedbackTypes = GetFeedbackTypes();
            ViewBag.Services = GetServices();
            return View();
        }

        // POST: Surveys/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Survey survey, IFormFile documentPath)
        {
            if (ModelState.IsValid)
            {
                if (documentPath != null && documentPath.Length > 0)
                {
                    var fileName = Path.GetFileName(documentPath.FileName);
                    var filePath = Path.Combine(_uploadFolderPath, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await documentPath.CopyToAsync(stream);
                    }

                    survey.DocumentPath = fileName;
                }

                _context.Add(survey);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.FeedbackTypes = GetFeedbackTypes();
            ViewBag.Services = GetServices();
            return View(survey);
        }

        // GET: Surveys/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var survey = await _context.Surveys.FindAsync(id);
            if (survey == null)
            {
                return NotFound();
            }

            ViewBag.FeedbackTypes = GetFeedbackTypes();
            ViewBag.Services = GetServices();
            return View(survey);
        }

        // POST: Surveys/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Survey survey, IFormFile documentPath)
        {
            if (id != survey.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (documentPath != null && documentPath.Length > 0)
                    {
                        var fileName = Path.GetFileName(documentPath.FileName);
                        var filePath = Path.Combine(_uploadFolderPath, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await documentPath.CopyToAsync(stream);
                        }

                        survey.DocumentPath = fileName;
                    }

                    _context.Update(survey);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SurveyExists(survey.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.FeedbackTypes = GetFeedbackTypes();
            ViewBag.Services = GetServices();
            return View(survey);
        }

        // GET: Surveys/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var survey = await _context.Surveys.FindAsync(id);
            if (survey == null)
            {
                return NotFound();
            }
            
            return View(survey);
        }

        // POST: Surveys/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var survey = await _context.Surveys.FindAsync(id);
            if (survey == null)
            {
                return NotFound();
            }

            _context.Surveys.Remove(survey);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SurveyExists(int id)
        {
            return _context.Surveys.Any(e => e.Id == id);
        }

        private IEnumerable<SelectListItem> GetFeedbackTypes()
        {
            var feedbackTypes = new List<string> { "Positive", "Neutral", "Negative" };
            return feedbackTypes.Select(ft => new SelectListItem { Value = ft, Text = ft });
        }

        private IEnumerable<SelectListItem> GetServices()
        {
            var services = new List<string> { "Service 1", "Service 2", "Service 3" };
            return services.Select(s => new SelectListItem { Value = s, Text = s });
        }
    }
}
