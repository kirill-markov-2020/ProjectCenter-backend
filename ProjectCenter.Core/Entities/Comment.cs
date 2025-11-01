using System;
using System.Collections.Generic;

namespace ProjectCenter.Core.Entities;

public partial class Comment
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string Text { get; set; } = null!;

    public DateTime Date { get; set; }

    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();

    public virtual User User { get; set; } = null!;
}
