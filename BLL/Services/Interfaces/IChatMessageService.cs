namespace BLL.Services.Interfaces
{
    public interface IChatMessageService
    {
        Task AddMessageAsync(string username, string message);
    }
}
