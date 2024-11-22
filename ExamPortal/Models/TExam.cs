using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ExamPortal.Models;

public partial class TExam
{
    public int EId { get; set; }

    /// <summary>
    /// subject code from t_subject
    /// </summary>
    public string ESCode { get; set; } = null!;

    /// <summary>
    /// student number from t_student
    /// </summary>
    public decimal ESNumber { get; set; }

    public DateTime EDate { get; set; }

    public decimal EGrade { get; set; }

    /// <summary>
    /// is exam active (1) or deleted (0)
    /// </summary>
    public bool? EStatus { get; set; }

    [JsonIgnore]
    public virtual TSubject? ESCodeNavigation { get; set; } = null!;

    [JsonIgnore]
    public virtual TStudent? ESNumberNavigation { get; set; } = null!;
}
