using System;
using System.Collections.Generic;

namespace ExamPortal.Models;

public partial class TSubject
{
    public decimal SId { get; set; }

    /// <summary>
    /// unique constraint added for distinction of the same subject for different classes
    /// </summary>
    public string SCode { get; set; } = null!;

    public string STitle { get; set; } = null!;

    public decimal SClass { get; set; }

    /// <summary>
    /// teacher name
    /// </summary>
    public string STName { get; set; } = null!;

    /// <summary>
    /// teacher surname
    /// </summary>
    public string STSurname { get; set; } = null!;

    /// <summary>
    /// is subject active (1) or deleted (0) 
    /// </summary>
    public bool? SStatus { get; set; }

    public virtual ICollection<TExam> TExams { get; set; } = new List<TExam>();
}
