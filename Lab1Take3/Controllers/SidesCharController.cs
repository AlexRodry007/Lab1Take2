using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Lab1Take3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SidesCharController : ControllerBase
    {
        private readonly Lab1v2Context _context;

        public SidesCharController (Lab1v2Context context)
        {
            _context = context;
        }
        [HttpGet("JsonData")]
        public JsonResult JsonData()
        {
            var sides = _context.Sides.Include(b => b.Characters).ToList();
            List<object> charSides = new List<object>();
            charSides.Add(new[] { "Сторона", "Кол-во персонажей" });
            foreach(var c in sides)
            {
                charSides.Add(new object[] { c.SideName, c.Characters.Count() });
            }
            return new JsonResult(charSides);
        }

    }
}
