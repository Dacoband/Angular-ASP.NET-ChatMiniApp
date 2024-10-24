using ChatMiniApp.Server.Context;
using ChatMiniApp.Server.DTOs;
using ChatMiniApp.Server.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatMiniApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class ChatController(ApplicationDbContext context) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetChats(Guid userId, Guid toUserId, CancellationToken cancellationToken)
        {
            List<Chat> chats = await context.Chats
                .Where(x => x.UserId == userId && x.ToUserId == toUserId || x.ToUserId == userId && x.UserId == toUserId)
                .OrderBy(x => x.Date)
                .ToListAsync(cancellationToken);
            return Ok(chats);
        }
        [HttpPost]
        public async Task<IActionResult> SendMessage(SendMessageDTO request, CancellationToken cancellationToken)
        {
            Chat chat = new()
            {
                UserId = request.UserId,
                ToUserId = request.ToUserId,
                Message = request.Message,
                Date = DateTime.Now
            };
            await context.AddAsync(chat, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            return Ok();
        }
    }
}
