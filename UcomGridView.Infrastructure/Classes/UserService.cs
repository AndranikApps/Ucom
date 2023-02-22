using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UcomGridView.Data.Entities;
using UcomGridView.Data.Interfaces;
using UcomGridView.Infrastructure.Common;
using UcomGridView.Infrastructure.Interfaces;
using UcomGridView.Shared;
using UcomGridView.Shared.Dtos;
using UcomGridView.Shared.Enums;

namespace UcomGridView.Infrastructure.Classes
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<UserDto> CreateAsync(CreateUserDto createUserDto)
        {
            var filename = CreateFile(createUserDto.Avatar);

            var user = _mapper.Map<CreateUserDto, User>(createUserDto);
            user.AvatarPath = filename;
            user = await _userRepository.CreateAsync(user);

            if (user == null)
            {
                throw new ArgumentException(Messages.CreateUserError);
            }

            return _mapper.Map<User, UserDto>(user);
        }

        public async Task DeleteAsync(int id)
        {
            if (await _userRepository.DeleteAsync(id) <= 0)
                throw new ArgumentException(Messages.UserIsNotFound);
        }

        public async Task<IEnumerable<UserDto>> GetAsync(GetUsersDto getUsersDto)
        {
            IsValid(getUsersDto.Filter.Sorting);

            var userDtos = _mapper.Map<IEnumerable<User>, IEnumerable<UserDto>>(
                await _userRepository.GetAsync((getUsersDto.Page - 1) * getUsersDto.Take,
                    getUsersDto.Take,
                    getUsersDto.Filter.Sorting.ColumnName,
                    getUsersDto.Filter.Sorting.OrderBy)
                );

            foreach (var userDto in userDtos)
            {
                if (!userDto.Avatar.IsNullOrEmpty())
                    userDto.Avatar = ToBase64StringImage(userDto.Avatar);
            }

            return userDtos;
        }

        public async Task<UserDto?> GetByIdAsync(int id)
        {
            return _mapper.Map<User, UserDto>(
                await _userRepository.GetByIdAsync(id)
                );
        }

        public async Task<string> UpdateAsync(UpdateUserDto updateUserDto)
        {
            var user = _mapper.Map<UpdateUserDto, User>(updateUserDto);
            user.AvatarPath = CreateFile(updateUserDto.Avatar);
            string? oldAvatar = null;

            try
            {
                oldAvatar = await _userRepository.UpdateAsync(user);
            }
            catch (Exception ex)
            {
                DeleteFile(user.AvatarPath);
                throw;
            }

            if (!oldAvatar.IsNullOrEmpty())
            {
                DeleteFile(oldAvatar);
            }

            return ToBase64StringImage(user.AvatarPath);
        }

        private string CreateFile(IFormFile file)
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), Constants.AppRoot);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            FileInfo fileInfo = new FileInfo(file.FileName);
            string filename = Guid.NewGuid().ToString() + fileInfo.Extension;
            string fullname = Path.Combine(path, filename);

            using (var fileStream = new FileStream(fullname, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }

            return filename;
        }

        private void DeleteFile(string filename)
        {
            File.Delete(Path.Combine(Directory.GetCurrentDirectory(), Constants.AppRoot, filename));
        }

        private string ToBase64StringImage(string filename)
        {
            var fullname = Path.Combine(Directory.GetCurrentDirectory(), Constants.AppRoot, filename);

            if (!File.Exists(fullname)) return string.Empty;

            var bytes = File.ReadAllBytes(fullname);
            var fileInfo = new FileInfo(fullname);

            return string.Concat("data:image/",
                fileInfo.Extension.Substring(1),
                ";base64,",
                Convert.ToBase64String(bytes)
                );
        }

        private void IsValid(OrderFilter orderFilters)
        {
            if (orderFilters.ColumnName.IsNullOrEmpty() && orderFilters.OrderBy.IsNullOrEmpty())
                return;

            switch ((Filter)Enum.Parse(typeof(Filter), orderFilters.ColumnName))
            {
                case Filter.Firstname: break;
                case Filter.Lastname: break;
                case Filter.Age: break;
                case Filter.Email: break;
                case Filter.CreatedAt: break;
                case Filter.UpdatedAt: break;
                case Filter.IsDeleted: break;
                case Filter.Status: break;
                default: throw new ArgumentException(Messages.FilterError);
            }

            switch ((OrderBy)Enum.Parse(typeof(OrderBy), orderFilters.OrderBy))
            {
                case OrderBy.ASC: break;
                case OrderBy.DESC: break;
                default: throw new ArgumentException(Messages.FilterError);
            }
        }
    }
}
