using System;
using System.Collections.Generic;

namespace ProjectCenter.Core.Entities;

public partial class Project
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public int StudentId { get; set; }

    public int TeacherId { get; set; }

    public int StatusId { get; set; }

    public int TypeId { get; set; }

    public int SubjectId { get; set; }

    public string FileProject { get; set; } = null!;

    public string FileDocumentation { get; set; } = null!;

    public bool IsPublic { get; set; }

    public DateOnly DateDeadline { get; set; }

    public int? CommentId { get; set; }

    public DateOnly CreatedDate { get; set; }

    public virtual Comment? Comment { get; set; }

    public virtual StatusProject Status { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;

    public virtual Subject Subject { get; set; } = null!;

    public virtual Teacher Teacher { get; set; } = null!;

    public virtual TypeProject Type { get; set; } = null!;
}
