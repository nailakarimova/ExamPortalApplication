using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExamPortal.Models;
using ExamPortal.Helpers;
using Newtonsoft.Json;

namespace ExamPortal.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExamController : ControllerBase
    {
        private readonly DbExamPortalContext _context;

        public ExamController(DbExamPortalContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExamDto>>> GetExams()
        {
            if (_context.TExams == null)
            {
                return NotFound();
            }

            return await _context.TExams
                                    .Where(e => e.EStatus == true) // Filter exams with E_STATUS = 1
                                    .Join(_context.TSubjects.Where(sb => sb.SStatus == true),
                                            e => e.ESCode,
                                            sb => sb.SCode,
                                            (e, sb) => new { Exam = e, Subject = sb }) // Join with subjects where S_STATUS = 1
                                    .Join(_context.TStudents.Where(st => st.SStatus == true),
                                            es => es.Exam.ESNumber,
                                            st => st.SNumber,
                                            (es, st) => new ExamDto
                                            {
                                                EId = es.Exam.EId,
                                                ESCode = es.Exam.ESCode,
                                                SubjectTitle = es.Subject.STitle,
                                                ESNumber = es.Exam.ESNumber,
                                                StudentName = st.SName,
                                                StudentSurname = st.SSurname,
                                                EDate = es.Exam.EDate.ToString("yyyy-MM-dd"),
                                                EGrade = es.Exam.EGrade,
                                                EStatus = es.Exam.EStatus
                                            }) // Join with students where S_STATUS = 1
                                    .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TExam>> GetExamById(int id)
        {
            try
            {
                TExam student = await _context.TExams
                                            .Where(e => e.EId == id && e.EStatus == true)
                                            .FirstOrDefaultAsync();
                if (student == null)
                {
                    return NotFound($"Exam with ID {id} not found.");
                }

                return Ok(student);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutExam(int id, TExam updatedExam)
        {
            if (id != updatedExam.EId)
            {
                return BadRequest(new { message = "Exam ID mismatch" });
            }

            TExam exam = await _context.TExams
                                            .Where(e => e.EId == id && e.EStatus == true)
                                            .FirstOrDefaultAsync();

            if (exam == null)
            {
                return NotFound(new { message = "exam not found" });
            }

            exam.ESCode = updatedExam.ESCode;
            exam.ESNumber = updatedExam.ESNumber;
            exam.EDate = updatedExam.EDate;
            exam.EGrade = updatedExam.EGrade;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Exam updated successfully" });
        }

        [HttpPost]
        public async Task<ActionResult<TExam>> PostExam([FromBody] ExamDto examDto)
        {
            if (examDto == null)
            {
                return BadRequest("Exam data is required.");
            }

            TExam exam = new TExam
            {
                ESCode = examDto.ESCode,
                ESNumber = examDto.ESNumber,
                EDate = Convert.ToDateTime(examDto.EDate).Date,
                EGrade = examDto.EGrade,
            };

            try
            {
                _context.TExams.Add(exam);
                await _context.SaveChangesAsync();

                var createdExamDto = await _context.TExams
                                                    .Where(e => e.EId == exam.EId)
                                                    .Select(e => new ExamDto
                                                    {
                                                        EId = e.EId,
                                                        ESCode = e.ESCode,
                                                        SubjectTitle = e.ESCodeNavigation.STitle,
                                                        ESNumber = e.ESNumber,
                                                        StudentName = e.ESNumberNavigation.SName,
                                                        StudentSurname = e.ESNumberNavigation.SSurname,
                                                        EDate = e.EDate.ToString("yyyy-MM-dd"),
                                                        EGrade = e.EGrade,
                                                        EStatus = e.EStatus ?? false
                                                    })
                                                    .FirstOrDefaultAsync();

                return CreatedAtAction(nameof(GetExamById), new { id = exam.EId }, createdExamDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExam(int id)
        {
            if (_context.TExams == null)
            {
                return NotFound();
            }

            TExam tExam = _context.TExams
                                  .FirstOrDefault(e => e.EId == id);

            if (tExam == null)
            {
                return NotFound();
            }

            tExam.EStatus = false;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Student deleted successfully" });
        }

        private bool TExamExists(int id)
        {
            return (_context.TExams?.Any(e => e.EId == id)).GetValueOrDefault();
        }
    }
}
