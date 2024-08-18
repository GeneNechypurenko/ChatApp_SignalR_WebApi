using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Data
{
	public class ApplicationDbContext : DbContext
	{
		public DbSet<ChatMessage> ChatMessages { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<UserChatSession> UsersChatSessions { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) => Database.EnsureCreated();
    }
}
