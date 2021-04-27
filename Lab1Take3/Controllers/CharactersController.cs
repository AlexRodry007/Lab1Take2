using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lab1Take3;

namespace Lab1Take3.Controllers
{
    public class CharactersController : Controller
    {
        private readonly Lab1v2Context _context;

        public CharactersController(Lab1v2Context context)
        {
            _context = context;
        }

        // GET: Characters
        public async Task<IActionResult> Index(int? id, string? name)
        {
            //var lab1v2Context = _context.Characters.Include(c => c.Armor).Include(c => c.Battle).Include(c => c.Race).Include(c => c.Weapon);
            if (id == null) return RedirectToAction("Index", "Battles");
            ViewBag.BattleId = id;
            ViewBag.BattleName = name;
            var characterByBattle = _context.Characters.Where(b => b.BattleId == id).Include(b => b.Battle);
            return View(await characterByBattle.ToListAsync());
        }

        // GET: Characters/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var character = await _context.Characters
                .Include(c => c.Armor)
                .Include(c => c.Battle)
                .Include(c => c.Race)
                .Include(c => c.Weapon)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (character == null)
            {
                return NotFound();
            }

            return View(character);
        }

        // GET: Characters/Create
        public IActionResult Create(int battleId)
        {
            ViewData["ArmorId"] = new SelectList(_context.Armors, "Id", "Id");
            ViewData["RaceId"] = new SelectList(_context.Races, "Id", "Id");
            ViewData["WeaponId"] = new SelectList(_context.Weapons, "Id", "Id");
            ViewBag.BattleId = battleId;
            ViewBag.BattleName = _context.Battles.Where(c => c.Id == battleId).FirstOrDefault().BattleName;
            return View();
        }

        // POST: Characters/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int battleId,[Bind("Id,CharacterName,CharacterLevel,CharacterDamage,CharacterHealth,RaceId,WeaponId,ArmorId,BattleId,SideId")] Character character)
        {
            character.BattleId = battleId;
            if (ModelState.IsValid)
            {
                _context.Add(character);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                return RedirectToAction("Index", "Characters", new { id = battleId, name = _context.Battles.Where(c => c.Id == battleId).FirstOrDefault().BattleName });
            }
            ViewData["ArmorId"] = new SelectList(_context.Armors, "Id", "Id", character.ArmorId);
            //ViewData["BattleId"] = new SelectList(_context.Battles, "Id", "Id", character.BattleId);
            ViewData["RaceId"] = new SelectList(_context.Races, "Id", "Id", character.RaceId);
            ViewData["WeaponId"] = new SelectList(_context.Weapons, "Id", "Id", character.WeaponId);
            //return View(character);
            return RedirectToAction("Index", "Characters", new { id = battleId, name = _context.Battles.Where(c => c.Id == battleId).FirstOrDefault().BattleName });

        }

        // GET: Characters/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var character = await _context.Characters.FindAsync(id);
            if (character == null)
            {
                return NotFound();
            }
            ViewData["ArmorId"] = new SelectList(_context.Armors, "Id", "Id", character.ArmorId);
            ViewData["BattleId"] = new SelectList(_context.Battles, "Id", "Id", character.BattleId);
            ViewData["RaceId"] = new SelectList(_context.Races, "Id", "Id", character.RaceId);
            ViewData["WeaponId"] = new SelectList(_context.Weapons, "Id", "Id", character.WeaponId);
            return View(character);
        }

        // POST: Characters/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CharacterName,CharacterLevel,CharacterDamage,CharacterHealth,RaceId,WeaponId,ArmorId,BattleId,SideId")] Character character)
        {
            if (id != character.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(character);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CharacterExists(character.Id))
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
            ViewData["ArmorId"] = new SelectList(_context.Armors, "Id", "Id", character.ArmorId);
            ViewData["BattleId"] = new SelectList(_context.Battles, "Id", "Id", character.BattleId);
            ViewData["RaceId"] = new SelectList(_context.Races, "Id", "Id", character.RaceId);
            ViewData["WeaponId"] = new SelectList(_context.Weapons, "Id", "Id", character.WeaponId);
            return View(character);
        }

        // GET: Characters/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var character = await _context.Characters
                .Include(c => c.Armor)
                .Include(c => c.Battle)
                .Include(c => c.Race)
                .Include(c => c.Weapon)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (character == null)
            {
                return NotFound();
            }

            return View(character);
        }

        // POST: Characters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var character = await _context.Characters.FindAsync(id);
            _context.Characters.Remove(character);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CharacterExists(int id)
        {
            return _context.Characters.Any(e => e.Id == id);
        }
    }
}
