namespace BLL.ModelsDTO
{
	public class ChatMessageDTO
	{
		public int Id { get; set; }
		public string? Message { get; set; }
		public string? Timestamp { get; set; }
		public int SessionId { get; set; }
	}
}
