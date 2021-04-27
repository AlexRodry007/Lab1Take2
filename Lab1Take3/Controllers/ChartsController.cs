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
    public class ChartsController : ControllerBase
    {
        private readonly Lab1v2Context _context;

        public ChartsController(Lab1v2Context context)
        {
            _context = context;
        }
        [HttpGet("JsonData")]
        public JsonResult JsonData()
        {
            var battles = _context.Battles.Include(b => b.Characters).ToList();
            List<object> batCharacter = new List<object>();

            batCharacter.Add(new[] { "Бой", "Кол-во участников" });
            foreach( var c in battles)
            {
                batCharacter.Add(new object[] { c.BattleName, c.Characters.Count });
            }
            return new JsonResult(batCharacter);
        }
    }
}
