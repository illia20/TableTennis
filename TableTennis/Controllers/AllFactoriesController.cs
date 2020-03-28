using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.IO;
using ClosedXML.Excel;
using TableTennis;

namespace TableTennis.Controllers
{
    public class AllFactoriesController : Controller
    {
        private readonly TableTennisDBContext _context;

        public AllFactoriesController(TableTennisDBContext context)
        {
            _context = context;
        }

        // GET: AllFactories
        public async Task<IActionResult> Index()
        {
            var tableTennisDBContext = _context.Factory.Include(f => f.Country);
            return View(await tableTennisDBContext.ToListAsync());
        }

        // GET: AllFactories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var factory = await _context.Factory
                .Include(f => f.Country)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (factory == null)
            {
                return NotFound();
            }

            return View(factory);
        }

        // GET: AllFactories/Create
        public IActionResult Create()
        {
            ViewData["CountryId"] = new SelectList(_context.Country, "Id", "CountryName");
            return View();
        }

        // POST: AllFactories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FactoryName,CountryId")] Factory factory)
        {
            if (FactoryExists(factory.FactoryName))
            {
                return RedirectToAction(nameof(Index));
            }
            if (ModelState.IsValid)
            {
                _context.Add(factory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CountryId"] = new SelectList(_context.Country, "Id", "CountryName", factory.CountryId);
            return View(factory);
        }

        // GET: AllFactories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var factory = await _context.Factory.FindAsync(id);
            if (factory == null)
            {
                return NotFound();
            }
            ViewData["CountryId"] = new SelectList(_context.Country, "Id", "CountryName", factory.CountryId);
            return View(factory);
        }

        // POST: AllFactories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FactoryName,CountryId")] Factory factory)
        {
            if (id != factory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(factory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FactoryExists(factory.Id))
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
            ViewData["CountryId"] = new SelectList(_context.Country, "Id", "CountryName", factory.CountryId);
            return View(factory);
        }

        // GET: AllFactories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var factory = await _context.Factory
                .Include(f => f.Country)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (factory == null)
            {
                return NotFound();
            }

            return View(factory);
        }

        // POST: AllFactories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            using (var context = new TableTennisDBContext())
            {
                var factory1 = context.Factory.Include(f => f.Blade).SingleOrDefault(f => f.Id == id);
                foreach (var child1 in factory1.Blade.ToList())
                {
                    var blade = context.Blade.Include(b => b.Racket).SingleOrDefault(b => b.Id == child1.Id);
                    foreach (var child2 in blade.Racket.ToList())
                    {

                        var racket = context.Racket.Include(w => w.PlayerRackets).SingleOrDefault(w => w.Id == child2.Id);
                        var child3 = racket.PlayerRackets;
                        context.PlayerRackets.Remove(child3);
                        context.Racket.Remove(child2);
                    }
                    context.Blade.Remove(child1);
                }
                var factory2 = context.Factory.Include(f => f.Rubber).SingleOrDefault(f => f.Id == id);
                foreach (var child1 in factory1.Rubber.ToList())
                {
                    var rubber1 = context.Rubber.Include(r => r.RacketBhrubber).SingleOrDefault(r => r.Id == child1.Id);
                    foreach (var child2 in rubber1.RacketBhrubber.ToList())
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
                context.SaveChanges();
            }
            var factory = await _context.Factory.FindAsync(id);
            _context.Factory.Remove(factory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool FactoryExists(string _factoryname)
        {
            return _context.Factory.Any(e => e.FactoryName == _factoryname);
        }

        private bool FactoryExists(int id)
        {
            return _context.Factory.Any(e => e.Id == id);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ImportBlades(IFormFile fileExcel)
        {
            if (ModelState.IsValid)
            {
                if (fileExcel != null)
                {
                    using (var stream = new FileStream(fileExcel.FileName, FileMode.Create))
                    {
                        await fileExcel.CopyToAsync(stream);
                        using (XLWorkbook workbook = new XLWorkbook(stream, XLEventTracking.Disabled))
                        {
                            foreach (IXLWorksheet worksheet in workbook.Worksheets)
                            {
                                string factoryName;
                                string countryName;
                                int d = worksheet.Name.IndexOf(" - ");
                                countryName = worksheet.Name.Substring(d + 3);
                                int len = worksheet.Name.Length - countryName.Length - 3;
                                factoryName = worksheet.Name.Substring(0, len);
                                factoryName = factoryName.Trim();
                                countryName = countryName.Trim();
                                Factory newfactory;
                                var f = (from factory in _context.Factory
                                         where factory.FactoryName.Contains(worksheet.Name)
                                         select factory).ToList();
                                if (f.Count > 0)
                                {
                                    newfactory = f[0];
                                }
                                else
                                {
                                    newfactory = new Factory();
                                    newfactory.FactoryName = factoryName;
                                    Country c = new Country();
                                    c.CountryName = countryName;
                                    _context.Country.Add(c);
                                    newfactory.Country = c;
                                    _context.Factory.Add(newfactory);
                                }
                                foreach (IXLRow row in worksheet.RowsUsed().Skip(1))
                                {
                                    try
                                    {
                                        Blade blade = new Blade();
                                        blade.BladeName = row.Cell(1).Value.ToString();
                                        string cValue = row.Cell(2).Value.ToString();
                                        if (cValue == "1" || cValue == "+") blade.Composite = true;
                                        else blade.Composite = false;
                                        blade.Factory = newfactory;
                                        _context.Blade.Add(blade);
                                    }
                                    catch (Exception e)
                                    {
                                        //
                                    }
                                }
                            }
                        }
                    }
                }
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ImportRubbers(IFormFile fileExcel)
        {
            if (ModelState.IsValid)
            {
                if (fileExcel != null)
                {
                    using (var stream = new FileStream(fileExcel.FileName, FileMode.Create))
                    {
                        await fileExcel.CopyToAsync(stream);
                        using (XLWorkbook workbook = new XLWorkbook(stream, XLEventTracking.Disabled))
                        {
                            foreach (IXLWorksheet worksheet in workbook.Worksheets)
                            {
                                string factoryName;
                                string countryName;
                                int d = worksheet.Name.IndexOf(" - ");
                                countryName = worksheet.Name.Substring(d + 3);
                                int len = worksheet.Name.Length - countryName.Length - 3;
                                factoryName = worksheet.Name.Substring(0, len);
                                factoryName = factoryName.Trim();
                                countryName = countryName.Trim();
                                Factory newfactory;
                                var f = (from factory in _context.Factory
                                         where factory.FactoryName.Contains(worksheet.Name)
                                         select factory).ToList();
                                if (f.Count > 0)
                                {
                                    newfactory = f[0];
                                }
                                else
                                {
                                    newfactory = new Factory();
                                    Country c = new Country();
                                    c.CountryName = countryName;
                                    newfactory.FactoryName = factoryName;
                                    _context.Country.Add(c);
                                    newfactory.Country = c;
                                    _context.Factory.Add(newfactory);
                                }
                                foreach (IXLRow row in worksheet.RowsUsed().Skip(1))
                                {
                                    try
                                    {
                                        Rubber rubber = new Rubber();
                                        rubber.RubberName = row.Cell(1).Value.ToString();
                                        rubber.Pimples = row.Cell(2).Value.ToString();
                                        rubber.Factory = newfactory;
                                        _context.Rubber.Add(rubber);
                                    }
                                    catch (Exception e)
                                    {
                                        //
                                    }
                                }
                            }
                        }
                    }
                }
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        public ActionResult ExportBlades()
        {
            using (XLWorkbook workbook = new XLWorkbook(XLEventTracking.Disabled))
            {
                var factories = _context.Factory.Include("Blade").ToList();
                foreach (var f in factories)
                {
                    var worksheet = workbook.Worksheets.Add(f.FactoryName);

                    worksheet.Cell("A1").Value = "Blade";
                    worksheet.Cell("B1").Value = "Composite";
                    worksheet.Row(1).Style.Font.Bold = true;
                    var blades = f.Blade.ToList();
                    for (int i = 0; i < blades.Count; i++)
                    {
                        worksheet.Cell(i + 2, 1).Value = blades[i].BladeName;
                        worksheet.Cell(i + 2, 2).Value = blades[i].Composite;
                    }
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Flush();
                    return new FileContentResult(stream.ToArray(), "application/vndopenxmlformats-officedocument.spreadsheet.sheet")
                    {
                        FileDownloadName = $"FactoriesBlades.xlsx"
                    };
                }
            }
        }
        public ActionResult ExportRubbers()
        {
            using (XLWorkbook workbook = new XLWorkbook(XLEventTracking.Disabled))
            {
                var factories = _context.Factory.Include("Rubber").ToList();
                foreach (var f in factories)
                {
                    var worksheet = workbook.Worksheets.Add(f.FactoryName);

                    worksheet.Cell("A1").Value = "Rubber";
                    worksheet.Cell("B1").Value = "Pimples";
                    worksheet.Row(1).Style.Font.Bold = true;
                    var rubbers = f.Rubber.ToList();
                    for (int i = 0; i < rubbers.Count; i++)
                    {
                        worksheet.Cell(i + 2, 1).Value = rubbers[i].RubberName;
                        worksheet.Cell(i + 2, 2).Value = rubbers[i].Pimples;
                    }
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Flush();
                    return new FileContentResult(stream.ToArray(), "application/vndopenxmlformats-officedocument.spreadsheet.sheet")
                    {
                        FileDownloadName = $"FactoriesRubbers.xlsx"
                    };
                }
            }
        }
    }
}
