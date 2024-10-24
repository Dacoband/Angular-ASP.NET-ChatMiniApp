using ChatMiniApp.Server.Context;
using ChatMiniApp.Server.DTOs;
using ChatMiniApp.Server.Models;
using GenericFileService.Files;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatMiniApp.Server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public sealed class AuthContronller(ApplicationDbContext context) : ControllerBase
    {
        [HttpPost("login")]
        public async Task<IActionResult> Register(RegisterDTO request, CancellationToken cancellationToken)
        {
            bool isNameExists = await context.Users.AnyAsync(x => x.Name == request.Name, cancellationToken);
            if (isNameExists)
            {
                return BadRequest(new { Message = "Đéo Vô Được Đâu" });
            }

            string avatar = FileService.FileSaveToServer(request.File, "wwwroot/avatar/");
            User user = new()
            {
                Name = request.Name,
                Avatar = avatar
            };
            await context.AddAsync(user, cancellationToken);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(string name, CancellationToken cancellationToken)
        {
            User? user = await context.Users.FirstOrDefaultAsync(x => x.Name == name, cancellationToken);
            if (user == null)
            {
                return BadRequest(new { Message = "Đéo Vô Được Đâu" });
            }
            user.Status = "Online";
            await context.SaveChangesAsync(cancellationToken);
            return Ok(user);
            
        }
    }
}
