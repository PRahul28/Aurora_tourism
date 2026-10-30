using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FormApp.Models;
using FormApp.Data;
using Microsoft.AspNetCore.Authorization;
namespace FormApp.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;

    public HomeController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [HttpPost]
public async Task<IActionResult> Submit([FromBody] Submission submission)
{
    try
    {
        if (!ModelState.IsValid)
        {
            return Json(new
            {
                success = false,
                message = "Please fill in the required fields."
            });
        }

        submission.CreatedAt = DateTime.Now;

        _context.Submissions.Add(submission);

        await _context.SaveChangesAsync();

        return Json(new
        {
            success = true,
            message = "Submitted successfully."
        });
    }
    catch (Exception ex)
{
    Console.WriteLine("FULL ERROR:");
    Console.WriteLine(ex.ToString());

    Console.WriteLine("INNER ERROR:");
    Console.WriteLine(ex.InnerException?.Message);

    return StatusCode(500, new
    {
        success = false,
        message = ex.InnerException?.Message ?? ex.Message
    });
}
}

    [ResponseCache(
        Duration = 0,
        Location = ResponseCacheLocation.None,
        NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel
        {
            RequestId = Activity.Current?.Id
                        ?? HttpContext.TraceIdentifier
        });
    }
}