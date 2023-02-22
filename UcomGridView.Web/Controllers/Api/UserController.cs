using Microsoft.AspNetCore.Mvc;
using UcomGridView.Infrastructure.Interfaces;
using UcomGridView.Shared.Dtos;

namespace UcomGridView.Web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("get/{id}")]
        public async Task<UserDto?> Get(int id)
        {
            return await _userService.GetByIdAsync(id);
        }

        [HttpPost("get-users")]
        public async Task<IEnumerable<UserDto>> GetAsync([FromBody] GetUsersDto getUsersDto)
        {
            return await _userService.GetAsync(getUsersDto);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Post([FromBody] CreateUserDto createUserDto)
        {
            var validation = createUserDto.IsValid();
            if (validation != null) return BadRequest(validation);

            return Ok(await _userService.CreateAsync(createUserDto));
        }

        [HttpPut("update")]
        public async Task<IActionResult> Put([FromForm] UpdateUserDto updateUserDto)
        {
            var validation = updateUserDto.IsValid();
            if (validation != null) return BadRequest(validation);

            return Ok(await _userService.UpdateAsync(updateUserDto));
        }

        [HttpDelete("delete/{id}")]
        public async Task Delete(int id)
        {
            await _userService.DeleteAsync(id);
        }
    }
}
