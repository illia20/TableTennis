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
    public class RubbersChartController : ControllerBase
    {
        private readonly TableTennisDBContext _context;

        public RubbersChartController(TableTennisDBContext context)
        {
            _context = context;
        }


        [HttpGet("JsonData")]
        public JsonResult JsonData()
        {
            var factories = _context.Factory.Include(f => f.Rubber).ToList();
            List<object> rubbersFactory = new List<object>();
            rubbersFactory.Add(new[] { "Factory", "Rubbers by the factory" });
            foreach (var f in factories)
            {
                rubbersFactory.Add(new object[] { f.FactoryName, f.Rubber.Count() });
            }
            return new JsonResult(rubbersFactory);
        }
    }
}