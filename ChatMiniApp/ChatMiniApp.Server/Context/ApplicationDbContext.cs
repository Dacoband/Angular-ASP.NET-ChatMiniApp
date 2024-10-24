using ChatMiniApp.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatMiniApp.Server.Context
{
    public sealed class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Chat> Chats { get; set; }
    }
}
