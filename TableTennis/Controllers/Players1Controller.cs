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
    public class Players1Controller : Controller
    {
        private readonly TableTennisDBContext _context;

        public Players1Controller(TableTennisDBContext context)
        {
            _context = context;
        }

        // GET: Players1
        public async Task<IActionResult> Index(int? id, string? name)
        {
            if (id == null) return RedirectToAction("Countries", "Index");
            ViewBag.CountryId = id;
            ViewBag.CountryName = name;

            var playersFromCountry = _context.Player.Where(p => p.CountryId == id).Include(p => p.Country);
            return View(await playersFromCountry.ToListAsync());
        }

        // GET: Players1/Details/5
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

            return View(player);
        }

        // GET: Players1/Create
        public IActionResult Create(int countryId)
        {
            //ViewData["CountryId"] = new SelectList(_context.Country, "Id", "CountryName");
            ViewBag.CountryId = countryId;
            // ViewBag.CountryName = _context.Country.Where(c => c.Id == countryId).FirstOrDefault().CountryName;
            return View();
        }

        // POST: Players1/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int countryId, [Bind("Id,Name,CountryId,Arm")] Player player)
        {
            player.CountryId = countryId;
            if (ModelState.IsValid)
            {
                _context.Add(player);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Players", new { id = countryId, name = _context.Country.Where(c => c.Id == countryId).FirstOrDefault().CountryName });
            }
            // ViewData["CountryId"] = new SelectList(_context.Country, "Id", "CountryName", player.CountryId);
            // return View(player);

            return RedirectToAction("Index", "Players", new { id = countryId, name = _context.Country.Where(c => c.Id == countryId).FirstOrDefault().CountryName });
        }

        // GET: Players1/Edit/5
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

        // POST: Players1/Edit/5
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

        // GET: Players1/Delete/5
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

        // POST: Players1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
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
