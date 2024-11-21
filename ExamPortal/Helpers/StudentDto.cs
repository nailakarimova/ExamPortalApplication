namespace ExamPortal.Helpers
{
    public class StudentDto
    {
        public decimal SNumber { get; set; }
        public string SName { get; set; } = null!;
        public string SSurname { get; set; } = null!;
        public decimal SClass { get; set; }
        public bool? SStatus { get; set; }
    }
}
