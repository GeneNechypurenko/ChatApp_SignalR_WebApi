using DAL.Models;

namespace BLL.ModelsDTO
{
	public class UserDTO
	{
		public int Id {  get; set; }
		public string? UserName { get; set; }
		public string? Password { get; set; }
		public string? PasswordHash { get; set; }
		public IEnumerable<ChatMessage>? Messages { get; set; }
	}
}
