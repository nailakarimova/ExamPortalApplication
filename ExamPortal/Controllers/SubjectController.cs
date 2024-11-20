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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TSubject>>> GetSubjects()
        {
            return await _context.TSubjects.Where(s => s.SStatus == true).ToListAsync();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutSubject(decimal id, TSubject updatedSubject)
        {
            if (id != updatedSubject.SId)
            {
                return BadRequest(new { message = "Subject ID mismatch" });
            }

            var subject = await _context.TSubjects.FindAsync(id);
            if (subject == null)
            {
                return NotFound(new { message = "Subject not found" });
            }

            subject.SCode = updatedSubject.SCode;
            subject.STitle = updatedSubject.STitle;
            subject.SClass = updatedSubject.SClass;
            subject.STName = updatedSubject.STName;
            subject.STSurname = updatedSubject.STSurname;
            subject.SStatus = updatedSubject.SStatus;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Subject updated successfully" });
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubject(int id)
        {
            if (_context.TSubjects == null)
            {
                return NotFound();
            }
            var tSubject = _context.TSubjects.FirstOrDefault(subj => subj.SId == id);
            if (tSubject == null)
            {
                return NotFound();
            }

            tSubject.SStatus = false;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Subject deleted successfully" });
        }

        private bool SubjectExists(decimal id)
        {
            return (_context.TSubjects?.Any(e => e.SId == id)).GetValueOrDefault();
        }
    }
}
