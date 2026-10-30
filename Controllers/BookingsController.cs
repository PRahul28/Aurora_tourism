
using System.Security.Claims;
using FormApp.Data;
using FormApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FormApp.Controllers;

[Authorize]

public class BookingsController : Controller
{
    private readonly ApplicationDbContext _context;

    public BookingsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Create(int roomId)
    {
        var room = await _context.Rooms
            .FirstOrDefaultAsync(r => r.Id == roomId && r.IsAvailable);

        if (room == null)
            return NotFound();

        return View(room);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        int roomId,
        DateTime checkInDate,
        DateTime checkOutDate,
        int guests)
    {
        var room = await _context.Rooms
            .FirstOrDefaultAsync(r => r.Id == roomId && r.IsAvailable);

        if (room == null)
            return NotFound();

        if (checkInDate.Date < DateTime.Today ||
            checkOutDate.Date <= checkInDate.Date)
        {
            ViewBag.BookingError =
                "Please select valid check-in and check-out dates.";
            return View(room);
        }

        if (guests < 1 || guests > room.Capacity)
        {
            ViewBag.BookingError =
                $"This room allows 1 to {room.Capacity} guests.";
            return View(room);
        }

        // Reject dates that overlap another active booking.
        bool alreadyBooked = await _context.Bookings.AnyAsync(b =>
            b.RoomId == roomId &&
            b.BookingStatus != "Cancelled" &&
            b.CheckInDate < checkOutDate.Date &&
            b.CheckOutDate > checkInDate.Date);

        if (alreadyBooked)
        {
            ViewBag.BookingError =
                "This room is already booked for some of those dates. Please choose different dates.";
            return View(room);
        }

        var email = User.FindFirstValue(ClaimTypes.Email);

        if (string.IsNullOrWhiteSpace(email))
        {
            ViewBag.BookingError =
                "Your Google account email could not be identified. Please sign in again.";
            return View(room);
        }

        int nights = (checkOutDate.Date - checkInDate.Date).Days;

        var booking = new Booking
        {
            RoomId = room.Id,
            UserEmail = email,
            CheckInDate = checkInDate.Date,
            CheckOutDate = checkOutDate.Date,
            Guests = guests,
            PricePerNight = room.PricePerNight,
            TotalPrice = room.PricePerNight * nights,
            BookingStatus = "Confirmed",
            CreatedAt = DateTime.Now,
            Room = room
        };

        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync();

        return View("Confirmation", booking);
    }

[HttpGet]
public async Task<IActionResult> MyBookings()
{
    var email = User.FindFirstValue(ClaimTypes.Email);

    if (string.IsNullOrWhiteSpace(email))
    {
        return Challenge();
    }

    var bookings = await _context.Bookings
        .Include(b => b.Room)
        .Where(b => b.UserEmail == email)
        .OrderByDescending(b => b.CreatedAt)
        .ToListAsync();

    return View(bookings);
}

}
