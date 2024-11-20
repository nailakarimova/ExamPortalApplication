using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExamPortal.Models;

namespace ExamPortal.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SubjectController : ControllerBase
    {
        private readonly DbExamPortalContext _context;

        public SubjectController(DbExamPortalContext context)
        {
            _context = context;
        }

        // GET: /Subjects
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TSubject>>> GetSubjects()
        {           
            return await _context.TSubjects.Where(s => s.SStatus == true).ToListAsync();
        }

        // GET: /Subjects/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TSubject>> GetSubject(decimal id)
        {
            if (_context.TSubjects == null)
            {
                return NotFound();
            }
            var tSubject = await _context.TSubjects.FindAsync(id);

            if (tSubject == null)
            {
                return NotFound();
            }

            return tSubject;
        }

        // PUT: /Subjects/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSubject(decimal id, TSubject tSubject)
        {
            if (id != tSubject.SId)
            {
                return BadRequest();
            }

            _context.Entry(tSubject).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubjectExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: /Subjects
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TSubject>> PostSubject(TSubject tSubject)
        {
            if (_context.TSubjects == null)
            {
                return Problem("Entity set 'DbExamPortalContext.TSubjects'  is null.");
            }
            _context.TSubjects.Add(tSubject);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTSubject", new { id = tSubject.SId }, tSubject);
        }

        // DELETE: /Subjects/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubject(decimal id)
        {
            if (_context.TSubjects == null)
            {
                return NotFound();
            }
            var tSubject = await _context.TSubjects.FindAsync(id);
            if (tSubject == null)
            {
                return NotFound();
            }

            _context.TSubjects.Remove(tSubject);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SubjectExists(decimal id)
        {
            return (_context.TSubjects?.Any(e => e.SId == id)).GetValueOrDefault();
        }
    }
}
