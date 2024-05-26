using Microsoft.AspNetCore.Mvc;
using Muzej.BLL;
using Muzej.DomainObjects;
using Muzej.Repository.Interfaces;

namespace Muzej.UI.Controllers
{
    public class JobController : Controller
    {
        private readonly ILogger<JobController> _logger;
        private readonly IRepositoryWrapper _repository;

        public JobController(ILogger<JobController> logger, IRepositoryWrapper repository)
        {
            _logger = logger;
            _repository = repository;
        }

        // GET: Job
        public IActionResult Index()
        {
            ViewBag.Role = HttpContext.Session.GetString("Role");

            //chek access rights
            if (ViewBag.Role != Roles.TimetableManager)
            {
                return RedirectToAction("UnauthorizedAccess", "Home");
            }

            var jobsBll = new JobsBLL(_repository);
            var jobs = jobsBll.GetJobs();
            return View(jobs);
        }

        // GET: Job/Create
        public IActionResult Create()
        {
            ViewBag.Role = HttpContext.Session.GetString("Role");

            //chek access rights
            if (ViewBag.Role != Roles.TimetableManager)
            {
                return RedirectToAction("UnauthorizedAccess", "Home");
            }
            return View();
        }

        // POST: Job/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Job job)
        {
            ViewBag.Role = HttpContext.Session.GetString("Role");

            //chek access rights
            if (ViewBag.Role != Roles.TimetableManager)
            {
                return RedirectToAction("UnauthorizedAccess", "Home");
            }

            if (ModelState.IsValid)
            {
                var jobsBll = new JobsBLL(_repository);
                jobsBll.CreateJob(job);
                return RedirectToAction(nameof(Index));
            }
            return View(job);
        }

        // GET: Job/Edit/5
        public IActionResult Edit(int id)
        {
            ViewBag.Role = HttpContext.Session.GetString("Role");

            //chek access rights
            if (ViewBag.Role != Roles.TimetableManager)
            {
                return RedirectToAction("UnauthorizedAccess", "Home");
            }
            var jobsBll = new JobsBLL(_repository);
            var job = jobsBll.GetJob(id);
            if (job == null)
            {
                return NotFound();
            }
            return View(job);
        }

        // POST: Job/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Job job)
        {
            ViewBag.Role = HttpContext.Session.GetString("Role");

            //chek access rights
            if (ViewBag.Role != Roles.TimetableManager)
            {
                return RedirectToAction("UnauthorizedAccess", "Home");
            }
            if (id != job.JobId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var jobsBll = new JobsBLL(_repository);
                if (jobsBll.UpdateJob(job))
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Unable to update job.");
            }
            return View(job);
        }

        public IActionResult ActionInfo(string message, bool successful)
        {
            ViewBag.Role = HttpContext.Session.GetString("Role");

            //chek access rights
            if (ViewBag.Role != Roles.TimetableManager)
            {
                return RedirectToAction("UnauthorizedAccess", "Home");
            }

            ViewBag.Role = HttpContext.Session.GetString("Role");
            ViewBag.Successful = successful;
            ViewBag.Message = message;
            return View();
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            ViewBag.Role = HttpContext.Session.GetString("Role");

            //chek access rights
            if (ViewBag.Role != Roles.TimetableManager)
            {
                return RedirectToAction("UnauthorizedAccess", "Home");
            }

            try
            {
                var jobsBll = new JobsBLL(_repository);
                var success = jobsBll.DeleteJob(id);
                if (success)
                {
                    return RedirectToAction("ActionInfo", new { message = $"Job with ID {id} was deleted successfully.", successful = true });
                }
                else
                {
                    return RedirectToAction("ActionInfo", new { message = $"Could not delete job because there are employees who have that job.", successful = false });
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return RedirectToAction("ActionInfo", new { message = "Error when deleting job.", successful = false });
            }
        }
    }
}
