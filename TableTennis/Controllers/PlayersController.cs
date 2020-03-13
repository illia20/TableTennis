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
    public class PlayersController : Controller
    {
        private readonly TableTennisDBContext _context;

        public PlayersController(TableTennisDBContext context)
        {
            _context = context;
        }

        // GET: Players
        public async Task<IActionResult> Index()
        {
            var tableTennisDBContext = _context.Player.Include(p => p.Country);
            return View(await tableTennisDBContext.ToListAsync());
        }

        // GET: Players/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var player = await _context.Player
                .Include(p => p.Country)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (player == null)
            {
                return NotFound();
            }
            /*
            return View(player); */
            return RedirectToAction("Index", "Games", new { Id = player.Id, Name = player.Name});
        }

        // GET: Players/Create
        public IActionResult Create()
        {
            ViewData["CountryId"] = new SelectList(_context.Country, "Id", "CountryName");
            return View();
        }

        // POST: Players/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,CountryId,Arm")] Player player)
        {
            if (ModelState.IsValid)
            {
                _context.Add(player);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CountryId"] = new SelectList(_context.Country, "Id", "CountryName", player.CountryId);
            return View(player);
        }

        // GET: Players/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var player = await _context.Player.FindAsync(id);
            if (player == null)
            {
                return NotFound();
            }
            ViewData["CountryId"] = new SelectList(_context.Country, "Id", "CountryName", player.CountryId);
            return View(player);
        }

        // POST: Players/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,CountryId,Arm")] Player player)
        {
            if (id != player.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(player);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlayerExists(player.Id))
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
            ViewData["CountryId"] = new SelectList(_context.Country, "Id", "CountryName", player.CountryId);
            return View(player);
        }

        // GET: Players/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var player = await _context.Player
                .Include(p => p.Country)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (player == null)
            {
                return NotFound();
            }

            return View(player);
        }

        // POST: Players/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            using(var context = new TableTennisDBContext())
            {
                var player2 = context.Player.Include(q => q.GamePlayer2).SingleOrDefault(q => q.Id == id);
                foreach (var child1 in player2.GamePlayer2.ToList())
                {
                    context.Game.Remove(child1);
                }
                var player1 = context.Player.Include(q => q.GamePlayer1).SingleOrDefault(q => q.Id == id);
                foreach (var child1 in player1.GamePlayer1.ToList())
                {
                    context.Game.Remove(child1);
                }
                var player3 = context.Player.Include(q => q.PlayerRackets).SingleOrDefault(q => q.Id == id);
                foreach (var child1 in player3.PlayerRackets.ToList())
                {
                    context.PlayerRackets.Remove(child1);
                }
                context.SaveChanges();
            }
            var player = await _context.Player.FindAsync(id);
            _context.Player.Remove(player);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlayerExists(int id)
        {
            return _context.Player.Any(e => e.Id == id);
        }
    }
}
