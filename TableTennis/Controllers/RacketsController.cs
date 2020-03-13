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
    public class RacketsController : Controller
    {
        private readonly TableTennisDBContext _context;

        public RacketsController(TableTennisDBContext context)
        {
            _context = context;
        }

        // GET: Rackets
        public async Task<IActionResult> Index()
        {
            var tableTennisDBContext = _context.Racket.Include(r => r.Bhrubber).Include(r => r.Blade).Include(r => r.Fhrubber);
            return View(await tableTennisDBContext.ToListAsync());
        }

        // GET: Rackets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var racket = await _context.Racket
                .Include(r => r.Bhrubber)
                .Include(r => r.Blade)
                .Include(r => r.Fhrubber)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (racket == null)
            {
                return NotFound();
            }

            return View(racket);
        }

        // GET: Rackets/Create
        public IActionResult Create()
        {
            ViewData["BhrubberId"] = new SelectList(_context.Rubber, "Id", "RubberName");
            ViewData["BladeId"] = new SelectList(_context.Blade, "Id", "BladeName");
            ViewData["FhrubberId"] = new SelectList(_context.Rubber, "Id", "RubberName");
            return View();
        }

        // POST: Rackets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,BladeId,FhrubberId,BhrubberId")] Racket racket)
        {
            if (ModelState.IsValid)
            {
                _context.Add(racket);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BhrubberId"] = new SelectList(_context.Rubber, "Id", "RubberName", racket.BhrubberId);
            ViewData["BladeId"] = new SelectList(_context.Blade, "Id", "BladeName", racket.BladeId);
            ViewData["FhrubberId"] = new SelectList(_context.Rubber, "Id", "RubberName", racket.FhrubberId);
            return View(racket);
        }

        // GET: Rackets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var racket = await _context.Racket.FindAsync(id);
            if (racket == null)
            {
                return NotFound();
            }
            ViewData["BhrubberId"] = new SelectList(_context.Rubber, "Id", "RubberName", racket.BhrubberId);
            ViewData["BladeId"] = new SelectList(_context.Blade, "Id", "BladeName", racket.BladeId);
            ViewData["FhrubberId"] = new SelectList(_context.Rubber, "Id", "RubberName", racket.FhrubberId);
            return View(racket);
        }

        // POST: Rackets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BladeId,FhrubberId,BhrubberId")] Racket racket)
        {
            if (id != racket.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(racket);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RacketExists(racket.Id))
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
            ViewData["BhrubberId"] = new SelectList(_context.Rubber, "Id", "RubberName", racket.BhrubberId);
            ViewData["BladeId"] = new SelectList(_context.Blade, "Id", "BladeName", racket.BladeId);
            ViewData["FhrubberId"] = new SelectList(_context.Rubber, "Id", "RubberName", racket.FhrubberId);
            return View(racket);
        }

        // GET: Rackets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var racket = await _context.Racket
                .Include(r => r.Bhrubber)
                .Include(r => r.Blade)
                .Include(r => r.Fhrubber)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (racket == null)
            {
                return NotFound();
            }

            return View(racket);
        }

        // POST: Rackets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            using(var context = new TableTennisDBContext())
            {
                var rackets = context.Racket.Include(w => w.PlayerRackets).SingleOrDefault(w => w.Id == id);
                var child3 = rackets.PlayerRackets;
                
                context.PlayerRackets.Remove(child3);
                context.SaveChanges();
            }
            var racket = await _context.Racket.FindAsync(id);
            _context.Racket.Remove(racket);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RacketExists(int id)
        {
            return _context.Racket.Any(e => e.Id == id);
        }
    }
}
