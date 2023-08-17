using Microsoft.AspNetCore.Mvc;
using ParkingManagement.Controllers.OutputObject;
using ParkingManagement.Domain.Dtos;
using ParkingManagement.Domain.Services.v1;

namespace ParkingManagement.Controllers
{
    [Route("v1/api/[Controller]")]
    [ApiController]
    public class UsersController : BaseController
    {
        private readonly UsersService _service;
        public UsersController(UsersService service) { _service = service;  }

        /// <summary>
        /// This function returns list of all Users available in database
        /// </summary>
        /// <param name="query">Specify Query parameter</param>
        /// <returns>BaseResponse with list of UserDto</returns>
        /// <response code="200">Successfully get all Users</response>
        /// <response code="204">Content Not Available</response>
        /// <response code="500">Internal server error</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<UserDto>>> GetAllUserAsync()
        {
            BaseResponse<List<UserDto>> response = await _service.GetAllUserAsync().ConfigureAwait(false);
            return ReplyBaseResponse(response);
        }

        /// <summary>
        /// This function returns User by its Id
        /// </summary>
        /// <param name="id">Specify Id of User</param>
        /// <returns>BaseResponse with UserDto</returns>
        /// <response code="200">Successfully get the User with Id</response>
        /// <response code="404">User with the given Id not found</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserDto>> GetUserByIdAsync([FromRoute] int id)
        {
            BaseResponse<UserDto> response = await _service.GetUserByIdAsync(id).ConfigureAwait(false);
            return ReplyBaseResponse(response);
        }

        /// <summary>
        /// This function add new User to database
        /// </summary>
        /// <param name="User">Specify User object</param>
        /// <returns>BaseResponse with User</returns>
        /// <response code="201">Successfully created the User</response>
        /// <response code="400">BadRequest</response>
        /// <response code="409">User already exists</response>
        /// <response code="500">Internal server error</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserDto>> AddUserAsync([FromBody] UserDto User)
        {
            BaseResponse<UserDto> response = await _service.AddUserAsync(User).ConfigureAwait(false);
            return ReplyBaseResponse(response);
        }

        /// <summary>
        /// This function update existing User
        /// </summary>
        /// <param name="id">Specify Id of User</param>
        /// <param name="User">Specify User object</param>
        /// <returns>BaseResponse with UserDto</returns>
        /// <response code="200">successfully updated the User</response>
        /// <response code="400">badRequest</response>
        /// <response code="404">User not found</response>
        /// <response code="500">internal server error</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserDto>> UpdateUserAsync([FromRoute] int id, [FromBody] UserDto User)
        {
            BaseResponse<UserDto> response = await _service.UpdateUserAsync(id, User).ConfigureAwait(false);
            return ReplyBaseResponse(response);
        }

        /// <summary>
        /// This function delete User by given id
        /// </summary>
        /// <param name="id">Specify Id of User</param>
        /// <returns>BaseResponse with UserDto</returns>        
        /// <response code="200">Successfully added the User</response>      
        /// <response code="404">User with the given id not found</response>
        /// <response code="500">Internal server error</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserDto>> DeleteUserByIdAsync([FromRoute] int id)
        {
            BaseResponse<UserDto> response = await _service.DeleteUserByIdAsync(id).ConfigureAwait(false);
            return ReplyBaseResponse(response);
        }
    }
}
