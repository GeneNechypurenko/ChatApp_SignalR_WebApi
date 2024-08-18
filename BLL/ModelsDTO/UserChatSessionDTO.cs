namespace BLL.ModelsDTO
{
	public class UserChatSessionDTO
	{
		public int Id { get; set; }
		public string? LoginTime { get; set; }
		public string? LogoutTime { get; set; }
		public int UserId { get; set; }
	}
}
