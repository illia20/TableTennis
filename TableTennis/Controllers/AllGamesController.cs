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
    public class AllGamesController : Controller
    {
        private readonly TableTennisDBContext _context;

        public AllGamesController(TableTennisDBContext context)
        {
            _context = context;
        }

        // GET: AllGames
        public async Task<IActionResult> Index()
        {
            var tableTennisDBContext = _context.Game.Include(g => g.Player1).Include(g => g.Player2).Include(g => g.Racket1).Include(g => g.Racket2);
            return View(await tableTennisDBContext.ToListAsync());
        }

        // GET: AllGames/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Game
                .Include(g => g.Player1)
                .Include(g => g.Player2)
                .Include(g => g.Racket1)
                .Include(g => g.Racket2)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }

        // GET: AllGames/Create
        public IActionResult Create()
        {
            ViewData["Player1Id"] = new SelectList(_context.Player, "Id", "Name");
            ViewData["Player2Id"] = new SelectList(_context.Player, "Id", "Name");
            ViewData["Racket1Id"] = new SelectList(_context.PlayerRackets, "Id", "Id");
            ViewData["Racket2Id"] = new SelectList(_context.PlayerRackets, "Id", "Id");
            return View();
        }

        // POST: AllGames/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Player1Id,Player2Id,Racket1Id,Racket2Id,Result,Score,GameDate")] Game game)
        {
            if (ModelState.IsValid)
            {
                _context.Add(game);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Player1Id"] = new SelectList(_context.Player, "Id", "Name", game.Player1Id);
            ViewData["Player2Id"] = new SelectList(_context.Player, "Id", "Name", game.Player2Id);
            ViewData["Racket1Id"] = new SelectList(_context.PlayerRackets, "Id", "Id", game.Racket1Id);
            ViewData["Racket2Id"] = new SelectList(_context.PlayerRackets, "Id", "Id", game.Racket2Id);
            return View(game);
        }

        // GET: AllGames/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Game.FindAsync(id);
            if (game == null)
            {
                return NotFound();
            }
            ViewData["Player1Id"] = new SelectList(_context.Player, "Id", "Name", game.Player1Id);
            ViewData["Player2Id"] = new SelectList(_context.Player, "Id", "Name", game.Player2Id);
            ViewData["Racket1Id"] = new SelectList(_context.PlayerRackets, "Id", "Id", game.Racket1Id);
            ViewData["Racket2Id"] = new SelectList(_context.PlayerRackets, "Id", "Id", game.Racket2Id);
            return View(game);
        }

        // POST: AllGames/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Player1Id,Player2Id,Racket1Id,Racket2Id,Result,Score,GameDate")] Game game)
        {
            if (id != game.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(game);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GameExists(game.Id))
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
            ViewData["Player1Id"] = new SelectList(_context.Player, "Id", "Name", game.Player1Id);
            ViewData["Player2Id"] = new SelectList(_context.Player, "Id", "Name", game.Player2Id);
            ViewData["Racket1Id"] = new SelectList(_context.PlayerRackets, "Id", "Id", game.Racket1Id);
            ViewData["Racket2Id"] = new SelectList(_context.PlayerRackets, "Id", "Id", game.Racket2Id);
            return View(game);
        }

        // GET: AllGames/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Game
                .Include(g => g.Player1)
                .Include(g => g.Player2)
                .Include(g => g.Racket1)
                .Include(g => g.Racket2)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }

        // POST: AllGames/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var game = await _context.Game.FindAsync(id);
            _context.Game.Remove(game);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GameExists(int id)
        {
            return _context.Game.Any(e => e.Id == id);
        }
    }
}
