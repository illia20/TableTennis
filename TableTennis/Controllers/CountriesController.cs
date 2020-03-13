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
    public class CountriesController : Controller
    {
        private readonly TableTennisDBContext _context;

        public CountriesController(TableTennisDBContext context)
        {
            _context = context;
        }

        // GET: Countries
        public async Task<IActionResult> Index()
        {
            return View(await _context.Country.ToListAsync());
        }

        // GET: Countries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var country = await _context.Country
                .FirstOrDefaultAsync(m => m.Id == id);
            if (country == null)
            {
                return NotFound();
            }

            return View(country);
        }

        // GET: Countries/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Countries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CountryName")] Country country)
        {
            if (ModelState.IsValid)
            {
                _context.Add(country);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(country);
        }

        // GET: Countries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var country = await _context.Country.FindAsync(id);
            if (country == null)
            {
                return NotFound();
            }
            return View(country);
        }

        // POST: Countries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CountryName")] Country country)
        {
            if (id != country.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(country);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CountryExists(country.Id))
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
            return View(country);
        }

        // GET: Countries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var country = await _context.Country
                .FirstOrDefaultAsync(m => m.Id == id);
            if (country == null)
            {
                return NotFound();
            }

            return View(country);
        }

        // POST: Countries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            
            using (var context = new TableTennisDBContext())
            {
                var country1 = context.Country.Include(c => c.Factory).SingleOrDefault(c => c.Id == id);
                foreach(var child in country1.Factory.ToList())
                {
                    var factory1 = context.Factory.Include(f => f.Blade).SingleOrDefault(f => f.Id == child.Id);
                    foreach(var child1 in factory1.Blade.ToList())
                    {
                        var blade = context.Blade.Include(b => b.Racket).SingleOrDefault(b => b.Id == child1.Id);
                        foreach(var child2 in blade.Racket.ToList())
                        {
                            // TODO remove player rackets?
                            var racket = context.Racket.Include(w => w.PlayerRackets).SingleOrDefault(w => w.Id == child2.Id);
                            var child3 = racket.PlayerRackets;
                            context.PlayerRackets.Remove(child3);
                            context.Racket.Remove(child2);
                        }
                        context.Blade.Remove(child1);
                    }
                    var factory2 = context.Factory.Include(f => f.Rubber).SingleOrDefault(f => f.Id == child.Id);
                    foreach (var child1 in factory1.Rubber.ToList())
                    {
                        var rubber1 = context.Rubber.Include(r => r.RacketBhrubber).SingleOrDefault(r => r.Id == child1.Id);
                        foreach (var child2 in rubber1.RacketBhrubber.ToList())
                        {
                            var racket = context.Racket.Include(w => w.PlayerRackets).SingleOrDefault(w => w.Id == child2.Id);
                            // TODO remove player rackets?
                            var child3 = racket.PlayerRackets;
                            foreach (var child4 in child3.GameRacket1.ToList())
                            {
                                context.Game.Remove(child4);
                            }
                            foreach (var child4 in child3.GameRacket2.ToList())
                            {
                                context.Game.Remove(child4);
                            }
                            context.PlayerRackets.Remove(child3);
                            context.Racket.Remove(child2);
                        }
                        var rubber2 = context.Rubber.Include(r => r.RacketFhrubber).SingleOrDefault(r => r.Id == child1.Id);
                        foreach (var child2 in rubber2.RacketFhrubber.ToList())
                        {
                            var racket = context.Racket.Include(w => w.PlayerRackets).SingleOrDefault(w => w.Id == child2.Id);
                            var child3 = racket.PlayerRackets;
                            foreach (var child4 in child3.GameRacket1.ToList())
                            {
                                context.Game.Remove(child4);
                            }
                            foreach (var child4 in child3.GameRacket2.ToList())
                            {
                                context.Game.Remove(child4);
                            }
                            context.PlayerRackets.Remove(child3);
                            context.Racket.Remove(child2);
                        }
                        context.Rubber.Remove(child1);
                    }
                    context.Factory.Remove(child);
                }
                var country2 = context.Country.Include(c => c.Player).SingleOrDefault(c => c.Id == id);
                foreach (var child in country2.Player.ToList())
                {
                    var player2 = context.Player.Include(q => q.GamePlayer2).SingleOrDefault(q => q.Id == child.Id);
                    foreach(var child1 in player2.GamePlayer2.ToList())
                    {
                        context.Game.Remove(child1);
                    }
                    var player1 = context.Player.Include(q => q.GamePlayer1).SingleOrDefault(q => q.Id == child.Id);
                    foreach (var child1 in player1.GamePlayer1.ToList())
                    {
                        context.Game.Remove(child1);
                    }
                    var player3 = context.Player.Include(q => q.PlayerRackets).SingleOrDefault(q => q.Id == child.Id);
                    foreach(var child1 in player3.PlayerRackets.ToList())
                    {
                        context.PlayerRackets.Remove(child1);
                    }
                    context.Player.Remove(child);
                }
                context.SaveChanges();
            }

            // Remove country
            var country = await _context.Country.FindAsync(id);
            _context.Country.Remove(country);
            await _context.SaveChangesAsync();



            return RedirectToAction(nameof(Index));
        }

        private bool CountryExists(int id)
        {
            return _context.Country.Any(e => e.Id == id);
        }
    }
}
