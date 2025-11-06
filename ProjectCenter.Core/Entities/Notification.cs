using System;
using System.Collections.Generic;

namespace ProjectCenter.Core.Entities;

public class Notification
{
    public int Id { get; set; }
    public int SenderId { get; set; }
    public int RecipientId { get; set; }
    public string Text { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


    public virtual User Sender { get; set; }
    public virtual User Recipient { get; set; }
}
