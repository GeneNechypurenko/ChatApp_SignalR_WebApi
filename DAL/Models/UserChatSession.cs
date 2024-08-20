using System.Security.Cryptography.X509Certificates;

namespace DAL.Models
{
	public class UserChatSession
	{
		public int Id { get; set; }
		public string? LoginTime { get; set; }
		public string? LogoutTime { get; set; }
		public int UserId { get; set; }
		public User? User { get; set; }
		public ChatMessage? ChatMessages { get; set; }
	}
}
