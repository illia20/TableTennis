using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TableTennis;

namespace TableTennis.Controllers
{
    public class PlayerRacketsController : Controller
    {
        private readonly TableTennisDBContext _context;

        public PlayerRacketsController(TableTennisDBContext context)
        {
            _context = context;
        }

        // GET: PlayerRackets
        public async Task<IActionResult> Index()
        {
            var tableTennisDBContext = _context.PlayerRackets.Include(p => p.IdNavigation).Include(p => p.Player);
            return View(await tableTennisDBContext.ToListAsync());
        }

        // GET: PlayerRackets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var playerRackets = await _context.PlayerRackets
                .Include(p => p.IdNavigation)
                .Include(p => p.Player)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (playerRackets == null)
            {
                return NotFound();
            }

            return View(playerRackets);
        }

        // GET: PlayerRackets/Create
        public IActionResult Create()
        {
            ViewData["Id"] = new SelectList(_context.Racket, "Id", "Id");
            ViewData["PlayerId"] = new SelectList(_context.Player, "Id", "Name");
            return View();
        }

        // POST: PlayerRackets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PlayerId,RacketId")] PlayerRackets playerRackets)
        {
            if (ModelState.IsValid)
            {
                _context.Add(playerRackets);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Id"] = new SelectList(_context.Racket, "Id", "Id", playerRackets.Id);
            ViewData["PlayerId"] = new SelectList(_context.Player, "Id", "Name", playerRackets.PlayerId);
            return View(playerRackets);
        }

        // GET: PlayerRackets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var playerRackets = await _context.PlayerRackets.FindAsync(id);
            if (playerRackets == null)
            {
                return NotFound();
            }
            ViewData["Id"] = new SelectList(_context.Racket, "Id", "Id", playerRackets.Id);
            ViewData["PlayerId"] = new SelectList(_context.Player, "Id", "Name", playerRackets.PlayerId);
            return View(playerRackets);
        }

        // POST: PlayerRackets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PlayerId,RacketId")] PlayerRackets playerRackets)
        {
            if (id != playerRackets.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(playerRackets);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlayerRacketsExists(playerRackets.Id))
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
            ViewData["Id"] = new SelectList(_context.Racket, "Id", "Id", playerRackets.Id);
            ViewData["PlayerId"] = new SelectList(_context.Player, "Id", "Name", playerRackets.PlayerId);
            return View(playerRackets);
        }

        // GET: PlayerRackets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var playerRackets = await _context.PlayerRackets
                .Include(p => p.IdNavigation)
                .Include(p => p.Player)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (playerRackets == null)
            {
                return NotFound();
            }

            return View(playerRackets);
        }

        // POST: PlayerRackets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var playerRackets = await _context.PlayerRackets.FindAsync(id);
            _context.PlayerRackets.Remove(playerRackets);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlayerRacketsExists(int id)
        {
            return _context.PlayerRackets.Any(e => e.Id == id);
        }
    }
}
