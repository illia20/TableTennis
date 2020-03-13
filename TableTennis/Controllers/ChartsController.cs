using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace TableTennis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartsController : ControllerBase
    {
        private readonly TableTennisDBContext _context;

        public ChartsController(TableTennisDBContext context)
        {
            _context = context;
        }


        [HttpGet("JsonData")]
        public JsonResult JsonData()
        {
            var countries = _context.Country.Include(c => c.Player).ToList();
            List<object> countryPlayer = new List<object>();
            countryPlayer.Add(new[] { "Country", "Amount of players" });
            foreach (var c in countries)
            {
                countryPlayer.Add(new object[] { c.CountryName, c.Player.Count()});
            }
            return new JsonResult(countryPlayer);
        }
    }
}