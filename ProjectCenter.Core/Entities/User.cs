using ProjectCenter.Core.ValueObjects;
using System;
using System.Collections.Generic;

namespace ProjectCenter.Core.Entities;

public class User
{
    public int Id { get; set; }
    public string Surname { get; set; }
    public string Name { get; set; }
    public string? Patronymic { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    public bool IsAdmin { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string? Photo { get; set; }


    public virtual Student Student { get; set; }
    public virtual Teacher Teacher { get; set; }
    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public virtual ICollection<Notification> SentNotifications { get; set; } = new List<Notification>();
    public virtual ICollection<Notification> ReceivedNotifications { get; set; } = new List<Notification>();
}
