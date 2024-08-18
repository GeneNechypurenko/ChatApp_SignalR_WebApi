namespace DAL.Repositories.Interfaces
{
	public interface IRepository<T> where T : class
	{
		Task<IEnumerable<T>> GetAllAsync();
		Task<T> GetAsync(int id);
		Task CreateAsync(T entity);
		void Update(T entity);
		Task DeleteAsync(int id);
	}
}
