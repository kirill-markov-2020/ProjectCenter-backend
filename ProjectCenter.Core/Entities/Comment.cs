using System;

namespace ProjectCenter.Core.Entities;

public class Comment
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int ProjectId { get; set; }
    public string Text { get; set; }
    public DateTime Date { get; set; }

    public virtual Project Project { get; set; }
    public virtual User User { get; set; }
}
