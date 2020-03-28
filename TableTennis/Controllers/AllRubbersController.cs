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
    public class AllRubbersController : Controller
    {
        private readonly TableTennisDBContext _context;

        public AllRubbersController(TableTennisDBContext context)
        {
            _context = context;
        }

        // GET: AllRubbers
        public async Task<IActionResult> Index()
        {
            var tableTennisDBContext = _context.Rubber.Include(r => r.Factory);
            return View(await tableTennisDBContext.ToListAsync());
        }

        // GET: AllRubbers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rubber = await _context.Rubber
                .Include(r => r.Factory)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rubber == null)
            {
                return NotFound();
            }

            return View(rubber);
        }

        // GET: AllRubbers/Create
        public IActionResult Create()
        {
            ViewData["FactoryId"] = new SelectList(_context.Factory, "Id", "FactoryName");
            return View();
        }

        // POST: AllRubbers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FactoryId,RubberName,Pimples")] Rubber rubber)
        {
            if (ModelState.IsValid)
            {
                if(RubberExists(rubber.FactoryId, rubber.RubberName))
                {
                    return RedirectToAction(nameof(Index));
                }
                _context.Add(rubber);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FactoryId"] = new SelectList(_context.Factory, "Id", "FactoryName", rubber.FactoryId);
            return View(rubber);
        }

        // GET: AllRubbers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rubber = await _context.Rubber.FindAsync(id);
            if (rubber == null)
            {
                return NotFound();
            }
            ViewData["FactoryId"] = new SelectList(_context.Factory, "Id", "FactoryName", rubber.FactoryId);
            return View(rubber);
        }

        // POST: AllRubbers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FactoryId,RubberName,Pimples")] Rubber rubber)
        {
            if (id != rubber.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rubber);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RubberExists(rubber.Id))
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
            ViewData["FactoryId"] = new SelectList(_context.Factory, "Id", "FactoryName", rubber.FactoryId);
            return View(rubber);
        }

        // GET: AllRubbers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rubber = await _context.Rubber
                .Include(r => r.Factory)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rubber == null)
            {
                return NotFound();
            }

            return View(rubber);
        }

        // POST: AllRubbers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            using(var context = new TableTennisDBContext())
            {
                var rubber1 = context.Rubber.Include(r => r.RacketBhrubber).SingleOrDefault(r => r.Id == id);
                foreach (var child2 in rubber1.RacketBhrubber.ToList())
                {
                    var racket = context.Racket.Include(w => w.PlayerRackets).SingleOrDefault(w => w.Id == child2.Id);
                    // TODO remove player rackets?
                    var child3 = racket.PlayerRackets;
                    context.PlayerRackets.Remove(child3);
                    context.Racket.Remove(child2);
                }
                var rubber2 = context.Rubber.Include(r => r.RacketFhrubber).SingleOrDefault(r => r.Id == id);
                foreach (var child2 in rubber2.RacketFhrubber.ToList())
                {
                    var racket = context.Racket.Include(w => w.PlayerRackets).SingleOrDefault(w => w.Id == child2.Id);
                    // TODO remove player rackets?
                    var child3 = racket.PlayerRackets;
                    context.PlayerRackets.Remove(child3);
                    context.Racket.Remove(child2);
                }
                context.SaveChanges();
            }
            var rubber = await _context.Rubber.FindAsync(id);
            _context.Rubber.Remove(rubber);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RubberExists(int fId, string rubberName)
        {
            bool rubber1 = _context.Rubber.Any(e => e.RubberName == rubberName);
            bool factory1 = _context.Rubber.Any(e => e.FactoryId == fId);
            return factory1 && rubber1;
        }
        private bool RubberExists(int id)
        {
            return _context.Rubber.Any(e => e.Id == id);
        }
    }
}
