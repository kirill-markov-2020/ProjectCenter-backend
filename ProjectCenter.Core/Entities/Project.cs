using System;
using System.Collections.Generic;

namespace ProjectCenter.Core.Entities;

public class Project
{
    public int Id { get; set; }
    public string Title { get; set; }
    public int StudentId { get; set; }
    public int TeacherId { get; set; }
    public int StatusId { get; set; }
    public int TypeId { get; set; }
    public int SubjectId { get; set; }
    public string? FileProject { get; set; }
    public string? FileDocumentation { get; set; }
    public bool IsPublic { get; set; }
    public DateTime DateDeadline { get; set; }
    public DateTime CreatedDate { get; set; }

    public virtual Student Student { get; set; }
    public virtual Teacher Teacher { get; set; }
    public virtual StatusProject Status { get; set; }
    public virtual TypeProject Type { get; set; }
    public virtual Subject Subject { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
}
