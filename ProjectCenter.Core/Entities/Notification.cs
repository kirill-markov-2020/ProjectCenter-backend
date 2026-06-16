using System;
using System.Collections.Generic;

namespace ProjectCenter.Core.Entities;

public class Notification
{
    public int Id { get; set; }
    public int RecipientId { get; set; }     
    public string Title { get; set; }              
    public string Text { get; set; }            
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public bool IsRead { get; set; } = false;   
    public int TypeId { get; set; }               


    public virtual User Recipient { get; set; }
    public virtual TypeNotification Type { get; set; }
}
