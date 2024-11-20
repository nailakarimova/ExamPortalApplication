using System;
using System.Collections.Generic;

namespace ExamPortal.Models;

public partial class TStudent
{
    public int SId { get; set; }

    public decimal SNumber { get; set; }

    public string SName { get; set; } = null!;

    public string SSurname { get; set; } = null!;

    public decimal SClass { get; set; }

    /// <summary>
    /// is student active (1) or deleted (0)
    /// </summary>
    public bool? SStatus { get; set; }

    public virtual ICollection<TExam> TExams { get; set; } = new List<TExam>();
}
