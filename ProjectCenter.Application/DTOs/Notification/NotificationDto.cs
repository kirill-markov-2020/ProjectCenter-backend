namespace ProjectCenter.Application.DTOs.Notification
{
    public class NotificationDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsRead { get; set; }
        public string TypeName { get; set; }  
        public int TypeId { get; set; }
    }
}
