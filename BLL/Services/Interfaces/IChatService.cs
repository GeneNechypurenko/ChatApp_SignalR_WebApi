namespace BLL.Services.Interfaces
{
	public interface IChatService <T> where T : class
	{
		Task<IEnumerable<T>> GetAllAsync();
		Task<T> GetAsync(int id);
		Task CreateAsync(T modelDTO);
		Task UpdateAsync(T modelDTO);
		Task DeleteAsync(int id);
	}
}
