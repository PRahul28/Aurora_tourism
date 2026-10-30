
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FormApp.Data;

namespace FormApp.Controllers;

[Authorize]
public class RoomsController : Controller
{
    private readonly ApplicationDbContext _context;

    public RoomsController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var rooms = await _context.Rooms
            .Where(r => r.IsAvailable)
            .ToListAsync();

        return View(rooms);
    }
}
