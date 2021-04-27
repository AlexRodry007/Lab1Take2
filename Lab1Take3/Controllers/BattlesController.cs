using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lab1Take3;
using Microsoft.AspNetCore.Http;
using System.IO;
using ClosedXML.Excel;

namespace Lab1Take3.Controllers
{
    public class BattlesController : Controller
    {
        private readonly Lab1v2Context _context;

        public BattlesController(Lab1v2Context context)
        {
            _context = context;
        }

        // GET: Battles
        public async Task<IActionResult> Index()
        {
            return View(await _context.Battles.ToListAsync());
        }

        // GET: Battles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var battle = await _context.Battles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (battle == null)
            {
                return NotFound();
            }

            //return View(battle);
            return RedirectToAction("Index", "Characters", new { id = battle.Id, name = battle.BattleName });
        }

        // GET: Battles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Battles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,BattleName,BattleTypeId")] Battle battle)
        {
            if (ModelState.IsValid)
            {
                _context.Add(battle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(battle);
        }

        // GET: Battles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var battle = await _context.Battles.FindAsync(id);
            if (battle == null)
            {
                return NotFound();
            }
            return View(battle);
        }

        // POST: Battles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BattleName,BattleTypeId")] Battle battle)
        {
            if (id != battle.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(battle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BattleExists(battle.Id))
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
            return View(battle);
        }

        // GET: Battles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var battle = await _context.Battles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (battle == null)
            {
                return NotFound();
            }

            return View(battle);
        }

        // POST: Battles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var battle = await _context.Battles.FindAsync(id);
            _context.Battles.Remove(battle);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BattleExists(int id)
        {
            return _context.Battles.Any(e => e.Id == id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Import(IFormFile fileExcel)
        {
            if (ModelState.IsValid)
            {
                if (fileExcel != null)
                {
                    using (var stream = new FileStream(fileExcel.FileName, FileMode.Create))
                    {
                        await fileExcel.CopyToAsync(stream);
                        using (XLWorkbook workBook = new XLWorkbook(stream, XLEventTracking.Disabled))
                        {
                            //перегляд усіх листів (в даному випадку категорій)
                            foreach (IXLWorksheet worksheet in workBook.Worksheets)
                            {
                                //worksheet.Name - назва категорії. Пробуємо знайти в БД, якщо відсутня, то створюємо нову
                                Battle newcat;
                                var c = (from battle in _context.Battles
                                         where battle.BattleName.Contains(worksheet.Name)
                                         select battle).ToList();
                                if (c.Count > 0)
                                {
                                    newcat = c[0];
                                }
                                else
                                {
                                    newcat = new Battle();
                                    newcat.BattleName = worksheet.Name;
                                    newcat.BattleTypeId = 1;
                                    newcat.BattleDescription = "from EXCEL";
                                    //додати в контекст
                                    _context.Battles.Add(newcat);
                                }
                                //перегляд усіх рядків                    
                                foreach (IXLRow row in worksheet.RowsUsed().Skip(1))
                                {
                                    try
                                    {
                                        Character character = new Character();
                                        character.CharacterName = row.Cell(1).Value.ToString();
                                        character.CharacterLevel = Convert.ToInt32(row.Cell(2).Value);
                                        character.RaceId = Convert.ToInt32(row.Cell(3).Value);
                                        character.Battle = newcat;
                                        _context.Characters.Add(character);
                                        //у разі наявності автора знайти його, у разі відсутності - додати
                                       /* for (int i = 3; i <= 3; i++)
                                        {
                                            if (row.Cell(i).Value.ToString().Length > 0)
                                            {
                                                Race race;

                                                var a = (from aut in _context.Races
                                                         where aut.RaceName.Contains(row.Cell(i).Value.ToString())
                                                         select aut).ToList();
                                                if (a.Count > 0)
                                                {
                                                    race = a[0];
                                                }
                                                else
                                                {
                                                    race = new Race();
                                                    race.RaceName = row.Cell(i).Value.ToString();
                                                    race.RaceHealthChange = 0;
                                                    race.RaceDamageChange = 0;
                                                    //додати в контекст
                                                    _context.Add(race);
                                                }
                                                /*AuthorsBooks ab = new AuthorsBooks();
                                                ab.Book = character;
                                                ab.Author = race;
                                                _context.AuthorsBooks.Add(ab);
                                            }
                                        }*/
                                    }
                                    catch (Exception e)
                                    {
                                        //logging самостійно :)
                                        Console.WriteLine("Error");
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
        public ActionResult Export()
        {
            using (XLWorkbook workbook = new XLWorkbook(XLEventTracking.Disabled))
            {
                var battles = _context.Battles.Where(a => a.BattleTypeId==3).Include("Characters").ToList();
                //тут, для прикладу ми пишемо усі книжки з БД, в своїх проектах ТАК НЕ РОБИТИ (писати лише вибрані)
                foreach (var c in battles)
                {
                    var worksheet = workbook.Worksheets.Add(c.BattleName);

                    worksheet.Cell("A1").Value = "Имя";
                    worksheet.Cell("B1").Value = "Уровень персонажа";
                    worksheet.Cell("C1").Value = "Расса";
                  /*  worksheet.Cell("D1").Value = "Автор 3";
                    worksheet.Cell("E1").Value = "Автор 4";
                    worksheet.Cell("F1").Value = "Категорія";
                    worksheet.Cell("G1").Value = "Інформація";*/
                    worksheet.Row(1).Style.Font.Bold = true;
                    var characters = c.Characters.ToList();

                    //нумерація рядків/стовпчиків починається з індекса 1 (не 0)
                    for (int i = 0; i < characters.Count; i++)
                    {
                        worksheet.Cell(i + 2, 1).Value = characters[i].CharacterName;
                        worksheet.Cell(i + 2, 2).Value = characters[i].CharacterLevel;
                        worksheet.Cell(i + 2, 3).Value = characters[i].RaceId;


                        /* var ab = _context.AuthorsBooks.Where(a => a.BookId == characters[i].Id).Include("Author").ToList();
                         //більше 4-ох нікуди писати
                         int j = 0;
                         foreach (var a in ab)
                         {
                             if (j < 5)
                             {
                                 worksheet.Cell(i + 2, j + 2).Value = a.Author.Name;
                                 j++;
                             }
                         }*/

                    }
                }
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Flush();

                    return new FileContentResult(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        FileDownloadName = $"library_{DateTime.UtcNow.ToShortDateString()}.xlsx"
                    };
                }
            }
        }


    }
}
