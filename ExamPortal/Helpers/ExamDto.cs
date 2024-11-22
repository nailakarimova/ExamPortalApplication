namespace ExamPortal.Helpers
{
    public class ExamDto
    {
        public int EId { get; set; }          // Exam ID
        public string ESCode { get; set; }    // Exam Subject Code
        public string SubjectTitle { get; set; } // Subject Title from T_Subject
        public decimal ESNumber { get; set; }     // Student Number
        public string StudentName { get; set; }  // Student Name from T_Student
        public string StudentSurname { get; set; } // Student Surname from T_Student
        public string EDate { get; set; }   // Exam Date
        public decimal EGrade { get; set; }   // Exam Grade
        public bool? EStatus { get; set; }     // Exam Status
    }
}
