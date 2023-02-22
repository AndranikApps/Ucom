using Microsoft.AspNetCore.Http;
using UcomGridView.Shared.Dtos;

namespace UcomGridView.Infrastructure.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> CreateAsync(CreateUserDto createUserDto);
        Task<UserDto?> GetByIdAsync(int id);
        Task<IEnumerable<UserDto>> GetAsync(GetUsersDto getUsersDto);
        Task<string> UpdateAsync(UpdateUserDto updateUserDto);
        Task DeleteAsync(int id);
    }
}
