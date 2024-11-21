using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExamPortal.Models;
using ExamPortal.Helpers;

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
            return await _context.TSubjects
                                   .Where(s => s.SStatus == true)
                                   .ToListAsync();
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<TSubject>>> GetSubjectById(int id)
        {
            try
            {
                TSubject subject = await _context.TSubjects
                                            .Where(s => s.SId == id && s.SStatus == true)
                                            .FirstOrDefaultAsync();
                if (subject == null)
                {
                    return NotFound($"Subject with ID {id} not found.");
                }

                return Ok(subject);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutSubject(int id, TSubject updatedSubject)
        {
            if (id != updatedSubject.SId)
            {
                return BadRequest(new { message = "Subject ID mismatch" });
            }

            TSubject subject = await _context.TSubjects
                                            .Where(s => s.SId == id && s.SStatus == true)
                                            .FirstOrDefaultAsync(); 

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

            TSubject tSubject = _context.TSubjects
                                    .FirstOrDefault(subj => subj.SId == id);

            if (tSubject == null)
            {
                return NotFound();
            }

            tSubject.SStatus = false;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Subject deleted successfully" });
        }

        [HttpPost]
        public async Task<IActionResult> CreateSubject([FromBody] SubjectDto subjectDto)
        {
            if (subjectDto == null)
            {
                return BadRequest("Subject data is required.");
            }

            TSubject subject = new TSubject
            {
                SCode = subjectDto.SCode,
                STitle = subjectDto.STitle,
                SClass = subjectDto.SClass,
                STName = subjectDto.StName,
                STSurname = subjectDto.StSurname,
                SStatus = subjectDto.SStatus
            };

            try
            {
                _context.TSubjects.Add(subject); 
                await _context.SaveChangesAsync(); 

                return CreatedAtAction(nameof(GetSubjectById), new { id = subject.SId }, subject); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }


        private bool SubjectExists(decimal id)
        {
            return (_context.TSubjects?.Any(e => e.SId == id)).GetValueOrDefault();
        }
    }
}
