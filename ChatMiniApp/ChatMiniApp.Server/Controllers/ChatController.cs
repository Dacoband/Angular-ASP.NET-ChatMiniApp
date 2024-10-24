using ChatMiniApp.Server.Context;
using ChatMiniApp.Server.DTOs;
using ChatMiniApp.Server.Hubs;
using ChatMiniApp.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace ChatMiniApp.Server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public sealed class ChatController(ApplicationDbContext context, IHubContext<ChatHub> hubContext) : ControllerBase
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
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            List<User> users = await context.Users.OrderBy(x => x.Name).ToListAsync();
            return Ok(users);
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

            string connectionId = ChatHub.Users.First(v => v.Value == chat.ToUserId).Key;
            await hubContext.Clients.Client(connectionId).SendAsync("ReceiveMessage", chat);
            return Ok();
        }
    }
}
