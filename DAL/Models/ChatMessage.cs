namespace DAL.Models
{
	public class ChatMessage
	{
		public int Id { get; set; }
		public string? Message { get; set; }
		public string? Timestamp { get; set; }
		public int SessionId { get; set; }
		public UserChatSession? Session { get; set; }
	}
}
