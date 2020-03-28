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
    public class AllBladesController : Controller
    {
        private readonly TableTennisDBContext _context;

        public AllBladesController(TableTennisDBContext context)
        {
            _context = context;
        }

        // GET: AllBlades
        public async Task<IActionResult> Index()
        {
            var tableTennisDBContext = _context.Blade.Include(b => b.Factory);
            return View(await tableTennisDBContext.ToListAsync());
        }

        // GET: AllBlades/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blade = await _context.Blade
                .Include(b => b.Factory)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blade == null)
            {
                return NotFound();
            }

            return View(blade);
        }

        // GET: AllBlades/Create
        public IActionResult Create()
        {
            ViewData["FactoryId"] = new SelectList(_context.Factory, "Id", "FactoryName");
            return View();
        }

        // POST: AllBlades/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FactoryId,BladeName,Composite")] Blade blade)
        {
            if (ModelState.IsValid)
            {
                if (BladeExists(blade.BladeName))
                {
                    return RedirectToAction(nameof(Index));
                }
                _context.Add(blade);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FactoryId"] = new SelectList(_context.Factory, "Id", "FactoryName", blade.FactoryId);
            return View(blade);
        }

        // GET: AllBlades/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blade = await _context.Blade.FindAsync(id);
            if (blade == null)
            {
                return NotFound();
            }
            ViewData["FactoryId"] = new SelectList(_context.Factory, "Id", "FactoryName", blade.FactoryId);
            return View(blade);
        }

        // POST: AllBlades/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FactoryId,BladeName,Composite")] Blade blade)
        {
            if (id != blade.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(blade);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BladeExists(blade.Id))
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
            ViewData["FactoryId"] = new SelectList(_context.Factory, "Id", "FactoryName", blade.FactoryId);
            return View(blade);
        }

        // GET: AllBlades/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blade = await _context.Blade
                .Include(b => b.Factory)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blade == null)
            {
                return NotFound();
            }

            return View(blade);
        }

        // POST: AllBlades/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            using(var context = new TableTennisDBContext())
            {
                var blade1 = context.Blade.Include(b => b.Racket).SingleOrDefault(b => b.Id == id);
                foreach (var child2 in blade1.Racket.ToList())
                {

                    var racket = context.Racket.Include(w => w.PlayerRackets).SingleOrDefault(w => w.Id == child2.Id);
                    var child3 = racket.PlayerRackets;
                    context.PlayerRackets.Remove(child3);
                    context.Racket.Remove(child2);
                }
                context.SaveChanges();
            }
            var blade = await _context.Blade.FindAsync(id);
            _context.Blade.Remove(blade);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool BladeExists(string bladeName)
        {
            return _context.Blade.Any(e => e.BladeName == bladeName);
        }
        private bool BladeExists(int id)
        {
            return _context.Blade.Any(e => e.Id == id);
        }
    }
}
