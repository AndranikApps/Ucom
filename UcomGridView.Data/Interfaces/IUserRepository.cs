using UcomGridView.Data.Entities;

namespace UcomGridView.Data.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> CreateAsync(User user);
        Task<User?> GetByIdAsync(int id);
        Task<IEnumerable<User>> GetAsync(int skip, int take, string columnName, string order);
        Task<string> UpdateAsync(User user);
        Task<int> DeleteAsync(int id);
    }
}
