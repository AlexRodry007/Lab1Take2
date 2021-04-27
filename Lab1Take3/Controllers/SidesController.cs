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
    public class SidesController : Controller
    {
        private readonly Lab1v2Context _context;

        public SidesController(Lab1v2Context context)
        {
            _context = context;
        }

        // GET: Sides
        public async Task<IActionResult> Index()
        {
            return View(await _context.Sides.ToListAsync());
        }

        // GET: Sides/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var side = await _context.Sides
                .FirstOrDefaultAsync(m => m.Id == id);
            if (side == null)
            {
                return NotFound();
            }

            return View(side);
        }

        // GET: Sides/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Sides/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SideName")] Side side)
        {
            if (ModelState.IsValid)
            {
                _context.Add(side);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(side);
        }

        // GET: Sides/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var side = await _context.Sides.FindAsync(id);
            if (side == null)
            {
                return NotFound();
            }
            return View(side);
        }

        // POST: Sides/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SideName")] Side side)
        {
            if (id != side.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(side);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SideExists(side.Id))
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
            return View(side);
        }

        // GET: Sides/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var side = await _context.Sides
                .FirstOrDefaultAsync(m => m.Id == id);
            if (side == null)
            {
                return NotFound();
            }

            return View(side);
        }

        // POST: Sides/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var side = await _context.Sides.FindAsync(id);
            _context.Sides.Remove(side);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SideExists(int id)
        {
            return _context.Sides.Any(e => e.Id == id);
        }
    }
}
