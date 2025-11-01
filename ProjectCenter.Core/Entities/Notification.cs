using System;
using System.Collections.Generic;

namespace ProjectCenter.Core.Entities;

public partial class Notification
{
    public int Id { get; set; }

    public int SenderId { get; set; }

    public int RecipientId { get; set; }

    public string Text { get; set; } = null!;

    public virtual User Recipient { get; set; } = null!;

    public virtual User Sender { get; set; } = null!;
}
