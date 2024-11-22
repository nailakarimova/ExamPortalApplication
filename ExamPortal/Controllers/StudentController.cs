using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExamPortal.Models;
using ExamPortal.Helpers;

namespace ExamPortal.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly DbExamPortalContext _context;

        public StudentController(DbExamPortalContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TStudent>>> GetStudents()
        {
          if (_context.TStudents == null)
          {
              return NotFound();
          }
            return await _context.TStudents
                                   .Where(s => s.SStatus == true)
                                   .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TStudent>> GetStudentById(int id)
        {
            try
            {
                TStudent student = await _context.TStudents
                                            .Where(s => s.SId == id && s.SStatus == true)
                                            .FirstOrDefaultAsync();
                if (student == null)
                {
                    return NotFound($"Student with ID {id} not found.");
                }

                return Ok(student);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent(int id, TStudent updatesStudent)
        {
            if (id != updatesStudent.SId)
            {
                return BadRequest(new { message = "Student ID mismatch" });
            }

            TStudent student = await _context.TStudents
                                            .Where(s => s.SId == id && s.SStatus == true)
                                            .FirstOrDefaultAsync();

            if (student == null)
            {
                return NotFound(new { message = "student not found" });
            }

            student.SNumber = updatesStudent.SNumber;
            student.SName = updatesStudent.SName;
            student.SSurname = updatesStudent.SSurname;
            student.SClass = updatesStudent.SClass;
            student.SStatus = updatesStudent.SStatus;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Student updated successfully" });
        }

        [HttpPost]
        public async Task<ActionResult<TStudent>> CreateStudent([FromBody] StudentDto studentDto)
        {
            if (studentDto == null)
            {
                return BadRequest("Student data is required.");
            }

            TStudent student = new TStudent
            {
                SNumber = studentDto.SNumber,
                SName = studentDto.SName,
                SSurname = studentDto.SSurname,
                SClass = studentDto.SClass,
                SStatus = studentDto.SStatus
            };

            try
            {
                _context.TStudents.Add(student);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetStudentById), new { id = student.SId }, student);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            if (_context.TStudents == null)
            {
                return NotFound();
            }

            TStudent tStudent = _context.TStudents
                                     .FirstOrDefault(st => st.SId == id);
            
            if (tStudent == null)
            {
                return NotFound();
            }

            tStudent.SStatus = false;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Student deleted successfully" });
        }

        private bool TStudentExists(int id)
        {
            return (_context.TStudents?.Any(e => e.SId == id)).GetValueOrDefault();
        }
    }
}
