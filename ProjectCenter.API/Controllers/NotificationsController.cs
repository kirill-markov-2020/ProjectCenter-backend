using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectCenter.Application.Interfaces;

namespace ProjectCenter.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationsController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

      
        [HttpGet]
        public async Task<IActionResult> GetMyNotifications()
        {
            if (!HttpContext.Items.ContainsKey("UserId"))
                return Unauthorized();

            int userId = (int)HttpContext.Items["UserId"];
            var notifications = await _notificationService.GetUserNotificationsAsync(userId);
            return Ok(notifications);
        }

        [HttpGet("unread")]
        public async Task<IActionResult> GetUnreadNotifications()
        {
            if (!HttpContext.Items.ContainsKey("UserId"))
                return Unauthorized();

            int userId = (int)HttpContext.Items["UserId"];
            var notifications = await _notificationService.GetUnreadNotificationsAsync(userId);
            return Ok(notifications);
        }

        [HttpGet("unread/count")]
        public async Task<IActionResult> GetUnreadCount()
        {
            if (!HttpContext.Items.ContainsKey("UserId"))
                return Unauthorized();

            int userId = (int)HttpContext.Items["UserId"];
            var count = await _notificationService.GetUnreadCountAsync(userId);
            return Ok(count);
        }

        [HttpPut("{id}/read")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            if (!HttpContext.Items.ContainsKey("UserId"))
                return Unauthorized();

            int userId = (int)HttpContext.Items["UserId"];
            await _notificationService.MarkAsReadAsync(userId, id);
            return NoContent();
        }

       
        [HttpPut("read-all")]
        public async Task<IActionResult> MarkAllAsRead()
        {
            if (!HttpContext.Items.ContainsKey("UserId"))
                return Unauthorized();

            int userId = (int)HttpContext.Items["UserId"];
            await _notificationService.MarkAllAsReadAsync(userId);
            return NoContent();
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotification(int id)
        {
            if (!HttpContext.Items.ContainsKey("UserId"))
                return Unauthorized();

            int userId = (int)HttpContext.Items["UserId"];
            await _notificationService.DeleteNotificationAsync(userId, id);
            return NoContent();
        }
    }
}